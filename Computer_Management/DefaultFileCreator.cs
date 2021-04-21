using System;
using System.IO;
using System.Xml.Linq;

namespace Computer_Management
{
    public static class DefaultFileCreator
    {
        private static string DefaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management");
        private static string DefaultDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Data.xml");
        private static string SettingsPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Settings.xml"); } }

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
                System.Windows.Forms.Application.Exit();
            }

            // --- SETTINGS --- |
            try
            {
                if (!File.Exists(SettingsPath))
                    SettingsClass.CreateDefault();
            }
            catch
            {
                MsgBoxEditor.EditErrorMessage("Application couldn't be started...\nPlease check your authorization!\nError[0x00001010]", "Settings creating error");
                System.Windows.Forms.Application.Exit();
            }

            // --- DATA FILE --- |
            try
            {
                if (!File.Exists(DefaultDataPath)) 
                {
                    XDocument data = new XDocument(new XDeclaration("1.0", "UTF-8", null), new XElement("Computers"));
                    data.Save(DefaultDataPath);
                }
            }
            catch
            {
                MsgBoxEditor.EditErrorMessage("Application couldn't be started...\nPlease check your authorization!\nError[0x00001011]", "Data file creating error");
                System.Windows.Forms.Application.Exit();
            }
        }
    }
}
