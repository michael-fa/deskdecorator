using deskdecorator.DekstopEngine.DesktopElements;
using deskdecorator.DekstopEngine;
using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace deskdecorator.tools
{
    static public class Utils
    {
        private const string RUN_KEY = @"Software\Microsoft\Windows\CurrentVersion\Run";

        /// <summary>
        /// Fügt die Anwendung dem Windows-Autostart hinzu.
        /// </summary>
        public static void AddToStartup(string appName)
        {
            try
            {
                string exePath = Application.ExecutablePath;

                using RegistryKey key = Registry.CurrentUser.OpenSubKey(RUN_KEY, writable: true)!;
                key.SetValue(appName, exePath);

                Debug.WriteLine($"{appName} erfolgreich zum Autostart hinzugefügt.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Fehler beim Hinzufügen zum Autostart: {ex.Message}");
            }
        }

        /// <summary>
        /// Prüft, ob die Anwendung bereits im Autostart ist.
        /// </summary>
        public static bool IsInStartup(string appName)
        {
            try
            {
                using RegistryKey key = Registry.CurrentUser.OpenSubKey(RUN_KEY, writable: false)!;
                return key.GetValue(appName) != null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Fehler beim Prüfen des Autostarts: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Entfernt die Anwendung aus dem Autostart.
        /// </summary>
        public static void RemoveFromStartup(string appName)
        {
            try
            {
                using RegistryKey key = Registry.CurrentUser.OpenSubKey(RUN_KEY, writable: true)!;

                if (key.GetValue(appName) != null)
                {
                    key.DeleteValue(appName);
                    Debug.WriteLine($"[INFO] {appName} aus dem Autostart entfernt.");
                }
                else
                {
                    Debug.WriteLine($"[INFO] {appName} war nicht im Autostart eingetragen.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] Fehler beim Entfernen aus dem Autostart: {ex.Message}");
            }
        }

        public static void RefreshComboBox()
        {
            Engine.combox_EditItemSelector.Items.Clear();

            foreach (var element in Manager.Labels)
                Engine.combox_EditItemSelector.Items.Add(element.Description);

            if (Engine.combox_EditItemSelector.Items.Count == 0)
                Engine.combox_EditItemSelector.Items.Add("No item selected");

            Engine.combox_EditItemSelector.SelectedIndex = 0;
            Debug.WriteLine($"[INFO] Item Selection refreshed!");
        }
    }
}
