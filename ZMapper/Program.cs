using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace ZMapper
{
    static class Program
    {
        internal static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        internal const string AppDirName = "ZMapper";
        internal const string SettingsFilename = "settings.json";
        internal static readonly string ZMapperData = Path.Combine(AppData, AppDirName);
        internal static readonly string SettingsPath = Path.Combine(ZMapperData, SettingsFilename);

        internal static Settings AppSettings;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            LoadSettings();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            SaveSettings();
        }

        public static void SaveSettings() {
            try {
                if (!Directory.Exists(ZMapperData)) Directory.CreateDirectory(ZMapperData);
                File.WriteAllText(SettingsPath, AppSettings.Serialize());
            } catch (Exception ex) {
                ShowSettingsError("write", ex);
                return;
            }
        }

        private static void LoadSettings() {
            try {
                if (File.Exists(SettingsPath)) {
                    var settingsText = File.ReadAllText(SettingsPath);
                    AppSettings = Settings.Deserialize(settingsText);
                }
            } catch (Exception ex) {
                ShowSettingsError("read", ex);
                return;
            } finally {
                if (AppSettings == null) AppSettings = new Settings();
            }


        }

        private static void ShowSettingsError(string operation, Exception ex) {
            MessageBox.Show("Failed to " + operation + " settings", "Failed To Save Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
