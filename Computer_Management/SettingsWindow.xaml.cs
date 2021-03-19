using System;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Diagnostics;

namespace Computer_Management
{
    public partial class SettingsWindow : Window
    {
        public string[] Months { get; private set; }

        public SettingsWindow()
        {
            InitializeComponent();
            string[] months = { "1 month", "2 months", "3 months", "4 months", "5 months", "6 months", "7 months", "8 months", "9 months", "10 months", "11 months", "12 months", };
            monthsComboBox.ItemsSource = months;
            monthsComboBox.SelectedIndex = Settings.Default.Month;
            dataPathLabel.Content = Settings.Default.DataPath;
            string dataPath = Settings.Default.DataPath;
            if (Settings.Default.DataPath.Length > 45) { ToolTipTxtBox.ToolTip = dataPath; dataPathLabel.ToolTip = dataPath; }
        }

        private void dataPathClick(object sender, MouseButtonEventArgs e)
        {
            try { Process.Start(dataPathLabel.Content.ToString().Remove(dataPathLabel.Content.ToString().Length - 8, 8)); }
            catch { MsgBoxEditor.EditErrorMessage("Datapath not found...", "Error"); }
        }

        private void dataPathPic_MouseDown(object sender, MouseButtonEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    string newPath = Path.Combine(dialog.SelectedPath, "Data.csv");
                    if (!File.Exists(newPath))
                        MsgBoxEditor.EditErrorMessage("Datafile doesn't exist in this directory...!", "");
                    else 
                    {
                        dataPathLabel.Content = newPath;
                        if (newPath.Length > 45) { ToolTipTxtBox.ToolTip = newPath; dataPathLabel.ToolTip = newPath; }
                    }
                }
            }
        }

        // --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- |
        private void RDBTN_Click(object sender, RoutedEventArgs e)
        {
            dataPathLabel.Content = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Data.csv");
            monthsComboBox.SelectedIndex = 2;
        }
        private void OkBTN_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.DataPath = dataPathLabel.Content.ToString().Trim();
            Settings.Default.Month = byte.Parse(monthsComboBox.SelectedIndex.ToString());
            Settings.Default.Save();
            SettingsClass.Save();
            this.Close();
        }
    }
}