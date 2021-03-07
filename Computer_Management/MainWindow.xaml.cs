using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Threading;

/*using System.Text;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;/
using System.Security.Principal;*/

namespace Computer_Management
{
    public partial class MainWindow : Window
    {
        public bool PasteChange { get; private set; }
        public bool DustClean { get; private set; }
        private Database database;
        Mutex oneInstance;
        private void Application_Startup()
        {
            bool aIsNewInstance = false;
            oneInstance = new Mutex(true, "Computer_Management", out aIsNewInstance);
            if (!aIsNewInstance)
            {
                MsgBoxEditor.EditMessage("This application is already running...", "");
                App.Current.Shutdown();
            }
        }

        public MainWindow()
        {
            Application_Startup();
            InitializeComponent();
            database = new Database(this);

            // --- DATA FOLDER --- | --- DATA FOLDER --- | --- DATA FOLDER --- | --- DATA FOLDER --- | --- DATA FOLDER --- | --- DATA FOLDER --- | --- DATA FOLDER --- |
            try
            {
                string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
            }
            catch
            {
                MessageBox.Show("Application couldn't be started...\nPlease check your authorization!" + MsgBoxEditor.EditText("\nError[0x00001001]"), "Folder creating error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }

            // --- DATA FILE --- | --- DATA FILE --- | --- DATA FILE --- | --- DATA FILE --- | --- DATA FILE --- | --- DATA FILE --- | --- DATA FILE --- | --- DATA FILE --- |
            try
            {
                string dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Data.csv");
                if (!File.Exists(dataPath))
                    File.Create(dataPath);
                else 
                {
                    try
                    {
                        database.LoadData(database.DataPath);
                    }
                    catch { MessageBox.Show(MsgBoxEditor.EditText("Something went wrong...") + "\nError[0xD0010001]", "Data not loaded", MessageBoxButton.OK, MessageBoxImage.Error); }
                }
                /*
                if (!File.Exists(database.SettingsPath)) 
                {
                    File.Create(database.SettingsPath);
                    fileExist = false;
                }
                if(fileExist == false)
                    Settings.RestoreDefault(null); //---upper -> SetSettings();
                
                for .THC*/
            }
            catch
            {
                MessageBox.Show("Application couldn't be started...\nPlease check your authorization!" + MsgBoxEditor.EditText("\nError[0x00001010]"), "Data file creating error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            // --- BUTTONS --- |
            PasteChange = false;
            DustClean = false;
            CancelPicBTN.Visibility = Visibility.Hidden;
            SavePicBTN.Visibility = Visibility.Hidden;
            CancelNotePicBTN.Visibility = Visibility.Hidden;
            SaveNotePicBTN.Visibility = Visibility.Hidden;
         }

        // --- LABEL CONTENTS, NOTES & TOOLTIPS --- | --- LABEL CONTENTS, NOTES & TOOLTIPS --- | --- LABEL CONTENTS, NOTES & TOOLTIPS --- |
        private void pcList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SavePicBTN.Visibility = Visibility.Hidden;
            CancelPicBTN.Visibility = Visibility.Hidden;

            if (pcList.SelectedItem != null)
            {
                Computer computer = (Computer)pcList.SelectedItem;

                if (computer.UserName.Contains('_'))
                    userLabel.Content = "_" + computer.UserName;
                else
                    userLabel.Content = computer.UserName;

                osLabel.Content = computer.OS;
                if (osLabel.Content.ToString().Length > 44)
                    osLabel.ToolTip = computer.OS;

                cpuLabel.Content = computer.Cpu;
                if (cpuLabel.Content.ToString().Length > 44)
                    cpuLabel.ToolTip = computer.Cpu;

                gpuLabel.Content = computer.Gpu;
                if (gpuLabel.Content.ToString().Length > 44)
                    gpuLabel.ToolTip = computer.Gpu;

                ramLabel.Content = computer.Ram;
                if (ramLabel.Content.ToString().Length > 44)
                    ramLabel.ToolTip = computer.Ram;

                mbLabel.Content = computer.Motherboard;
                if (mbLabel.Content.ToString().Length > 44)
                    mbLabel.ToolTip = computer.Motherboard;

                pasteLabel.Content = computer.Paste;

                nextCleaningDate.Content = "Next maintenance: " + ((Computer)pcList.SelectedItem).NextCleaning.ToString("dd.MM. yyyy");
                // --- NOTE ---
                CachedNote = computer.Note.Replace('$', '\n').Remove(0, 1);
                noteTextBox.Text = computer.Note.Replace('$', '\n').Remove(0, 1);//cause 1st char is '$'
            }
            else { }
        }

        // --- CHANGING ACTUAL DATA --- | --- CHANGING ACTUAL DATA --- | --- CHANGING ACTUAL DATA --- | --- CHANGING ACTUAL DATA --- | --- CHANGING ACTUAL DATA --- |
        private string CachedNote { get; set; }

        private void ParametrClick(object sender, MouseButtonEventArgs e)
        {
            object sndr = sender;
            string data = ((Label)sender).Content.ToString();
            ChangeData chdata = new ChangeData(this, database, data, sndr);
            chdata.ShowDialog();
        }

        private void noteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CachedNote == noteTextBox.Text) 
            {
            }
            else 
            { 
                CancelNotePicBTN.Visibility = Visibility.Visible;
                SaveNotePicBTN.Visibility = Visibility.Visible; 
            }

            int linesCount = noteTextBox.Text.Split('\n').Length;
            if (linesCount >= 21) 
            {
                noteTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible; 
            }
            else 
            {
                noteTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden; 
            }
        }

        // --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- |
        private void addBTN_Click(object sender, RoutedEventArgs e)
        {
            new AddPC(this, database, "", "", "", "", "", "").Show();
        }

        private void removeBTN_Click(object sender, MouseButtonEventArgs e)
        {
            new SureWindow(database, ((Image)sender).Name).ShowDialog();
        }

        private void refreshListBTN_Click(object sender, MouseButtonEventArgs e)
        {
            database.Computers.Clear();
            try { database.LoadData(database.DataPath); }
            catch { MessageBox.Show(MsgBoxEditor.EditText("Refreshing data failed...") + "\nError[0xD0110001]", "Refreshing failed", MessageBoxButton.OK, MessageBoxImage.Error); }
            MsgBoxEditor.EditMessage("Computer list reloaded...", "");
        }

        private void CancelPicClick(object sender, MouseButtonEventArgs e)
        {
            CancelPicBTN.Visibility = Visibility.Hidden;
            SavePicBTN.Visibility = Visibility.Hidden;
            pasteChangeCheckBox.IsChecked = false;
            dustClearCheckBox.IsChecked = false;
        }

        private void SavePicClick(object sender, MouseButtonEventArgs e)
        {
            CancelPicBTN.Visibility = Visibility.Hidden;
            SavePicBTN.Visibility = Visibility.Hidden;

            if (PasteChange == true || DustClean == true) 
            { 
                nextCleaningDate.Content = "Next maintenance: " + DateTime.Today.AddMonths(3).ToString("dd.MM. yyyy");
                ((Computer)pcList.SelectedItem).Change("date", DateTime.Today.AddMonths(3).ToString("dd.MM. yyyy"));

                if (PasteChange == true) 
                {
                    pasteLabel.Content = pasteType.SelectedItem.ToString();
                    ((Computer)pcList.SelectedItem).Change("pasteLabel", pasteType.SelectedItem.ToString().Trim());
                }
            }

            try { database.SaveData(database.DataPath); } //change title + make erroor NO.
            catch { MessageBox.Show(MsgBoxEditor.EditText("Something went wrong...") + "\nError[0xD0110010]", "Changing note failed", MessageBoxButton.OK, MessageBoxImage.Error);  }

            pasteChangeCheckBox.IsChecked = false;
            dustClearCheckBox.IsChecked = false;
        }

        private void CancelSaveNoteBTN(object sender, MouseButtonEventArgs e)
        {
            CancelNotePicBTN.Visibility = Visibility.Hidden;
            SaveNotePicBTN.Visibility = Visibility.Hidden;
            noteTextBox.Text = CachedNote;
        }

        private void SaveNoteBTN(object sender, MouseButtonEventArgs e)
        {
            CancelNotePicBTN.Visibility = Visibility.Hidden;
            SaveNotePicBTN.Visibility = Visibility.Hidden;
            CachedNote = noteTextBox.Text;

            try { ((Computer)pcList.SelectedItem).Change("changeNote", NoteCorrector.CorrectNote(noteTextBox.Text)); }
            catch { MessageBox.Show(MsgBoxEditor.EditText("Changing note failed...") + "\nError[0xD0110011]", "Changing note failed", MessageBoxButton.OK, MessageBoxImage.Error); }
            
            try { database.SaveData(database.DataPath); }
            catch { MessageBox.Show(MsgBoxEditor.EditText("Changing note failed...") + "\nError[0xD0110100]", "Changing note failed", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        // --- CHECKBOXES --- | --- CHECKBOXES --- | --- CHECKBOXES --- | --- CHECKBOXES --- | --- CHECKBOXES --- | --- CHECKBOXES --- | --- CHECKBOXES --- |
        private void pasteChangeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CancelPicBTN.Visibility = Visibility.Visible;
            SavePicBTN.Visibility = Visibility.Visible;
            pasteType.IsEnabled = true;
            PasteChange = true;
        }
        private void pasteChangeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            pasteType.IsEnabled = false;
            PasteChange = false;
        }

        private void dustClearCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CancelPicBTN.Visibility = Visibility.Visible;
            SavePicBTN.Visibility = Visibility.Visible;
            DustClean = true;
        }

        private void dustClearCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DustClean = false;
        }

        // --- MENU --- | --- MENU --- | --- MENU --- | --- MENU --- | --- MENU --- | --- MENU --- | --- MENU --- | --- MENU --- | --- MENU --- | --- MENU --- |
        private void BackupLoad_Click(object sender, RoutedEventArgs e)
        {
            string sndr = ((MenuItem)sender).Name;

            if (sndr == "LoadBackup")
            {
                if (File.Exists(database.BackUpPath))
                {
                    new SureWindow(database, sndr).ShowDialog();
                }
                else
                    MsgBoxEditor.EditMessage("Backup file doesn't exist", "File doesn't exist");
            }

            if (sndr == "LoadBackupFEF")
            {
                database.ImportPC(sndr); //Already have Error No.
            }
        }

        private void SaveBackup_Click(object sender, RoutedEventArgs e)
        {
            string sndr = ((MenuItem)sender).Name;

            if (database.Computers.Count == 0) { MsgBoxEditor.EditMessage("No computers to save...", ""); }
            else 
            {
                if (sndr == "SaveBackup")
                {
                    new SureWindow(database, sndr).ShowDialog();
                }

                if (sndr == "SaveBackupTEF")
                {
                    try
                    {
                        using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                        {
                            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                            if (result == System.Windows.Forms.DialogResult.OK)
                            {
                                database.SaveData(Path.Combine(dialog.SelectedPath, "Data_Backup.csv"));
                                MsgBoxEditor.EditMessage("Backup saved successfully!", "Backup saved");
                            }
                        }
                    }
                    catch { MessageBox.Show(MsgBoxEditor.EditText("Saving backup failed...") + "\nError[0xD0111001]", "Saving backup failed", MessageBoxButton.OK, MessageBoxImage.Error); }
                }
            }
        }

        private void OpenDataFolder(object sender, RoutedEventArgs e)
        {
            Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management"));
        }

        private void deleteALL_Click(object sender, RoutedEventArgs e)
        {
            new SureWindow(database, ((MenuItem)sender).Name).ShowDialog();
            CancelNotePicBTN.Visibility = Visibility.Hidden;
            SaveNotePicBTN.Visibility = Visibility.Hidden;
            CachedNote = noteTextBox.Text;
        }

        private void exportPC_Click(object sender, RoutedEventArgs e)
        {
            try { database.ExportPC(); }
            catch { MsgBoxEditor.EditMessage("Exporting PC failed...\nInternal error[0xD0111010]", "Exporting PC failed"); }
        }

        private void importPC_Click(object sender, RoutedEventArgs e)
        {
            database.ImportPC("importPC"); //Already have error_No;
        }

        private void Version_Click(object sender, RoutedEventArgs e)
        {
            MsgBoxEditor.EditMessage(MsgBoxEditor.EditText("Computer management") + "\nVersion: 0.1.5 -BETA\n\n                                     Created by JD_1609\n", "About");
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow().ShowDialog();
        }

        // --- ENVIROMENT --- | --- ENVIROMENT --- | --- ENVIROMENT --- | --- ENVIROMENT --- | --- ENVIROMENT --- | --- ENVIROMENT --- | --- ENVIROMENT --- |
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.W))
            {
                MessageBox.Show("asdasdasd");
            }
        }
    }
}