using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Threading;

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
                MsgBoxEditor.EditInfoMessage("This application is already running...", "");
                App.Current.Shutdown();
            }
        }

        public MainWindow()
        {
            Application_Startup();
            Thread filesThread = new Thread(DefaultFileCreator.DataCheck);
            filesThread.Start();
            filesThread.Join();
            
            Thread settingsThread = new Thread(SettingsClass.Load);
            settingsThread.SetApartmentState(ApartmentState.STA);
            settingsThread.Start();
            settingsThread.Join();
            
            InitializeComponent();

            database = new Database(this);
            try
            {
                database.LoadData(Settings.Default.DataPath);
            }
            catch
            {
                MsgBoxEditor.EditErrorMessage("Something went wrong...\nError[0xD0010001]", "Data not loaded");
            }

            // --- BUTTONS --- |
            PasteChange = false;
            DustClean = false;
            CancelPicBTN.Visibility = Visibility.Hidden;
            SavePicBTN.Visibility = Visibility.Hidden;
            CancelNotePicBTN.Visibility = Visibility.Hidden;
            SaveNotePicBTN.Visibility = Visibility.Hidden;

            if (Settings.Default.IsDarkModeEnabled)
                Dark_mode.SetDarkMode(this, Settings.Default.Background, Settings.Default.Midground, Settings.Default.Foreground);
            else 
            {
                darkModeSwitch_enabled_border.Visibility = Visibility.Hidden;
                darkModeSwitch_enabled.Visibility = Visibility.Hidden;
                darkModeSwitch_disabled.Visibility = Visibility.Visible;
            }
        }

        // --- LABEL CONTENTS, NOTES & TOOLTIPS --- | --- LABEL CONTENTS, NOTES & TOOLTIPS --- | --- LABEL CONTENTS, NOTES & TOOLTIPS --- |
        private void PcList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SavePicBTN.Visibility = Visibility.Hidden;
            CancelPicBTN.Visibility = Visibility.Hidden;

            SaveNotePicBTN.Visibility = Visibility.Hidden;
            CancelNotePicBTN.Visibility = Visibility.Hidden;

            int linesCount = noteTextBox.Text.Split('\n').Length;
            if (linesCount >= 21)
            {
                noteTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            }
            else
            {
                noteTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            }

            if (pcList.SelectedItem != null)
            {
                noteTextBox.IsEnabled = true;
                Computer computer = (Computer)pcList.SelectedItem;

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

                diskLabel.Content = computer.Disk;
                if (diskLabel.Content.ToString().Length > 44)
                    diskLabel.ToolTip = computer.Disk;

                mbLabel.Content = computer.Motherboard;
                if (mbLabel.Content.ToString().Length > 44)
                    mbLabel.ToolTip = computer.Motherboard;

                pasteLabel.Content = computer.Paste;

                nextCleaningDate.Content = "Next maintenance: " + ((Computer)pcList.SelectedItem).Maintenance.ToString("dd.MM. yyyy");

                if (computer.DustClean && computer.Maintenance >= DateTime.Today)
                {
                    dustClearCheckBox.IsChecked = true;
                    dustClearCheckBox.IsEnabled = false;
                    CancelPicBTN.Visibility = Visibility.Hidden;
                    SavePicBTN.Visibility = Visibility.Hidden;
                }
                else
                {
                    dustClearCheckBox.IsChecked = false;
                    dustClearCheckBox.IsEnabled = true;
                    CancelPicBTN.Visibility = Visibility.Hidden;
                    SavePicBTN.Visibility = Visibility.Hidden;
                }
                // --- NOTE ---
                CachedNote = computer.Note;
                noteTextBox.Text = computer.Note.Trim();
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

        private void NoteTextBox_TextChanged(object sender, TextChangedEventArgs e)
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
        private void AddBTN_Click(object sender, RoutedEventArgs e)
        {
            new AddPC(this, database, "", "", "", "", "", "", "").Show();
        }

        private void RemoveBTN_Click(object sender, MouseButtonEventArgs e)
        {
            if (pcList.SelectedItem != null)
                new SureWindow(database, ((Image)sender).Name).ShowDialog();
            else { MsgBoxEditor.EditErrorMessage("There is not any PC to remove...!", "Error"); }
        }

        private void RefreshListBTN_Click(object sender, MouseButtonEventArgs e)
        {
            database.Computers.Clear();
            try { database.Computers.Clear(); database.LoadData(Settings.Default.DataPath); MessageBox.Show(MsgBoxEditor.EditText("Computer list reloaded..."), ""); }
            catch { MsgBoxEditor.EditErrorMessage("Refreshing data failed...\nError[0xD0110001]", "Refreshing failed"); }
        }

        private void DuplicatePC_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (pcList.SelectedItem != null)
                new AddPC(this, database, "", ((Computer)pcList.SelectedItem).OS, ((Computer)pcList.SelectedItem).Cpu, ((Computer)pcList.SelectedItem).Gpu, ((Computer)pcList.SelectedItem).Ram, ((Computer)pcList.SelectedItem).Disk, ((Computer)pcList.SelectedItem).Motherboard).Show();
            else { MsgBoxEditor.EditErrorMessage("You have not selected any PC to duplicate...!", "Error"); }
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

            if (PasteChange == true) 
            {
                // --- Maintenance
                int i = Settings.Default.PasteReplaceMonth;
                nextCleaningDate.Content = "Next maintenance: " + DateTime.Today.AddMonths(i + 1).ToString("dd.MM. yyyy");
                ((Computer)pcList.SelectedItem).Change("date", DateTime.Today.AddMonths(i + 1).ToString("dd.MM. yyyy"));
                // --- Changing paste
                pasteLabel.Content = pasteType.SelectedItem.ToString();
                ((Computer)pcList.SelectedItem).Change("pasteLabel", pasteType.SelectedItem.ToString().Trim());
                pasteChangeCheckBox.IsChecked = false;
            }
            if (DustClean == true) 
            {
                ((Computer)pcList.SelectedItem).Change("dust", DustClean.ToString());
                dustClearCheckBox.IsChecked = true;
                dustClearCheckBox.IsEnabled = false;
            }

            try { database.SaveData(); }
            catch { MsgBoxEditor.EditErrorMessage("Something went wrong...\nError[0xD0110010]", "Changing note failed"); }
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

            try { ((Computer)pcList.SelectedItem).Change("changeNote", noteTextBox.Text.Trim()); }
            catch { MsgBoxEditor.EditErrorMessage("Changing note failed...\nError[0xD0110011]", "Error"); }
            
            try { database.SaveData(); }
            catch { MsgBoxEditor.EditErrorMessage("Changing note failed...\nError[0xD0110100]", "Error"); }
        }
            // --- DarkMode switch
        private void darkModeSwitch_enabled_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Dark_mode.Deactivate(this);
            Settings.Default.IsDarkModeEnabled = false;
            SettingsClass.Save();
        }

        private void darkModeSwitch_disabled_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Dark_mode.SetDarkMode(this, Settings.Default.Background, Settings.Default.Midground, Settings.Default.Foreground);
            Settings.Default.IsDarkModeEnabled = true;
            SettingsClass.Save();
        }

        // --- CHECKBOXES --- | --- CHECKBOXES --- | --- CHECKBOXES --- | --- CHECKBOXES --- | --- CHECKBOXES --- | --- CHECKBOXES --- | --- CHECKBOXES --- |
        private void PasteChangeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CancelPicBTN.Visibility = Visibility.Visible;
            SavePicBTN.Visibility = Visibility.Visible;
            dustClearCheckBox.IsChecked = true;
            pasteType.IsEnabled = true;
            PasteChange = true;
        }
        private void PasteChangeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            pasteType.IsEnabled = false;
            PasteChange = false;
            if (!DustClean) 
            {
                CancelPicBTN.Visibility = Visibility.Hidden;
                SavePicBTN.Visibility = Visibility.Hidden;
            }
        }

        private void DustClearCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CancelPicBTN.Visibility = Visibility.Visible;
            SavePicBTN.Visibility = Visibility.Visible;
            DustClean = true;
        }

        private void DustClearCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DustClean = false;
            if (!PasteChange) 
            {
                CancelPicBTN.Visibility = Visibility.Hidden;
                SavePicBTN.Visibility = Visibility.Hidden;
            }
        }

        // --- MENU --- | --- MENU --- | --- MENU --- | --- MENU --- | --- MENU --- | --- MENU --- | --- MENU --- | --- MENU --- | --- MENU --- | --- MENU --- |
        private void LoadBackup_Click(object sender, RoutedEventArgs e)
        {
            string sndr = ((MenuItem)sender).Name;

            if (sndr == "menuItem_LoadBackup")
            {
                if (File.Exists(database.BackUpPath))
                {
                    new SureWindow(database, sndr).ShowDialog();
                }
                else
                    MsgBoxEditor.EditErrorMessage("Backup file doesn't exist", "File doesn't exist");
            }

            if (sndr == "menuItem_LoadBackupFEF")
            {
                database.ImportPC(sndr); //Already have Error No.
            }
        }

        private void SaveBackup_Click(object sender, RoutedEventArgs e)
        {
            string sndr = ((MenuItem)sender).Name;

            if (database.Computers.Count == 0) { MsgBoxEditor.EditErrorMessage("No computers to save...", ""); }
            else 
            {
                if (sndr == "menuItem_SaveBackup")
                {
                    new SureWindow(database, sndr).ShowDialog();
                }

                if (sndr == "menuItem_SaveBackupTEF")
                {
                    try
                    {
                        using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                        {
                            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                            if (result == System.Windows.Forms.DialogResult.OK)
                            {
                                database.SaveData(Path.Combine(dialog.SelectedPath, "Data_Backup.xml"));
                                MessageBox.Show(MsgBoxEditor.EditText("Backup saved successfully!"), "");
                            }
                        }
                    }
                    catch { MsgBoxEditor.EditErrorMessage("Saving backup failed...\nError[0xD0111001]", "Error"); }
                }
            }
        }

        private void OpenDataFolder(object sender, RoutedEventArgs e)
        {
            Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management"));
        }

        private void DeleteALL_Click(object sender, RoutedEventArgs e)
        {
            new SureWindow(database, ((MenuItem)sender).Name).ShowDialog();
            CancelNotePicBTN.Visibility = Visibility.Hidden;
            SaveNotePicBTN.Visibility = Visibility.Hidden;
            CachedNote = noteTextBox.Text;
        }

        private void ExportPC_Click(object sender, RoutedEventArgs e)
        {
            try { database.ExportPC(); }
            catch { MsgBoxEditor.EditErrorMessage("Exporting PC failed...\nInternal error[0xD0111010]", "Exporting PC failed"); }
        }

        private void ImportPC_Click(object sender, RoutedEventArgs e)
        {
            database.ImportPC("importPC"); //Already have error_No;
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow(this).ShowDialog();
        }

        private void Shortcuts_Click(object sender, RoutedEventArgs e)
        {
            new ShortcutsWindow().ShowDialog();
        }

        private void Version_Click(object sender, RoutedEventArgs e)
        {
            new About().ShowDialog();
        }

        // --- ENVIROMENT --- | --- ENVIROMENT --- | --- ENVIROMENT --- | --- ENVIROMENT --- | --- ENVIROMENT --- | --- ENVIROMENT --- | --- ENVIROMENT --- |
            // --- SHORTCUTS --- ||
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftShift) && Keyboard.IsKeyDown(Key.I)) //Import backup from external file
            {
                database.ImportPC("LoadBackupFEF"); //Already have Error No.
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftShift) && Keyboard.IsKeyDown(Key.E)) //Export backup from external file
            {
                try
                {
                    using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                    {
                        System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                        if (result == System.Windows.Forms.DialogResult.OK)
                        {
                            database.SaveData(Path.Combine(dialog.SelectedPath, "Data_Backup.xml"));
                            MessageBox.Show(MsgBoxEditor.EditText("Backup saved successfully!"), "");
                        }
                    }
                }
                catch { MsgBoxEditor.EditErrorMessage("Saving backup failed...\nError[0xD0111001]", "Error"); }
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.I)) //Import PC
            {
                database.ImportPC("importPC"); //Already have error_No;
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.C)) //Duplicate current PC
            {
                if (pcList.SelectedItem != null)
                    new AddPC(this, database, "", ((Computer)pcList.SelectedItem).OS, ((Computer)pcList.SelectedItem).Cpu, ((Computer)pcList.SelectedItem).Gpu, ((Computer)pcList.SelectedItem).Ram, ((Computer)pcList.SelectedItem).Disk, ((Computer)pcList.SelectedItem).Motherboard).Show();
                else { MsgBoxEditor.EditErrorMessage("You have not selected any PC to duplicate...!", "Error"); }
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.E)) //Export PC
            {
                try { database.ExportPC(); }
                catch { MsgBoxEditor.EditErrorMessage("Exporting PC failed...\nInternal error[0xD0111010]", "Exporting PC failed"); }
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.F)) //Open data folder
            {
                try { Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management")); }
                catch { MsgBoxEditor.EditErrorMessage("Data folder couldn't be open...", "Folder Error"); }
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.D)) //Delete ALL data
            {
                new SureWindow(database, "deleteALL").ShowDialog();
                CancelNotePicBTN.Visibility = Visibility.Hidden;
                SaveNotePicBTN.Visibility = Visibility.Hidden;
                CachedNote = noteTextBox.Text;
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.A)) //Add PC
            {
                new AddPC(this, database, "", "", "", "", "", "", "").Show();
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.R)) //Delete current selected pc
            {
                new SureWindow(database, "removePC").ShowDialog();
            }
        }
    }
}