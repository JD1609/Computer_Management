using System;
using System.IO;
using System.Xml;
using System.Linq;
using Microsoft.Win32;
using System.Collections.ObjectModel;

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
            BackUpPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Computer management", "Data_Backup.xml");
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
                MsgBoxEditor.EditErrorMessage("No PCs found...", "Missing data");
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
            using (XmlReader xmlReader = XmlReader.Create(dataPath)) 
            {
                string element = "";

                string userName = "";
                string os = "";
                string cpu = "";
                string gpu = "";
                string ram = "";
                string mb = "";
                string paste = "";
                string note = "";
                DateTime maintenance = DateTime.Today;

                while (xmlReader.Read()) 
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        element = xmlReader.Name;
                        if (element == "Computer")
                        {
                            userName = xmlReader.GetAttribute("UserName");
                        }
                    }

                    else if (xmlReader.NodeType == XmlNodeType.Text)
                    {
                        switch (element)
                        {
                            case "OS":
                                os = xmlReader.Value;
                                break;
                            case "CPU":
                                cpu = xmlReader.Value;
                                break;
                            case "GPU":
                                gpu = xmlReader.Value;
                                break;
                            case "RAM":
                                ram = xmlReader.Value;
                                break;
                            case "Motherboard":
                                mb = xmlReader.Value;
                                break;
                            case "Paste":
                                paste = xmlReader.Value;
                                break;
                            case "Note":
                                note = xmlReader.Value;
                                break;
                            case "Maintenance":
                                maintenance = DateTime.Parse(xmlReader.Value);
                                break;
                        }
                    }

                    else if ((xmlReader.NodeType == XmlNodeType.EndElement) && (xmlReader.Name == "Computer"))
                        Computers.Add(new Computer(userName, os, cpu, gpu, ram, mb, paste, note, maintenance));
                }
            }

            if (Settings.Default.SortingBy == 0)
                mw.pcList.ItemsSource = Computers;
            if (Settings.Default.SortingBy == 1)
                mw.pcList.ItemsSource = Computers.OrderBy(c => c.UserName);
            if (Settings.Default.SortingBy == 2)
                mw.pcList.ItemsSource = Computers.OrderByDescending(c => c.UserName);

            mw.pcList.SelectedIndex = 0;
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
        public void SaveData()
        {
            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;

            using (XmlWriter xmlWriter = XmlWriter.Create(Settings.Default.DataPath, xmlSettings)) 
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Computers");

                foreach (Computer c in Computers) 
                {
                    xmlWriter.WriteStartElement("Computer");
                    xmlWriter.WriteAttributeString("UserName", c.UserName);

                    xmlWriter.WriteElementString("OS", c.OS);
                    xmlWriter.WriteElementString("CPU", c.Cpu);
                    xmlWriter.WriteElementString("GPU", c.Gpu);
                    xmlWriter.WriteElementString("RAM", c.Ram);
                    xmlWriter.WriteElementString("Motherboard", c.Motherboard);
                    xmlWriter.WriteElementString("Paste", c.Paste);
                    xmlWriter.WriteElementString("Note", c.Note);
                    xmlWriter.WriteElementString("Maintenance", c.NextCleaning.ToString());

                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
            }
        }

        public void SaveData(string dataPath)
        {
            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;

            using (XmlWriter xmlWriter = XmlWriter.Create(dataPath, xmlSettings))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Computers");

                foreach (Computer c in Computers)
                {
                    xmlWriter.WriteStartElement("Computer");
                    xmlWriter.WriteAttributeString("UserName", c.UserName);

                    xmlWriter.WriteElementString("OS", c.OS);
                    xmlWriter.WriteElementString("CPU", c.Cpu);
                    xmlWriter.WriteElementString("GPU", c.Gpu);
                    xmlWriter.WriteElementString("RAM", c.Ram);
                    xmlWriter.WriteElementString("Motherboard", c.Motherboard);
                    xmlWriter.WriteElementString("Paste", c.Paste);
                    xmlWriter.WriteElementString("Note", c.Note);
                    xmlWriter.WriteElementString("Maintenance", c.NextCleaning.ToString());

                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
            }
        }

        public void SaveOne(string dataPath) 
        {
            if (mw.pcList.SelectedItem != null)
            {
                XmlWriterSettings xmlSettings = new XmlWriterSettings();
                xmlSettings.Indent = true;

                using (XmlWriter xmlWriter = XmlWriter.Create(dataPath, xmlSettings)) 
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("Computers");

                    xmlWriter.WriteStartElement("Computer");
                    xmlWriter.WriteAttributeString("UserNname", ((Computer)mw.pcList.SelectedItem).UserName);

                    xmlWriter.WriteElementString("OS", ((Computer)mw.pcList.SelectedItem).OS);
                    xmlWriter.WriteElementString("CPU", ((Computer)mw.pcList.SelectedItem).Cpu);
                    xmlWriter.WriteElementString("GPU", ((Computer)mw.pcList.SelectedItem).Gpu);
                    xmlWriter.WriteElementString("RAM", ((Computer)mw.pcList.SelectedItem).Ram);
                    xmlWriter.WriteElementString("Motherboard", ((Computer)mw.pcList.SelectedItem).Motherboard);
                    xmlWriter.WriteElementString("Paste", ((Computer)mw.pcList.SelectedItem).Paste);
                    xmlWriter.WriteElementString("Note", ((Computer)mw.pcList.SelectedItem).Note);
                    xmlWriter.WriteElementString("Maintenance", ((Computer)mw.pcList.SelectedItem).NextCleaning.ToString());
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndElement();
                    xmlWriter.Flush();
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