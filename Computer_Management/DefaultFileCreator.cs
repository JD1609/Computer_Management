using System;
using System.IO;

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
                MsgBoxEditor.EditErrorMessage("Application couldn't be started...\nPlease check your authorization!\nError[0x00001001]", "Folder creating error");
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
                MsgBoxEditor.EditErrorMessage("Application couldn't be started...\nPlease check your authorization!\nError[0x00001001]", "Settings creating error");
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
                MsgBoxEditor.EditErrorMessage("Application couldn't be started...\nPlease check your authorization!\nError[0x00001010]", "Data file creating error");
                App.Current.Shutdown();
            }
        }
    }
}
