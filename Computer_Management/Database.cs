using System;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace Computer_Management
{
    public class Database
    {
        public ObservableCollection<Computer> Computers { get; private set; }
        private MainWindow mw;
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
            ListCountCheck();
        }

        // --- LIST COUNT CHECK --- | --- LIST COUNT CHECK --- | --- LIST COUNT CHECK --- | --- LIST COUNT CHECK --- | --- LIST COUNT CHECK --- | --- LIST COUNT CHECK --- |
        public void ListCountCheck() 
        {
            if (Computers.Count == 0)
            {
                mw.userLabel.Content = null;
                mw.osLabel.Content = null;
                mw.osLabel.IsEnabled = false;
                mw.cpuLabel.Content = null;
                mw.cpuLabel.IsEnabled = false;
                mw.gpuLabel.Content = null;
                mw.gpuLabel.IsEnabled = false;
                mw.ramLabel.Content = null;
                mw.ramLabel.IsEnabled = false;
                mw.mbLabel.Content = null;
                mw.mbLabel.IsEnabled = false;
                mw.pasteLabel.Content = null;
                mw.pasteLabel.IsEnabled = false;
                mw.nextCleaningDate.Content = null;

                mw.duplicatePC.Visibility = System.Windows.Visibility.Hidden;
                mw.noteTextBox.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Hidden;
                mw.dustClearCheckBox.IsEnabled = false;
                mw.pasteChangeCheckBox.IsEnabled = false;
                mw.noteTextBox.IsEnabled = false;
                mw.noteTextBox.Text = "";
                MsgBoxEditor.EditInfoMessage("No PCs found...", "No data");
            }

            else
            {
                mw.osLabel.IsEnabled = true;
                mw.cpuLabel.IsEnabled = true;
                mw.gpuLabel.IsEnabled = true;
                mw.ramLabel.IsEnabled = true;
                mw.mbLabel.IsEnabled = true;
                mw.pasteLabel.IsEnabled = false;

                mw.noteTextBox.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Hidden;
                mw.duplicatePC.Visibility = System.Windows.Visibility.Visible;
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

                    if(Settings.Default.SortingBy == 0)
                        mw.pcList.ItemsSource = Computers;
                    if(Settings.Default.SortingBy == 1)
                        mw.pcList.ItemsSource = Computers.OrderBy(c => c.UserName);
                    if (Settings.Default.SortingBy == 2)
                        mw.pcList.ItemsSource = Computers.OrderByDescending(c => c.UserName);

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
            SaveData();
            mw.pcList.SelectedIndex = 0;
            ListCountCheck();
        }

        // --- SAVING DATA --- | --- SAVING DATA --- | --- SAVING DATA --- | --- SAVING DATA --- | --- SAVING DATA --- | --- SAVING DATA --- | --- SAVING DATA --- |
        public void SaveData()
        {
            string dataPath = Settings.Default.DataPath;
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
                    MsgBoxEditor.EditInfoMessage("PC was successfully exported!", "Exporting successfull");
                    streamwriter.Flush();
                }
            }

            else 
            {
                MsgBoxEditor.EditInfoMessage("You have not selected PC...", "Not PC selected");
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

                    try { SaveData(); }
                    catch { MsgBoxEditor.EditErrorMessage("Importing PC failed...\nInternal error[Dx00110001]", "Importing PC failed"); }
                }
                catch { MsgBoxEditor.EditErrorMessage("Importing PC failed...\nInternal error[Dx00110010]", "Importing PC failed"); }
                
                MsgBoxEditor.EditInfoMessage("PC imported successfully!", "PC imported");
            }
        }
    }
}