using System;
using System.IO;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Windows;

namespace Computer_Management
{
    public class Database
    {
        public ObservableCollection<Computer> Computers { get; private set; }
        private MainWindow mw;
        public string DataPath { get; private set; }
        public string BackUpPath { get; private set; }
        public string[] Pastas { get; private set; }

        public Database(MainWindow mw)
        {
            this.mw = mw;
            Computers = new ObservableCollection<Computer>();
            string[] pastas = { "Cheap", "Expensive" };
            Pastas = pastas;
            mw.pasteType.ItemsSource = Pastas;
            BackUpPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Data_Backup.csv");
            DataPath = Settings.Default.DataPath;
        }

        // --- LIST COUNT CHECK --- | --- LIST COUNT CHECK --- | --- LIST COUNT CHECK --- | --- LIST COUNT CHECK --- | --- LIST COUNT CHECK --- | --- LIST COUNT CHECK --- |
        public void ListCountCheck() 
        {
            if (Computers.Count == 0)
            {
                mw.userLabel.Content = null;
                mw.osLabel.Content = null;
                mw.cpuLabel.Content = null;
                mw.gpuLabel.Content = null;
                mw.ramLabel.Content = null;
                mw.mbLabel.Content = null;
                mw.pasteLabel.Content = null;
                mw.nextCleaningDate.Content = null;

                mw.dustClearCheckBox.IsEnabled = false;
                mw.pasteChangeCheckBox.IsEnabled = false;
                mw.noteTextBox.IsEnabled = false;
                mw.noteTextBox.Text = "";
                MsgBoxEditor.EditMessage("No PCs found...", "Missing data");
            }

            else
            {
                mw.dustClearCheckBox.IsEnabled = true;
                mw.pasteChangeCheckBox.IsEnabled = true;
                mw.noteTextBox.IsEnabled = true;
            }
        }

        // --- LOADING DATA --- | --- LOADING DATA --- | --- LOADING DATA --- | --- LOADING DATA --- | --- LOADING DATA --- | --- LOADING DATA --- | --- LOADING DATA --- |
        public void LoadData(string dataPath)
        {
            using (StreamReader streamreader = new StreamReader(dataPath))
            {
                if (streamreader.ReadLine() != null)
                {
                    string s;
                    while ((s = streamreader.ReadLine()) != null)
                    {
                        string[] splitted = s.Split(';');
                        string user = splitted[0].ToString();
                        string os = splitted[1].ToString();
                        string cpu = splitted[2].ToString();
                        string gpu = splitted[3].ToString();
                        string ram = splitted[4].ToString();
                        string mb = splitted[5].ToString();
                        string paste = splitted[6].ToString();
                        string note = splitted[7].ToString();
                        DateTime nextCleaning = DateTime.Parse(splitted[8]);
                        Computers.Add(new Computer(user, os, cpu, gpu, ram, mb, paste, note, nextCleaning));
                    }
                    mw.pcList.ItemsSource = Computers;
                    mw.pcList.SelectedIndex = 0;
                }
                else { }
                ListCountCheck();
            }
        }

        // --- REMOVE DATA --- | --- REMOVE DATA --- | --- REMOVE DATA --- | --- REMOVE DATA --- | --- REMOVE DATA --- | --- REMOVE DATA --- | --- REMOVE DATA --- |
        public void RemovePc()
        {
            Computers.Remove((Computer)mw.pcList.SelectedItem);
            SaveData(DataPath);
            mw.pcList.SelectedIndex = 0;
            ListCountCheck();
        }

        // --- SAVING DATA --- | --- SAVING DATA --- | --- SAVING DATA --- | --- SAVING DATA --- | --- SAVING DATA --- | --- SAVING DATA --- | --- SAVING DATA --- |
        public void SaveData(string dataPath)
        {
            using (StreamWriter streamwriter = new StreamWriter(dataPath))
            {
                streamwriter.WriteLine(); //cause 1st line isn't added to list
                foreach (Computer c in Computers)
                {
                    string note = c.Note;
                    if (note == "")
                        note = " "; //cause if is null delete all data
                    
                    string[] values = { c.UserName, c.OS, c.Cpu, c.Gpu, c.Ram, c.Motherboard, c.Paste, note, c.NextCleaning.ToShortDateString() };
                    string row = String.Join(";", values);
                    streamwriter.WriteLine(row);
                }
                streamwriter.Flush();
            }
        }

        public void SaveOne(string dataPath) 
        {
            if (mw.pcList.SelectedItem != null)
            {
                using (StreamWriter streamwriter = new StreamWriter(dataPath))
                {
                    Computer c = (Computer)mw.pcList.SelectedItem;
                    string note = c.Note;
                    if (note == "")
                        note = " ";

                    streamwriter.WriteLine();
                    string[] values = { c.UserName, c.OS, c.Cpu, c.Gpu, c.Ram, c.Motherboard, c.Paste, note, c.NextCleaning.ToShortDateString() };
                    string row = String.Join(";", values);
                    streamwriter.WriteLine(row);
                    MsgBoxEditor.EditMessage("PC was successfully exported!", "Exporting successfull");
                    streamwriter.Flush();
                }
            }

            else 
            {
                MsgBoxEditor.EditMessage("You have not selected PC...", "Not PC selected");
            }
        }

        // --- EXPORTING DATA --- | --- EXPORTING DATA --- | --- EXPORTING DATA --- | --- EXPORTING DATA --- | --- EXPORTING DATA --- | --- EXPORTING DATA --- |
        public void ExportPC() 
        {
                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        SaveOne(Path.Combine(dialog.SelectedPath, "Exported_PC.csv"));
                    }
                }
        }

        // --- IMPORTING DATA --- | --- IMPORTING DATA --- | --- IMPORTING DATA --- | --- IMPORTING DATA --- | --- IMPORTING DATA --- | --- IMPORTING DATA --- |
        public void ImportPC(string sender) 
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = "";
            dlg.DefaultExt = ".cvs";
            dlg.Filter = ".csv|*csv*|.txt|*txt*";

            if (sender == "importPC")
                dlg.FileName = "Exported_PC";

            if (sender == "LoadBackupFEF")
                dlg.FileName = "Data_Backup";

            Nullable<bool> result = dlg.ShowDialog();

            string filePath = "";

            if (result == true) 
            {
                filePath = dlg.FileName;
                try 
                { 
                    LoadData(filePath);

                    try { SaveData(DataPath); }
                    catch { MessageBox.Show(MsgBoxEditor.EditText("Importing PC failed...") + "\nInternal error[Dx00110001]", "Importing PC failed", MessageBoxButton.OK, MessageBoxImage.Error); }
                }
                catch { MessageBox.Show(MsgBoxEditor.EditText("Importing PC failed...") + "\nInternal error[Dx00110010]", "Importing PC failed", MessageBoxButton.OK, MessageBoxImage.Error); }

                MsgBoxEditor.EditMessage("PC imported successfully!", "PC imported");
            }
        }
    }
}