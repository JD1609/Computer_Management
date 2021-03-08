using System;
using System.IO;
using System.Windows;

namespace Computer_Management
{
    public static class DefaultFileCreator
    {
        private static string DefaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management");
        private static string DefaultDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Data.csv");
        private static string SettingsPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Settings.settings"); } }

        public static void DataCheck() 
        {
            // --- FOLDER --- |
            try 
            {
                if (!Directory.Exists(DefaultPath))
                    Directory.CreateDirectory(DefaultPath);
            }
            catch
            {
                MessageBox.Show("Application couldn't be started...\nPlease check your authorization!" + MsgBoxEditor.EditText("\nError[0x00001001]"), "Folder creating error", MessageBoxButton.OK, MessageBoxImage.Error);
                App.Current.Shutdown();
            }

            // --- SETTINGS --- |
            try
            {
                if (!File.Exists(SettingsPath))
                    File.Create(SettingsPath);
            }
            catch
            {
                MessageBox.Show("Application couldn't be started...\nPlease check your authorization!" + MsgBoxEditor.EditText("\nError[0x00001001]"), "Settings creating error", MessageBoxButton.OK, MessageBoxImage.Error);
                App.Current.Shutdown();
            }

            // --- DATA FILE --- |
            try
            {
                if (!File.Exists(DefaultDataPath))
                    File.Create(DefaultDataPath);
            }
            catch
            {
                MessageBox.Show("Application couldn't be started...\nPlease check your authorization!" + MsgBoxEditor.EditText("\nError[0x00001010]"), "Data file creating error", MessageBoxButton.OK, MessageBoxImage.Error);
                App.Current.Shutdown();
            }
        }
    }
}
