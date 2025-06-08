// FILE 2: Label.cs
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using deskdecorator.tools;

namespace deskdecorator.DekstopEngine.DesktopElements
{
    public class Label
    {
        [DllImport("user32.dll")] private static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32.dll")] private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        public string Description { get; set; } = "New Element";
        public Point Position { get; set; } = new Point(100, 100);
        public ELEMENT_TYPE Type { get; set; } = ELEMENT_TYPE.LABEL;
        private bool Dynamic { get; set; } = false;
        private Font _font;
        private bool keepUpdating = true;

        public tools.Properties safefile { get; set; } = null!;
        public System.Windows.Forms.Label? _label;
        public string _text;

        public Label(string description, string text, bool dynamic, Font? font = null)
        {
            _font = font ?? new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            Description = description;
            Dynamic = dynamic;
            _text = text;
            safefile = new tools.Properties(Path.Combine(Application.StartupPath, "saved_elements", $"{description.Replace(" ", "_")}.element"));
            Engine.ElementNameTable.Add(description);
        }

        public Label(string description, string text, tools.Properties prop, bool dynamic)
        {
            Description = description;
            Dynamic = dynamic;
            _text = text;
            safefile = prop;
            _font = ReadFontFromFile(safefile, "fontdata");
            Engine.ElementNameTable.Add(description);
        }

        public void SetFont(Font f)
        {
            _font = f;
            if (_label != null)
                _label.Font = f;
        }

        public virtual void Update()
        {
            if (_label == null) return;

            _label.Text = _text.Replace("{time1}", DateTime.Now.ToString("HH:mm"))
                .Replace("{time2}", DateTime.Now.ToString("HH:mm:ss"))
                .Replace("{date1}", DateTime.Now.ToString("dd/MM/yyyy"))
                .Replace("{date2}", DateTime.Now.ToString("MM/dd/yyyy"))
                .Replace("{date3}", DateTime.Now.ToString("yyyy-MM-dd"))
                .Replace("{username}", Environment.UserName)
                .Replace("{computername}", Environment.MachineName)
                .Replace("\\n", Environment.NewLine);
        }

        public void Save()
        {
            try
            {
                safefile.set("type", 0);
                safefile.set("description", Description);
                safefile.set("positionX", Position.X);
                safefile.set("positionY", Position.Y);
                safefile.set("dynamic", Dynamic.ToString());
                safefile.set("text", _text);

                Font fontToSave = _label?.Font ?? _font;
                string fontData = $"{fontToSave.FontFamily.Name}|{fontToSave.Size}|{fontToSave.Bold}|{fontToSave.Italic}|{fontToSave.Underline}";
                safefile.set("fontdata", fontData);

                string path = Path.Combine(Application.StartupPath, "saved_elements", $"{Description.Replace(" ", "_")}.element");
                safefile.Save(path);

                Debug.WriteLine($"[INFO] Label saved: {Description} at {Position} with text '{_text}'");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Error saving label '{Description}': {ex.Message}");
            }
        }

        public void Delete()
        {
            try
            {
                File.Delete(Path.Combine(Application.StartupPath, "saved_elements", $"{Description.Replace(" ", "_")}.element"));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Error deleting label file: {ex.Message}");
            }

            _label?.Hide();
            _label?.Dispose();
            keepUpdating = false;

            Engine.ElementNameTable.Remove(Description);
            Manager.Labels.Remove(this);

            if (Engine.combox_EditItemSelector != null)
            {
                Engine.combox_EditItemSelector.Items.Clear();

                foreach (var element in Manager.Labels)
                    Engine.combox_EditItemSelector.Items.Add(element.Description);
            }
        }

        public void CreateControl(Point point)
        {
            _label = new System.Windows.Forms.Label
            {
                AutoSize = true,
                Font = _font,
                ForeColor = Color.White,
                Location = point,
                Name = Description.Replace(" ", "_"),
                Size = new Size(212, 32),
                TabIndex = 0,
                UseCompatibleTextRendering = true,
                Text = _text,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Engine._form.Controls.Add(_label);
            _label.Show();

            if (Dynamic)
            {
                Thread thread = new(() =>
                {
                    while (keepUpdating && Engine.m_Running)
                    {
                        Engine._form.BeginInvoke(new Action(Update));
                        Thread.Sleep(1000);
                    }
                });

                thread.IsBackground = true;
                thread.Start();
            }

            Update();
        }

        public Font ReadFontFromFile(tools.Properties properties, string key)
        {
            string fontData = properties.get(key, null!);
            if (string.IsNullOrEmpty(fontData))
                return new Font("Arial", 12);

            var parts = fontData.Split('|');
            if (parts.Length != 5)
                return new Font("Arial", 12);

            try
            {
                string fontFamily = parts[0];
                float fontSize = float.Parse(parts[1]);
                bool isBold = bool.Parse(parts[2]);
                bool isItalic = bool.Parse(parts[3]);
                bool isUnderline = bool.Parse(parts[4]);

                FontStyle style = FontStyle.Regular;
                if (isBold) style |= FontStyle.Bold;
                if (isItalic) style |= FontStyle.Italic;
                if (isUnderline) style |= FontStyle.Underline;

                return new Font(fontFamily, fontSize, style);
            }
            catch
            {
                return new Font("Arial", 12);
            }
        }
    }
}
