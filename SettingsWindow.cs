using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace deskdecorator
{
    public partial class SettingsWindow : Form
    {
        public SettingsWindow()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = Program.gLanguage;
            //pre-check/uncheck the box for autostart
            if (tools.Utils.IsInStartup("DeskDecorator")) checkBox1.Checked = true;

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked && !tools.Utils.IsInStartup("DeskDecorator"))
            {
                tools.Utils.AddToStartup("DeskDecorator");
            }
            else if (!checkBox1.Checked && tools.Utils.IsInStartup("DeskDecorator"))
            {
                tools.Utils.RemoveFromStartup("DeskDecorator");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            int selectedLang = comboBox1.SelectedIndex;
            if (Program.gLanguage == selectedLang) return;
            
            switch(selectedLang)
            {
                case 0:
                    var key = Registry.CurrentUser.CreateSubKey(@"Software\DeskDecorator");
                    key?.SetValue("Language", 0, RegistryValueKind.DWord);
                    break;
                case 1:
                    key = Registry.CurrentUser.CreateSubKey(@"Software\DeskDecorator");
                    key?.SetValue("Language", 1, RegistryValueKind.DWord);
                    break;
            }

            Program.gLanguage = selectedLang;
            Application.Restart();
            Application.Exit(); // Clean shutdown

        }
    }
}
