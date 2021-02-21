using System;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Diagnostics;

/*
using System.Windows.Shapes;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
*/

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
            monthsComboBox.SelectedIndex = 0;
            dataPathLabel.Content = Settings.DataPath();
            if (Settings.DataPath().Length >= 50) { dataPathLabel.ToolTip = Settings.DataPath(); }
            else { }
        }

        private void dataPathClick(object sender, MouseButtonEventArgs e)
        {
            try { Process.Start(dataPathLabel.Content.ToString().Remove(dataPathLabel.Content.ToString().Length - 8, 8)); }
            catch { MessageBox.Show(MsgBoxEditor.EditText("Datapath not found..."), "Datapath not found", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void dataPathPic_MouseDown(object sender, MouseButtonEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    string newPath = Path.Combine(dialog.SelectedPath, "Data.csv");
                    dataPathLabel.Content = newPath;
                    dataPathLabel.ToolTip = newPath;
                }
            }
        }

        private void RDBTN_Click(object sender, RoutedEventArgs e)
        {
            Settings.RestoreDefault(this);
        }

        private void OkBTN_Click(object sender, RoutedEventArgs e)
        {
            Settings.SaveSettings(this);
            this.Close();
        }
    }
}