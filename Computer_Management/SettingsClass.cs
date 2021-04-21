using System.Xml.Linq;

namespace Computer_Management
{
    public static class SettingsClass
    {
        private static string SettingsPath { get { return System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "Computer management", "Settings.xml"); } }
        private static object zamek = new object();

        public static void CreateDefault() 
        {
            lock (zamek) 
            {
                if (System.IO.File.Exists(SettingsPath))
                    System.IO.File.Delete(SettingsPath);

                string defaultDataPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "Computer management", "Data.xml");

                XDocument defaultSettings = new XDocument(new XDeclaration("1.0", "UTF-8", null), new XElement("Settings"));

                defaultSettings.Element("Settings").Add(new XElement("DataPath", defaultDataPath),
                                new XElement("Month", new XAttribute("number", 2)),
                                new XElement("SortingBy", new XAttribute("number", 0), new XComment("0 - Newest\n\t1 - Oldest\n\t2 - Maintenance\n\t3 - Name/Ascending\n\t4 - Name/Descending"))
                               );

                defaultSettings.Element("Settings").Add(new XElement("DarkMode", new XAttribute("enabled", false),
                                                    new XElement("Background", new XAttribute("color", "#FF151B25")),
                                                    new XElement("Midground", new XAttribute("color", "#FF202936")),
                                                    new XElement("Foreground", new XAttribute("color", "#FFFFFFFF")),
                                                    new XElement("Border", new XAttribute("color", "#FFFF7400"))
                                                 ));

                defaultSettings.Save(SettingsPath);
            }
            Load();
        }

        public static void Load() 
        {
            bool b = true;
            try 
            {
                XDocument XMLsettings = XDocument.Load(SettingsPath);

                Settings.Default.DataPath = XMLsettings.Element("Settings").Element("DataPath").Value;
                Settings.Default.PasteReplaceMonth = byte.Parse(XMLsettings.Element("Settings").Element("Month").Attribute("number").Value);
                Settings.Default.SortingBy = byte.Parse(XMLsettings.Element("Settings").Element("SortingBy").Attribute("number").Value);

                Settings.Default.IsDarkModeEnabled = bool.Parse(XMLsettings.Element("Settings").Element("DarkMode").Attribute("enabled").Value);
                Settings.Default.Background = XMLsettings.Element("Settings").Element("DarkMode").Element("Background").Attribute("color").Value;
                Settings.Default.Midground = XMLsettings.Element("Settings").Element("DarkMode").Element("Midground").Attribute("color").Value;
                Settings.Default.Foreground = XMLsettings.Element("Settings").Element("DarkMode").Element("Foreground").Attribute("color").Value;
                Settings.Default.BorderColor = XMLsettings.Element("Settings").Element("DarkMode").Element("Border").Attribute("color").Value;
            }
            catch { new SureWindow("LoadSettings").ShowDialog(); b = false; }

            if(b)
                Settings.Default.Save();
        }

        public static void Save()
        {
            XDocument XMLsettings = new XDocument(new XDeclaration("1.0", "UTF-8", null), new XElement("Settings"));

            lock (zamek) 
            {
                XMLsettings.Element("Settings").Add(new XElement("DataPath", Settings.Default.DataPath),
                            new XElement("Month", new XAttribute("number", Settings.Default.PasteReplaceMonth)),
                            new XElement("SortingBy", new XAttribute("number", Settings.Default.SortingBy), new XComment("0 - Newest\n\t1 - Oldest\n\t2 - Maintenance\n\t3 - Name/Ascending\n\t4 - Name/Descending"))
                           );

                XMLsettings.Element("Settings").Add(new XElement("DarkMode", new XAttribute("enabled", Settings.Default.IsDarkModeEnabled),
                                                    new XElement("Background", new XAttribute("color", Settings.Default.Background)),
                                                    new XElement("Midground", new XAttribute("color", Settings.Default.Midground)),
                                                    new XElement("Foreground", new XAttribute("color", Settings.Default.Foreground)),
                                                    new XElement("Border", new XAttribute("color", Settings.Default.BorderColor))
                                                 ));
                // + PASTAS -name as attribute -> make list
            }

            XMLsettings.Save(SettingsPath);
        }
    }
}
