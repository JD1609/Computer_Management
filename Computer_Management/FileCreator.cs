using System;
using System.IO;

namespace Computer_Management
{
    public static class Creator
    {
        private static string DefaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management");
        private static string DefaultDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Data.csv");
        private static string SettingsPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Settings.settings"); } }

        public static void DataCheck() 
        {
            if (!Directory.Exists(DefaultPath))
                Directory.CreateDirectory(DefaultPath);

            if (!File.Exists(SettingsPath))
                File.Create(SettingsPath);

            if (!File.Exists(DefaultDataPath))
                File.Create(DefaultDataPath);
        }
    }
}
