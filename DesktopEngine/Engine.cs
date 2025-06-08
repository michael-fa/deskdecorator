using deskdecorator.DekstopEngine.DesktopElements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace deskdecorator.DekstopEngine
{
    internal static class Engine
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        public static bool m_Running;
        public static Logger _logger = null!;
        public static Form _form = null!;
        public static bool DT_Toggle = false;
        public static DomainUpDown combox_EditItemSelector = null!;

        public static List<string> ElementNameTable = new();

        public static bool Init(Form1 form, Logger log)
        {
            Debug.WriteLine("Engine init...");

            if (log == null || form == null)
                return false;

            _logger = log;
            _form = form;

            try
            {
                // Finde das Desktop-Fenster
                IntPtr hWndDesktop = FindWindow("Progman", null!);
                if (hWndDesktop == IntPtr.Zero)
                {
                    Debug.WriteLine("[INFO] Progman window not found.");
                }
                else
                {
                    SetParent(form.Handle, hWndDesktop);
                    Debug.WriteLine("[INFO] Form successfully attached to Progman.");
                }
            }
            catch (Exception e)
            {
                _logger.LogException(e);
                Debug.WriteLine("[ERROR] Engine attachment failed: " + e.Message);
            }

            // Save-Verzeichnis sicherstellen
            if (!Directory.Exists(Manager.SaveDirectory))
            {
                try
                {
                    Directory.CreateDirectory(Manager.SaveDirectory);
                    Debug.WriteLine("[INFO] Created save directory at: " + Manager.SaveDirectory);
                }
                catch (Exception ex)
                {
                    _logger.LogException(ex);
                    return false;
                }
            }

            m_Running = true;

            try
            {
                Manager.LoadElements();     // <-- Labels laden
                Manager.UpdateLabels();     // <-- Variablen anwenden (falls dynamisch)
            }
            catch (Exception e)
            {
                _logger.LogException(e);
                Debug.WriteLine("[ERROR] Failed to load elements: " + e.Message);
            }

            //RPC's
            NetworkLabelReceiver.StartListening(form);

            return true;
        }

        /// <summary>
        /// Liefert ein Label anhand der Beschreibung zurück.
        /// </summary>
        public static DesktopElements.Label? GetLabelFromText(string elementName)
        {
            return Manager.Labels.FirstOrDefault(label => label.Description == elementName);
        }
    }
}
