using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace Computer_Management
{
    public class Database
    {
        public ObservableCollection<Computer> Computers { get; private set; }
        private XDocument dataDocument;
        private MainWindow mw;
        public string BackUpPath { get; private set; }
        public string[] Pastas { get; private set; }

        public Database(MainWindow mw)
        {
            this.mw = mw;
            Computers = new ObservableCollection<Computer>();
            string[] pastas = { "Cheap", "Expensive" };
            Pastas = pastas;
            dataDocument = XDocument.Load(Settings.Default.DataPath);
            mw.pasteType.ItemsSource = Pastas;
            BackUpPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Data_Backup.xml");
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
            DateTime added = DateTime.Now;
            string user = "Username";
            string os = "Operating system";
            string cpu = "CPU";
            string gpu = "GPU";
            string ram = "RAM";
            string mb = "Motherboard";
            string paste = "Paste";
            string note = "";
            DateTime maintenance = DateTime.Now.AddMonths(3);

            if (dataDocument.Elements("Computer").Count() > 0)
            {
                foreach (XElement c in dataDocument.Elements("Computer"))
                {
                    added = DateTime.Parse(c.Attribute("Added").Value);
                    user = c.Attribute("userName").Value;
                    os = c.Element("OS").Value;
                    cpu = c.Element("CPU").Value;
                    gpu = c.Element("GPU").Value;
                    ram = c.Element("RAM").Value;
                    mb = c.Element("Motherboard").Value;
                    paste = c.Element("Paste").Value;
                    foreach (XElement row in c.Element("Note").Elements("Row"))
                        note += row.Value.Trim() + "\n";
                    maintenance = DateTime.Parse(c.Element("Maintenance").Value);

                    Computers.Add(new Computer(added, user, os, cpu, gpu, ram, mb, paste, note, maintenance));
                }

                if (Settings.Default.SortingBy == 0)
                    mw.pcList.ItemsSource = Computers.OrderBy(c => c.Added);
                if (Settings.Default.SortingBy == 1)
                    mw.pcList.ItemsSource = Computers.OrderBy(c => c.UserName);
                if (Settings.Default.SortingBy == 2)
                    mw.pcList.ItemsSource = Computers.OrderByDescending(c => c.UserName);

                mw.pcList.SelectedIndex = 0;
                ListCountCheck();
            }
            else 
            {
                MsgBoxEditor.EditInfoMessage("No PCs found", "No data");
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
            XDocument data = new XDocument(new XDeclaration("1.O", "UTF-8", null));
            try 
            {
                data.Add(new XElement("Computers"));

                if (Computers.Count != 0)
                {
                    foreach (Computer c in Computers)
                    {
                        XElement noteRows = new XElement("Note");
                        foreach (var s in c.Note.Split('\n'))
                        {
                            noteRows.Add(new XElement("Row", s.Trim()));
                        }

                        data.Element("Computers").Add(new XElement("Computer", new XAttribute("userName", c.UserName), new XAttribute("Added", c.Added),
                                            new XElement("OS", c.OS),
                                            new XElement("CPU", c.Cpu),
                                            new XElement("GPU", c.Gpu),
                                            new XElement("RAM", c.Ram),
                                            new XElement("Motherboard", c.Motherboard),
                                            new XElement("Paste", c.Paste),
                                            noteRows,
                                            new XElement("Maintenance", c.Maintenance)
                                            ));
                    }
                }
            }
            catch { MsgBoxEditor.EditInfoMessage("sdasdasdad - " + Computers.Count, ""); }

            try { data.Save(Settings.Default.DataPath); }
            catch { MsgBoxEditor.EditErrorMessage("Saving data failed...\nError[Dx00100010]", "Saving data failed"); }
        }

        public void SaveData(string dataPath)
        {
            XDocument dataDocument = new XDocument(new XDeclaration("1.0", "UTF-8", null));
            foreach (Computer c in Computers)
            {
                XElement noteRows = new XElement("Note");

                foreach (var s in c.Note.Split('\n'))
                {
                    noteRows.Add(new XElement("Row", s));
                }

                dataDocument.Add(new XElement("Computer", new XAttribute("userName", c.UserName), new XAttribute("Added", c.Added),
                                    new XElement("OS", c.OS),
                                    new XElement("CPU", c.Cpu),
                                    new XElement("GPU", c.Gpu),
                                    new XElement("RAM", c.Ram),
                                    new XElement("Motherboard", c.Motherboard),
                                    new XElement("Paste", c.Paste),
                                    noteRows,
                                    new XElement("Maintenance", c.Maintenance)
                                    ));
            }

            try { dataDocument.Save(dataPath); }
            catch { MsgBoxEditor.EditErrorMessage("Saving data...\nInternal error[Dx00100010]", "Saving data failed"); }
        }

        public void SaveOne(string dataPath) 
        {
            if (mw.pcList.SelectedItem != null)
            {
                XDocument dataDocument = new XDocument(new XDeclaration("1.0", "UTF-8", null));
                XElement noteRows = new XElement("Note");
                foreach (var s in ((Computer)mw.pcList.SelectedItem).Note.Split('\n'))
                {
                    noteRows.Add(new XElement("Row", s));
                }

                dataDocument.Add(new XElement("Computer", new XAttribute("userName", ((Computer)mw.pcList.SelectedItem).UserName)), new XAttribute("Added", ((Computer)mw.pcList.SelectedItem).Added),
                                    new XElement("OS", ((Computer)mw.pcList.SelectedItem).OS),
                                    new XElement("CPU", ((Computer)mw.pcList.SelectedItem).Cpu),
                                    new XElement("GPU", ((Computer)mw.pcList.SelectedItem).Gpu),
                                    new XElement("RAM", ((Computer)mw.pcList.SelectedItem).Ram),
                                    new XElement("Motherboard", ((Computer)mw.pcList.SelectedItem).Motherboard),
                                    new XElement("Paste", ((Computer)mw.pcList.SelectedItem).Paste),
                                    noteRows,
                                    new XElement("Maintenance", ((Computer)mw.pcList.SelectedItem).Maintenance)
                                    );

            
                try { dataDocument.Save(dataPath); }
                catch { MsgBoxEditor.EditErrorMessage("Importing PC failed...\nInternal error[Dx00100011]", "Saving PC failed"); }
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
                        SaveOne(Path.Combine(dialog.SelectedPath, "Exported_PC.xml"));
                    }
                }
        }

        // --- IMPORTING DATA --- | --- IMPORTING DATA --- | --- IMPORTING DATA --- | --- IMPORTING DATA --- | --- IMPORTING DATA --- | --- IMPORTING DATA --- |
        public void ImportPC(string sender) 
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = "";
            dlg.DefaultExt = ".xml";
            dlg.Filter = ".txt|*txt*|.xml|*xml*";

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