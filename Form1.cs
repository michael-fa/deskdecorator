using deskdecorator.DekstopEngine;
using deskdecorator.DekstopEngine.DesktopElements;
using deskdecorator.Properties;
using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace deskdecorator
{
    public partial class Form1 : Form
    {
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        private NotifyIcon notifyIcon = null!;
        private ContextMenuStrip contextMenu = null!;
        public DomainUpDown combox_EditItemSelector = null!;
        public ToolStripButton combobox_DTButton = null!;

        public Logger log;
       

        public Form1(Logger logger)
        {
            InitializeComponent();
            log = logger;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;

            foreach (Screen screen in Screen.AllScreens)
            {
                minX = Math.Min(minX, screen.Bounds.Left);
                minY = Math.Min(minY, screen.Bounds.Top);
                maxX = Math.Max(maxX, screen.Bounds.Right);
                maxY = Math.Max(maxY, screen.Bounds.Bottom);
            }

            int totalWidth = maxX - minX;
            int totalHeight = maxY - minY;

            this.Location = new Point(minX, minY);
            this.Size = new Size(totalWidth, totalHeight);

            log.Log("[INFO] Calculated screen span: ", $"{totalWidth}x{totalHeight} at offset {minX},{minY}");


            //For better logging overall.
            try
            {
                Engine.Init(this, log);
            }
            catch (Exception ex)
            {
                log.LogException(ex);
            }

            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(@"Software\DeskDecorator");
                if (key != null)
                {
                    object value = key.GetValue("Language");
                    if (value is int lang)
                        Program.gLanguage = lang;
                    if (int.TryParse(value?.ToString(), out int parsed))
                        Program.gLanguage = parsed;
                }
            }
            catch { }


            InitializeTrayIcon();
            RegisterExplorerContextMenu();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Engine.m_Running = false;
            Manager.SaveElements();
        }

        private void InitializeTrayIcon()
        {
            contextMenu = new ContextMenuStrip();

            // Element-Auswahlfeld
            combox_EditItemSelector = new DomainUpDown
            {
                ReadOnly = true,
                ForeColor = Color.DarkGray
            };

            foreach (var element in Manager.Labels)
                combox_EditItemSelector.Items.Add(element.Description);

            if (combox_EditItemSelector.Items.Count == 0)
                combox_EditItemSelector.Items.Add(Language.Get("systray_noitemsel"));

            combox_EditItemSelector.SelectedIndex = 0;
            combox_EditItemSelector.SelectedItemChanged += ComboBox_SelectedItemChanged;

            // Menüeinträge
            contextMenu.Items.Add(new ToolStripMenuItem(Language.Get("systray_crnwlbl"), null, Click_Add_Label));
            //contextMenu.Items.Add(new ToolStripMenuItem("Add -> Image", null, Click_Add_Image));
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(new ToolStripControlHost(combox_EditItemSelector));
            contextMenu.Items.Add(new ToolStripMenuItem(Language.Get("systray_edelmt"), null, Click_EditElement));
            contextMenu.Items.Add(new ToolStripMenuItem(Language.Get("systray_delelement"), null, Click_DeleteElement));
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(new ToolStripMenuItem(Language.Get("systray_settings"), null, Click_Settings));
            contextMenu.Items.Add(new ToolStripMenuItem(Language.Get("systray_credits"), null, Click_Credits));
            contextMenu.Items.Add(new ToolStripMenuItem(Language.Get("systray_close"), null, Click_Close));

            // Notify-Icon
            notifyIcon = new NotifyIcon
            {
                Icon = Properties.Resources.icon,
                Text = "Desk Decorator",
                Visible = true,
                ContextMenuStrip = contextMenu
            };

            Engine.combox_EditItemSelector = combox_EditItemSelector;
        }

        private void RegisterExplorerContextMenu()
        {
            try
            {
                // Define registry path for the Widget submenu (under Directory\Background\shell)
                string widgetKeyPath = @"Software\Classes\Directory\Background\shell\Widget";
                // Try to open the Widget key
                using (RegistryKey widgetKey = Registry.CurrentUser.OpenSubKey(widgetKeyPath, writable: true)
                                                ?? Registry.CurrentUser.CreateSubKey(widgetKeyPath))
                {
                    if (widgetKey == null)
                    {
                        // If we cannot create or open, just return (no permissions?)
                        return;
                    }
                    // Set up the Widget submenu properties
                    widgetKey.SetValue("MUIVerb", "Widget");           // Display name for submenu:contentReference[oaicite:16]{index=16}
                    widgetKey.SetValue("SubCommands", "");             // Indicates this is a cascaded menu:contentReference[oaicite:17]{index=17}
                                                                       // (Optional) widgetKey.SetValue("Icon", $"\"{Application.ExecutablePath}\",0"); // use app icon

                    // Ensure the Shell subkey exists for submenu items
                    using (RegistryKey shellKey = widgetKey.CreateSubKey("Shell"))
                    {
                        // Current application path (quoted) for commands
                        string exePath = Application.ExecutablePath;
                        string labelCmd = $"\"{exePath}\" --add-label";
                        string imageCmd = $"\"{exePath}\" --add-image";

                        // **Label** submenu item
                        using (RegistryKey labelKey = shellKey.CreateSubKey("Label"))
                        {
                            labelKey.SetValue("", "Label");  // default value is item name:contentReference[oaicite:18]{index=18}
                            using (RegistryKey cmdKey = labelKey.CreateSubKey("command"))
                            {
                                cmdKey.SetValue("", labelCmd);  // command to execute (exe path + argument)
                            }
                        }
                        /*// **Image** submenu item
                        using (RegistryKey imageKey = shellKey.CreateSubKey("Image"))
                        {
                            imageKey.SetValue("", "Image");
                            using (RegistryKey cmdKey = imageKey.CreateSubKey("command"))
                            {
                                cmdKey.SetValue("", imageCmd);
                            }
                        }*/
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Language.Get("form1_001") + $"\n{ex.Message}", Language.Get("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            combox_EditItemSelector.ForeColor = Color.Blue;
        }

        private void Click_Add_Label(object? sender, EventArgs e)
        {
            var dialog = new AddLabel(this);
            dialog.Show();
        }

        /*private void Click_Add_Image(object? sender, EventArgs e)
        {
            var dialog = new AddImage(this);
            dialog.Show();
        }*/

        private void Click_EditElement(object? sender, EventArgs e)
        {
            if (combox_EditItemSelector.SelectedItem == null || combox_EditItemSelector.SelectedItem.ToString() == "No item selected")
            {
                MessageBox.Show(Language.Get("form1_002"), Language.Get("systray_noitemsel"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var label = Engine.GetLabelFromText(combox_EditItemSelector.SelectedItem.ToString()!);
            if (label == null)
            {
                MessageBox.Show(Language.Get("form1_003"), Language.Get("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var dialog = new AddLabel(label.Description, label._text, true, label);
            dialog.Show();
        }

        private void Click_DeleteElement(object? sender, EventArgs e)
        {
            if (combox_EditItemSelector.SelectedItem == null || combox_EditItemSelector.SelectedItem.ToString() == "No item selected")
            {
                MessageBox.Show(Language.Get("form1_004"), Language.Get("Notice"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var label = Engine.GetLabelFromText(combox_EditItemSelector.SelectedItem.ToString()!);
            if (label == null)
            {
                MessageBox.Show(Language.Get("form1_005"), Language.Get("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var confirm = MessageBox.Show(Language.Get("form1_006"), Language.Get("form1_007"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {


                combox_EditItemSelector.Items.Remove(label.Description);
                combox_EditItemSelector.ForeColor = Color.DarkGray;

                if (combox_EditItemSelector.Items.Count == 0)
                    combox_EditItemSelector.Items.Add(Language.Get("form1_004"));

                combox_EditItemSelector.SelectedIndex = 0;

                label.Delete();
            }
        }

        private void Click_Settings(object? sender, EventArgs e)
        {
            SettingsWindow settings = new SettingsWindow();
            settings.Show();
        }

        private void Click_Credits(object? sender, EventArgs e)
        {
            MessageBox.Show(Language.Get("form1_008"), Language.Get("systray_credits"));
        }

        private void Click_Close(object? sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            Application.Exit();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // optional: Debug-Visualisierung
        }
    }
}
