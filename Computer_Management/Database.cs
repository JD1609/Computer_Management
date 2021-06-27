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
            mw.pasteType.ItemsSource = Pastas;
            BackUpPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Data_Backup.xml");
        }

        // --- LIST COUNT CHECK --- | --- LIST COUNT CHECK --- | --- LIST COUNT CHECK --- | --- LIST COUNT CHECK --- | --- LIST COUNT CHECK --- | --- LIST COUNT CHECK --- |
        public void ListCountCheck() 
        {
            if (Computers.Count == 0)
            {
                Computers.Clear();
                mw.pcList.ItemsSource = Computers;
                mw.diskLabel.Content = null;
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

                mw.dataStackpanel.IsEnabled = false;

                mw.duplicatePC.Visibility = System.Windows.Visibility.Hidden;
                mw.noteTextBox.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Hidden;
                mw.noteTextBox.IsEnabled = false;
                mw.noteTextBox.Text = "";
                MsgBoxEditor.EditInfoMessage("No PCs found...", "No data");
            }

            else
            {
                if (Settings.Default.SortingBy == 0)
                    mw.pcList.ItemsSource = Computers.OrderBy(c => c.Added);
                if (Settings.Default.SortingBy == 1)
                    mw.pcList.ItemsSource = Computers.OrderByDescending(c => c.Added);
                if (Settings.Default.SortingBy == 2)
                    mw.pcList.ItemsSource = Computers.OrderBy(c => c.Maintenance);
                if (Settings.Default.SortingBy == 3)
                    mw.pcList.ItemsSource = Computers.OrderBy(c => c.UserName);
                if (Settings.Default.SortingBy == 4)
                    mw.pcList.ItemsSource = Computers.OrderByDescending(c => c.UserName);

                mw.pcList.SelectedIndex = 0;

                mw.dataStackpanel.IsEnabled = true;

                mw.noteTextBox.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Hidden;
                mw.duplicatePC.Visibility = System.Windows.Visibility.Visible;
                mw.noteTextBox.IsEnabled = true;
            }
        }

        // --- LOADING DATA --- | --- LOADING DATA --- | --- LOADING DATA --- | --- LOADING DATA --- | --- LOADING DATA --- | --- LOADING DATA --- | --- LOADING DATA --- |
        public void LoadData(string dataPath)
        {
            dataDocument = XDocument.Load(dataPath);
            DateTime added = DateTime.Now;
            Computer.Type type = Computer.Type.PC;
            string user = "Unknown";
            string os = "Unknown";
            string cpu = "Unknown";
            string gpu = "Unknown";
            string ram = "Unknown";
            string disk = "Unknown";
            string mb = "Unknown";
            string paste = "Cheap";
            DateTime maintenance = DateTime.Today.AddMonths(3);
            bool dustClean = false;

            foreach (XElement c in dataDocument.Element("Computers").Elements("Computer"))
            {
                string note = "";
                added = DateTime.Parse(c.Attribute("added").Value);

                switch (c.Attribute("type").Value)
                {
                    case "PC":
                        type = Computer.Type.PC;
                        break;
                    case "NTB":
                        type = Computer.Type.NTB;
                        break;
                    case "NAS":
                        type = Computer.Type.NTB;
                        break;
                    case "SRVR":
                        type = Computer.Type.SRVR;
                        break;
                }

                user = c.Element("Username").Value;
                os = c.Element("OS").Value;
                cpu = c.Element("CPU").Value;
                gpu = c.Element("GPU").Value;
                ram = c.Element("RAM").Value;
                disk = c.Element("Disk").Value;
                mb = c.Element("Motherboard").Value;
                paste = c.Element("Paste").Value;
                foreach (XElement row in c.Element("Note").Elements("Row"))
                    note += row.Value.Trim() + "\n";
                maintenance = DateTime.Parse(c.Element("Maintenance").Value);
                dustClean = bool.Parse(c.Attribute("dustClean").Value);

                Computers.Add(new Computer(added, type, user, os, cpu, gpu, ram, disk, mb, paste, note, dustClean, maintenance));
            }

            ListCountCheck();
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
        private void MakeDocument()
        {
            dataDocument = new XDocument(new XDeclaration("1.O", "UTF-8", null));
            dataDocument.Add(new XElement("Computers"));

            if (Computers.Count != 0)
            {
                foreach (Computer c in Computers)
                {
                    XElement noteRows = new XElement("Note");
                    foreach (var s in c.Note.Split('\n'))
                    {
                        noteRows.Add(new XElement("Row", s.Trim()));
                    }

                    dataDocument.Element("Computers").Add(new XElement("Computer", new XAttribute("type", c.DeviceType),
                                                                                   new XAttribute("added", c.Added),
                                                                                   new XAttribute("dustClean", c.DustClean),
                                                                                        new XElement("Username", c.UserName),
                                                                                        new XElement("OS", c.OS),
                                                                                        new XElement("CPU", c.Cpu),
                                                                                        new XElement("GPU", c.Gpu),
                                                                                        new XElement("RAM", c.Ram),
                                                                                        new XElement("Disk", c.Disk),
                                                                                        new XElement("Motherboard", c.Motherboard),
                                                                                        new XElement("Paste", c.Paste),
                                                                                        noteRows,
                                                                                        new XElement("Maintenance", c.Maintenance)
                                                                                        ));
                }
            }
        }
        public void SaveData()
        {
            MakeDocument();
            try { dataDocument.Save(Settings.Default.DataPath); }
            catch { MsgBoxEditor.EditErrorMessage("Saving data failed...\nError[Dx00100001]", "Saving data failed"); }
        }

        public void SaveData(string dataPath)
        {
            MakeDocument();
            try { dataDocument.Save(dataPath); }
            catch { MsgBoxEditor.EditErrorMessage("Saving data failed...\nError[Dx00100010]", "Saving data failed"); }
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

                dataDocument.Add(new XElement("Computers", new XElement("Computer", new XAttribute("type", ((Computer)mw.pcList.SelectedItem).DeviceType),
                                                                                    new XAttribute("added", ((Computer)mw.pcList.SelectedItem).Added),
                                                                                    new XAttribute("dustClean", ((Computer)mw.pcList.SelectedItem).Added),
                                                                                         new XElement("OS", ((Computer)mw.pcList.SelectedItem).OS),
                                                                                         new XElement("CPU", ((Computer)mw.pcList.SelectedItem).Cpu),
                                                                                         new XElement("GPU", ((Computer)mw.pcList.SelectedItem).Gpu),
                                                                                         new XElement("RAM", ((Computer)mw.pcList.SelectedItem).Ram),
                                                                                         new XElement("Disk", ((Computer)mw.pcList.SelectedItem).Disk),
                                                                                         new XElement("Motherboard", ((Computer)mw.pcList.SelectedItem).Motherboard),
                                                                                         new XElement("Paste", ((Computer)mw.pcList.SelectedItem).Paste),
                                                                                         noteRows,
                                                                                         new XElement("Maintenance", ((Computer)mw.pcList.SelectedItem).Maintenance)
                                    )));

            
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
            dlg.Filter = ".xml|*xml*|.txt|*txt*";

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