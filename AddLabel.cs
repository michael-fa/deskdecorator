using deskdecorator.DekstopEngine;
using deskdecorator.DekstopEngine.DesktopElements;
using System.Diagnostics;
using System.Windows.Forms;
using LabelAlias = deskdecorator.DekstopEngine.DesktopElements.Label;

namespace deskdecorator
{
    public partial class AddLabel : Form
    {
        string _labelname = "";
        string _labeltext = "";
        bool _dynamic = false;

        bool fromContext = false;
        bool edit = false;
        Font tmpFont = null!;
        Form1 origin;

        private LabelAlias? gEditLabel;
        private LabelAlias? gLabel;

        public AddLabel(Form1 origin_)
        {
            InitializeComponent();
            origin = origin_;
            checkTextInput(null!, null!);
        }

        public AddLabel(Form1 origin_, bool fromcont = false)
        {
            InitializeComponent();
            origin = origin_;
            fromContext = fromcont;
            btn_SetPosition.Text = Language.Get("Create_here");
            checkTextInput(null!, null!);
        }

        public AddLabel(string name, string txt, bool dynamic, LabelAlias lbl)
        {
            InitializeComponent();

            _labelname = name;
            _labeltext = txt;
            _dynamic = dynamic;
            gLabel = lbl;

            textBox1.Text = _labelname;
            richTextBox1.Text = _labeltext;
            checkBox1.Checked = _dynamic;
            this.Text =  Language.Get("crlabel_editlabelversion") + $"'{_labelname}'";

            tmpFont = lbl._label?.Font ?? new Font("Segoe UI", 12);
            edit = true;
            checkTextInput(null!, null!);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Language.Get("helptext"), Language.Get("Help"));
        }

        private void OnBtn_SetPosition(object sender, EventArgs e)
        {
            if (edit && gLabel != null)
            {
                gLabel.Description = textBox1.Text;
                gLabel._text = richTextBox1.Text;
                gLabel.Save();
                gLabel._label!.Text = richTextBox1.Text;
                StartMouseTracking(gLabel);
                return;
            }

            if (fromContext)
            {
                tmpFont ??= new Font("Segoe UI", 12);
                var newLabelx = new LabelAlias(textBox1.Text, richTextBox1.Text, checkBox1.Checked, tmpFont);

                Invoke(() =>
                {
                    newLabelx.CreateControl(Point.Empty);
                    Manager.Label_Add(newLabelx);
                });

                WindowState = FormWindowState.Minimized;
                Hide();

                StartMouseTracking(newLabelx);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(richTextBox1.Text))
                return;

            if (Engine.ElementNameTable.Contains(textBox1.Text))
            {
                MessageBox.Show($"'{textBox1.Text}' " + Language.Get("crlabel_001"), Language.Get("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox1.Text.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                MessageBox.Show(Language.Get("crlabel_002"), Language.Get("crlabel_invalidname"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            tmpFont ??= new Font("Segoe UI", 12);
            var newLabel = new LabelAlias(textBox1.Text, richTextBox1.Text, checkBox1.Checked, tmpFont);
            gEditLabel = newLabel;

            Invoke(() =>
            {
                newLabel.CreateControl(Point.Empty);
                Manager.Label_Add(newLabel);
            });

            tools.Utils.RefreshComboBox();
            StartMouseTracking(newLabel);
        }

        private void StartMouseTracking(LabelAlias label)
        {
            Thread thread = new(() =>
            {
                Debug.WriteLine("[INFO] Mouse pointer thread started");

                while (Engine.m_Running)
                {
                    Point mousePos = Cursor.Position;

                    Invoke(() =>
                    {
                        if (label._label != null)
                        {
                            Point clientPos = Engine._form.PointToClient(mousePos);
                            label._label.Location = clientPos;
                        }
                    });

                    if (Control.MouseButtons == MouseButtons.Left)
                    {
                        Invoke(() =>
                        {
                            if (label._label != null)
                            {
                                label._text = richTextBox1.Text;
                                label.Description = textBox1.Text;
                                label._label.Text = richTextBox1.Text;
                                label.Position = label._label.Location;
                                label.Save();

                                if (!Engine.combox_EditItemSelector.Items.Contains(label.Description))
                                    Engine.combox_EditItemSelector.Items.Add(label.Description);

                                Engine.combox_EditItemSelector.SelectedItem = label.Description;
                                Debug.WriteLine("[INFO] Mouse pointer thread finished.");
                                Dispose();
                            }
                        });
                        break;
                    }

                    Thread.Sleep(80);
                }
            });

            thread.IsBackground = true;
            thread.Start();
        }

        private void checkTextInput(object sender, EventArgs e)
        {
            bool hasText = !string.IsNullOrWhiteSpace(richTextBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text);
            btn_SetPosition.Enabled = hasText;
            btn_SetPosition.BackColor = hasText ? Color.DarkGreen : Color.DarkRed;
        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {
            tmpFont = fontDialog1.Font;
        }

        private void AddLabel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (edit && e.KeyChar == (char)Keys.Escape && gEditLabel?._label != null)
            {
                gEditLabel._label.Hide();
                Dispose(true);
                Close();
            }
        }

        private void btn_FontSettings_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                tmpFont = fontDialog1.Font;

                if (gLabel != null)
                {
                    gLabel.SetFont(tmpFont);
                    gLabel.Save();
                }
            }
        }
    }
}
