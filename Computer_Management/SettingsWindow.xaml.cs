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
        MainWindow mw;
        public string[] Months { get; private set; }

        public SettingsWindow(MainWindow mw)
        {
            InitializeComponent();
            if (Settings.Default.IsDarkModeEnabled) { Dark_mode.SetDarkMode(this, Settings.Default.Background, Settings.Default.Midground, Settings.Default.Foreground); }
            this.mw = mw;
            string[] months = { "1 month", "2 months", "3 months", "4 months", "5 months", "6 months", "7 months", "8 months", "9 months", "10 months", "11 months", "12 months", };
            monthsComboBox.ItemsSource = months;
            sortingComboBox.SelectedIndex = Settings.Default.SortingBy;
            monthsComboBox.SelectedIndex = Settings.Default.PasteReplaceMonth;
            dataPathLabel.Content = Settings.Default.DataPath;
            changeDataPathLabel.Visibility = Visibility.Hidden;
            string dataPath = Settings.Default.DataPath;
            if (Settings.Default.DataPath.Length > 45) { ToolTipTxtBox.ToolTip = dataPath; dataPathLabel.ToolTip = dataPath; }
            if (Settings.Default.AfterStartUp)
                startUpCheckBox.IsChecked = true;
        }

        private void dataPathClick(object sender, MouseButtonEventArgs e)
        {
            try { Process.Start(Path.GetDirectoryName(dataPathLabel.Content.ToString())); }
            catch { MsgBoxEditor.EditErrorMessage("Datapath not found...", "Error"); }
        }

        private void dataPathPic_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = "";
            dlg.DefaultExt = ".xml";
            dlg.Filter = ".xml|*xml*|.txt|*txt*";
            dlg.FileName = "Data";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                changeDataPathLabel.Visibility = Visibility.Visible;
                dataPathLabel.Content = dlg.FileName;
                if (dataPathLabel.Content.ToString().Length > 45) { ToolTipTxtBox.ToolTip = dlg.FileName; dataPathLabel.ToolTip = dlg.FileName; }
                Settings.Default.DataPath = dlg.FileName;
                Settings.Default.Save();
            }
        }

        // --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- |
        private void RDBTN_Click(object sender, RoutedEventArgs e)
        {
            dataPathLabel.Content = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Data.xml");
            monthsComboBox.SelectedIndex = 2;
            sortingComboBox.SelectedIndex = 0;
            if (Settings.Default.DataPath != dataPathLabel.Content.ToString())
                changeDataPathLabel.Visibility = Visibility.Visible;
            startUpCheckBox.IsChecked = false;
        }
        private void OkBTN_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.DataPath = dataPathLabel.Content.ToString().Trim();
            Settings.Default.PasteReplaceMonth = byte.Parse(monthsComboBox.SelectedIndex.ToString());
            Settings.Default.SortingBy = byte.Parse(sortingComboBox.SelectedIndex.ToString());
            Settings.Default.Save();
            SettingsClass.Save();
            Database database = new Database(mw);
            database.LoadData(Settings.Default.DataPath);
            this.Close();
        }

        private void startUpCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.AfterStartUp = true;
        }

        private void startUpCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.AfterStartUp = false;
        }
    }
}