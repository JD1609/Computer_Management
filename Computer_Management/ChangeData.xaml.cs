using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Computer_Management
{
    public partial class ChangeData : Window
    {
        private MainWindow mw;
        private Database database;
        private object Sender { get; set; }

        public ChangeData(MainWindow mw, Database database, string data, object sender)
        {
            InitializeComponent();
            Sender = sender;
            textBoxData.Text = data;
            this.mw = mw;
            this.database = database;
            SetTitle();
            pasteCombobox.ItemsSource = database.Pastas;

            if (((Computer)mw.pcList.SelectedItem).Paste == "Cheap")
                pasteCombobox.SelectedIndex = 0;
            if (((Computer)mw.pcList.SelectedItem).Paste == "Expensive")
                pasteCombobox.SelectedIndex = 1;

            if (((Label)Sender).Name != "pasteLabel")
                pasteCombobox.Visibility = Visibility.Hidden;
            else
                textBoxData.Visibility = Visibility.Hidden;
        }

        public void SetTitle()
        {
            string sndrName = ((Label)Sender).Name;
            if (sndrName == "userLabel")
            {
                labelTxt.Content = "User:";
            }
            if (sndrName == "osLabel")
            {
                labelTxt.Content = "Operating system:";
            }
            if (sndrName == "cpuLabel")
            {
                labelTxt.Content = "Processor:";
            }
            if (sndrName == "gpuLabel")
            {
                labelTxt.Content = "Graphic card:";
            }
            if (sndrName == "ramLabel")
            {
                labelTxt.Content = "RAM:";
            }
            if (sndrName == "mbLabel")
            {
                labelTxt.Content = "Motherboard:";
            }
            if (sndrName == "pasteLabel")
            {
                labelTxt.Content = "Paste:";
            }
        }

        private void Change() 
        {
            Computer c = (Computer)mw.pcList.SelectedItem;
            string paste = pasteCombobox.Text;
            string data = textBoxData.Text.Trim();
            if (String.IsNullOrEmpty(data))
                data = "None";


            if (((Label)Sender).Name != "pasteLabel")
            {
                try
                {
                    c.Change(((Label)Sender).Name, data);
                    ((Label)Sender).Content = data;
                }
                catch { MsgBoxEditor.EditErrorMessage("Changing data failed...!\nError[Cx00000001]", "Changing failed"); }
                
                try { database.SaveData(database.DataPath); }
                catch { MsgBoxEditor.EditErrorMessage("Changing data failed...!\nError[Cx00000010]", "Changing failed"); }
            }
            else 
            {
                try
                {
                    c.Change(((Label)Sender).Name, paste);
                    ((Label)Sender).Content = paste;
                }
                catch { MsgBoxEditor.EditErrorMessage("Changing data failed...!\nError[Cx00000011]", "Changing failed"); }
                
                try { database.SaveData(database.DataPath); }
                catch { MsgBoxEditor.EditErrorMessage("Changing data failed...!\nError[Cx00000100]", "Changing failed"); }
            }
        }

        // --- SETTING DATA --- | --- SETTING DATA --- | --- SETTING DATA --- | --- SETTING DATA --- | --- SETTING DATA --- | --- SETTING DATA --- | --- SETTING DATA --- |
        private void SaveBTN_Click(object sender, RoutedEventArgs e)
        {
            string data = textBoxData.Text.Trim();
            if (data.Contains(";"))
                MsgBoxEditor.EditInfoMessage("Component can not contains \';\' symbol", "");
            else 
            {
                Change();
                Close();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();

            if (e.Key == Key.Enter)
            {
                string data = textBoxData.Text.Trim();
                if (data.Contains(";"))
                    MsgBoxEditor.EditInfoMessage("Component can not contains \';\' symbol", "");
                else
                {
                    Change();
                    Close();
                }
            }
        }
    }
}