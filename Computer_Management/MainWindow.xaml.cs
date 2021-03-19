using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Reflection;

namespace Computer_Management
{
    public partial class MainWindow : Window
    {
        public bool PasteChange { get; private set; }
        public bool DustClean { get; private set; }
        private object zamek = new object();
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

            Thread settingsThread = new Thread(SettingsClass.CorrectSettings);
            settingsThread.Start();
            settingsThread.Join();

            SettingsClass.Save();
            InitializeComponent();

            database = new Database(this);
            try
            {
                database.LoadData(database.DataPath);
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

        private void NoteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (noteTextBox.Text.Contains("$"))
            {
                MsgBoxEditor.EditErrorMessage("Notes can not contains '$' & ';' symobols!", "");
                noteTextBox.Text = noteTextBox.Text.Replace('$', '\0');
            }
            else { }
            if (noteTextBox.Text.Contains(";"))
            {
                MsgBoxEditor.EditErrorMessage("Notes can not contains '$' and ';' symobols!", "");
                noteTextBox.Text = noteTextBox.Text.Replace(';', '\0');
            }
            else { }

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
            new AddPC(this, database, "", "", "", "", "", "").Show();
        }

        private void RemoveBTN_Click(object sender, MouseButtonEventArgs e)
        {
            new SureWindow(database, ((Image)sender).Name).ShowDialog();
        }

        private void RefreshListBTN_Click(object sender, MouseButtonEventArgs e)
        {
            database.Computers.Clear();
            try { database.Computers.Clear(); database.LoadData(Settings.Default.DataPath); MessageBox.Show(MsgBoxEditor.EditText("Computer list reloaded..."), ""); }
            catch { MsgBoxEditor.EditErrorMessage("Refreshing data failed...\nError[0xD0110001]", "Refreshing failed"); }
        }

        private void DuplicatePC_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new AddPC(this, database, "", ((Computer)pcList.SelectedItem).OS, ((Computer)pcList.SelectedItem).Cpu, ((Computer)pcList.SelectedItem).Gpu, ((Computer)pcList.SelectedItem).Ram, ((Computer)pcList.SelectedItem).Motherboard).Show();
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
                int i = Settings.Default.Month;
                nextCleaningDate.Content = "Next maintenance: " + DateTime.Today.AddMonths(i + 1).ToString("dd.MM. yyyy");
                ((Computer)pcList.SelectedItem).Change("date", DateTime.Today.AddMonths(i + 1).ToString("dd.MM. yyyy"));

                if (PasteChange == true) 
                {
                    pasteLabel.Content = pasteType.SelectedItem.ToString();
                    ((Computer)pcList.SelectedItem).Change("pasteLabel", pasteType.SelectedItem.ToString().Trim());
                }
            }

            try { database.SaveData(database.DataPath); }
            catch { MsgBoxEditor.EditErrorMessage("Something went wrong...\nError[0xD0110010]", "Changing note failed"); }
            
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
            catch { MsgBoxEditor.EditErrorMessage("Changing note failed...\nError[0xD0110011]", "Error"); }
            
            try { database.SaveData(database.DataPath); }
            catch { MsgBoxEditor.EditErrorMessage("Changing note failed...\nError[0xD0110100]", "Error"); }
        }

        // --- CHECKBOXES --- | --- CHECKBOXES --- | --- CHECKBOXES --- | --- CHECKBOXES --- | --- CHECKBOXES --- | --- CHECKBOXES --- | --- CHECKBOXES --- |
        private void PasteChangeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CancelPicBTN.Visibility = Visibility.Visible;
            SavePicBTN.Visibility = Visibility.Visible;
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
                    MsgBoxEditor.EditErrorMessage("Backup file doesn't exist", "File doesn't exist");
            }

            if (sndr == "LoadBackupFEF")
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
            new SettingsWindow().ShowDialog();
        }

        private void Shortcuts_Click(object sender, RoutedEventArgs e)
        {
            string text = MsgBoxEditor.EditText("Shortcuts:\n") + "\nImport PC - CTRL+I" + "\nExport PC - CTRL+E" + "\nOpen data folder - CTRL+F" + "\nDelete ALL data - CTRL+D"
                + "\nAdd PC - CTRL+A" + "\nRemove PC - CTRL+R" + "\nDuplicate PC - CTRL+C" + "\nImport backup - CTRL+SHIFT+I" + "\nExport backup - CTRL+SHIFT+E";
            MessageBox.Show(text);
        }

        private void Version_Click(object sender, RoutedEventArgs e)
        {
            string version = Assembly.GetEntryAssembly().GetName().Version.ToString();
            MessageBox.Show(MsgBoxEditor.EditText("Computer management") + "\nVersion: " + version + "\n\n                                     Created by JD_1609\n", "About");
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
                            database.SaveData(Path.Combine(dialog.SelectedPath, "Data_Backup.csv"));
                            MessageBox.Show(MsgBoxEditor.EditText("Backup saved successfully!"), "");
                        }
                    }
                }
                catch { MsgBoxEditor.EditErrorMessage("Saving backup failed...\nError[0xD0111001]", "Error"); }
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.C)) //Import PC
            {
                new AddPC(this, database, "", ((Computer)pcList.SelectedItem).OS, ((Computer)pcList.SelectedItem).Cpu, ((Computer)pcList.SelectedItem).Gpu, ((Computer)pcList.SelectedItem).Ram, ((Computer)pcList.SelectedItem).Motherboard).Show();
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.I)) //Duplicate current PC
            {
                new AddPC(this, database, "", ((Computer)pcList.SelectedItem).OS, ((Computer)pcList.SelectedItem).Cpu, ((Computer)pcList.SelectedItem).Gpu, ((Computer)pcList.SelectedItem).Ram, ((Computer)pcList.SelectedItem).Motherboard).Show();
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.E)) //Export PC
            {
                try { database.ExportPC(); }
                catch { MsgBoxEditor.EditErrorMessage("Exporting PC failed...\nInternal error[0xD0111010]", "Exporting PC failed"); }
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.F)) //Open data folder
            {
                Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management"));
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
                new AddPC(this, database, "", "", "", "", "", "").Show();
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.R)) //Delete current selected pc
            {
                new SureWindow(database, "removePC").ShowDialog();
            }
        }
    }
}