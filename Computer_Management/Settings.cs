using System;
using System.IO;

namespace Computer_Management
{
    public static class Settings
    {
        public static string DefaultDataPath() { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Data.csv"); }

        public static string DataPath()
        {
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Settings.csv")))
            {
                string r = "";
                using (StreamReader streamreader = new StreamReader(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Settings.csv")))
                {
                    if (streamreader.ReadLine() != null)
                    {
                        string s;
                        while ((s = streamreader.ReadLine()) != null)
                        {
                            string[] splitted = s.Split(';');
                            r = splitted[0].Trim();
                        }
                    }
                }
                return r;
            }
            else
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Data.csv");
            }
        }

        public static byte GetMonth()
        {
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Settings.csv")))
            {
                byte r = 0;
                using (StreamReader streamreader = new StreamReader(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Settings.csv")))
                {
                    if (streamreader.ReadLine() != null)
                    {
                        string s;
                        while ((s = streamreader.ReadLine()) != null)
                        {
                            string[] splitted = s.Split(';');
                            r = byte.Parse(splitted[1]);
                        }
                    }
                }
                return r;
            }
            else
            {
                return 2;
            }
        }


        public static void RestoreDefault(SettingsWindow swindow) 
        {
            string defaultDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Data.csv");
            string settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Settings.csv");

            if (swindow == null) 
            {
                
                using (StreamWriter sw = new StreamWriter(settingsPath))
                {
                    sw.WriteLine(String.Format("{0};{1};", defaultDataPath, 2));
                    sw.Flush();
                }
            }
            else 
            {
                swindow.monthsComboBox.SelectedIndex = 2;
                swindow.dataPathLabel.Content = defaultDataPath;
            }
        }

        public static void SaveSettings(SettingsWindow swindow)
        { 
            string settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Settings.csv");
            using (StreamWriter sw = new StreamWriter(settingsPath)) 
            {
                sw.WriteLine(String.Format("{0};{1};{2};", swindow.dataPathLabel.Content, swindow.monthsComboBox.SelectedIndex, swindow.CrossPlatform));
                sw.Flush();
            }
        }
    }
}
