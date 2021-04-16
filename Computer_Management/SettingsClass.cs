using System;
using System.IO;

namespace Computer_Management
{
    public static class SettingsClass
    {
        private static string DefaultDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Data.xml");
        private static string SettingsPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Settings.settings"); } }
        private static object zamek = new object();
        
        public static void Load(string s) 
        {
            if (s == "Path") 
            {
                lock (zamek)
                {
                    using (StreamReader sr = new StreamReader(SettingsPath))
                    {
                        string line = sr.ReadLine();
                        if (line != null)
                        {
                            string[] splitted = line.Split(';');
                            Settings.Default.DataPath = splitted[0];
                            Settings.Default.Save();
                        }
                        else
                        {
                            Settings.Default.DataPath = DefaultDataPath;
                            Settings.Default.PasteReplaceMonth = 2;
                            Settings.Default.Save();
                        }
                    }
                }
            }

            if (s == "Month")
            {
                lock (zamek)
                {
                    using (StreamReader sr = new StreamReader(SettingsPath))
                    {
                        string line = sr.ReadLine();
                        if (line != null)
                        {
                            string[] splitted = line.Split(';');
                            Settings.Default.PasteReplaceMonth = byte.Parse(splitted[1]);
                            Settings.Default.Save();
                        }
                        else
                        {
                            Settings.Default.DataPath = DefaultDataPath;
                            Settings.Default.PasteReplaceMonth = 2;
                            Settings.Default.Save();
                        }
                    }
                }
            }

            if (s == "SortingBy")
            {
                lock (zamek)
                {
                    using (StreamReader sr = new StreamReader(SettingsPath))
                    {
                        string line = sr.ReadLine();
                        if (line != null)
                        {
                            string[] splitted = line.Split(';');
                            Settings.Default.SortingBy = byte.Parse(splitted[2]);
                            Settings.Default.Save();
                        }
                        else
                        {
                            Settings.Default.DataPath = DefaultDataPath;
                            Settings.Default.SortingBy = 0;
                            Settings.Default.Save();
                        }
                    }
                }
            }
        }

        public static void CorrectSettings()
        {
            lock (zamek)
            {
                string defaultDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Data.xml");
                Settings.Default.DataPath = defaultDataPath;
                Settings.Default.PasteReplaceMonth = 2;
                Settings.Default.SortingBy = 0;
                Settings.Default.Save();
            }

            lock (zamek)
            {
                Load("Path");
                Load("Month");
                Load("SortingBy");
            }
        }

        public static void Save() 
        {
            using (StreamWriter sw = new StreamWriter(SettingsPath))
            {
                sw.WriteLine("{0};{1};{2}", Settings.Default.DataPath, Settings.Default.PasteReplaceMonth, Settings.Default.SortingBy);
                sw.Flush();
            }
        }
    }
}
