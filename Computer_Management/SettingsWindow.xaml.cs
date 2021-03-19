using System;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;

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
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = "";
            dlg.DefaultExt = ".cvs";
            dlg.Filter = ".csv|*csv*|.txt|*txt*";
            dlg.FileName = "Data";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                dataPathLabel.Content = dlg.FileName;
                if (dataPathLabel.Content.ToString().Length > 45) { ToolTipTxtBox.ToolTip = dlg.FileName; dataPathLabel.ToolTip = dlg.FileName; }
                Settings.Default.DataPath = dlg.FileName;
                Settings.Default.Save();
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