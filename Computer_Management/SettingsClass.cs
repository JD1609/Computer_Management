using System;
using System.IO;
using System.Xml.Linq;

namespace Computer_Management
{
    public static class SettingsClass
    {
        private static string DefaultDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Data.xml");
        private static string SettingsPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Settings.xml"); } }
        private static object zamek = new object();
        
        public static void Load() 
        {
            Settings.Default.DataPath = DefaultDataPath;
            Settings.Default.PasteReplaceMonth = 2;
            Settings.Default.SortingBy = 0;
            Settings.Default.Save();
        }

        public static void Save()
        {
            XDocument settingsXML = new XDocument(new XElement("Settings"));

            settingsXML.Element("Settings").Add(new XElement("DataPath", Settings.Default.DataPath),
                            new XElement("Month", new XAttribute("number", Settings.Default.PasteReplaceMonth)),
                            new XElement("SortingBy", new XAttribute("number", Settings.Default.SortingBy), new XComment("0 - Newest\n\t1 - Oldest\n\t2 - Maintenance\n\t3 - Name/Ascending\n\t4 - Name/Descending"))
                           );

            settingsXML.Element("Settings").Add(new XElement("DarkMode", new XAttribute("enabled", Settings.Default.IsDarkModeEnabled),
                                                new XElement("Background", new XAttribute("color", Settings.Default.Background)),
                                                new XElement("Midground", new XAttribute("color", Settings.Default.Midground)),
                                                new XElement("Foreground", new XAttribute("color", Settings.Default.Foreground)),
                                                new XElement("Border", new XAttribute("color", Settings.Default.BorderColor))
                                             ));
            // + PASTAS -name as attribute -> make list
            settingsXML.Save(SettingsPath);
        }
    }
}
