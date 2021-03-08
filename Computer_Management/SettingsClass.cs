using System;
using System.IO;

namespace Computer_Management
{
    public static class SettingsClass
    {
        private static string DefaultDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Data.csv");
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
                            Settings.Default.Month = 2;
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
                            Settings.Default.Month = byte.Parse(splitted[1]);
                            Settings.Default.Save();
                        }
                        else
                        {
                            Settings.Default.DataPath = DefaultDataPath;
                            Settings.Default.Month = 2;
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
                string defaultDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Data.csv");
                Settings.Default.DataPath = defaultDataPath;
                Settings.Default.Month = 2;
                Settings.Default.Save();
            }

            lock (zamek)
            {
                Load("Path");
                Load("Month");
            }
        }

        public static void Save() 
        {
            using (StreamWriter sw = new StreamWriter(SettingsPath))
            {
                sw.WriteLine("{0};{1}", Settings.Default.DataPath, Settings.Default.Month);
                sw.Flush();
            }
        }
    }
}
