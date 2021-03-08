using System;
using System.IO;

namespace Computer_Management
{
    public static class SavingSettings
    {
        private static string SettingsPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Settings.settings"); } }
        public static string Load(string s) 
        {
            string output = "";
            if (s == "Path") 
            {
                using (StreamReader sr = new StreamReader(SettingsPath))
                {
                    if (sr.ReadLine() != null)
                    {
                        string[] splitted = sr.ReadLine().Split(';');
                        output = splitted[0];
                    }
                    else 
                    {
                        output = Settings.Default.DataPath;
                    }
                }
            }
            if (s == "Month") 
            {
                using (StreamReader sr = new StreamReader(SettingsPath))
                {
                    if (sr.ReadLine() != null)
                    {
                        string[] splitted = sr.ReadLine().Split(';');
                        output = splitted[0];
                    }
                    else
                    {
                        output = Settings.Default.Month.ToString();
                    }
                }
            }

            return output;
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
