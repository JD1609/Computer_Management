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
        private string RecievedData { get; set; }

        public ChangeData(MainWindow mw, Database database, string data, object sender)
        {
            InitializeComponent();
            if (Settings.Default.IsDarkModeEnabled) { Dark_mode.SetDarkMode(this, Settings.Default.Background, Settings.Default.Midground, Settings.Default.Foreground); }
            Sender = sender;
            RecievedData = data;
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
            switch (sndrName) 
            {
                case "userLabel":
                    labelTxt.Content = "User:";
                    break;
                case "osLabel":
                    labelTxt.Content = "Operating system:";
                    break;
                case "cpuLabel":
                    labelTxt.Content = "Processor:";
                    break;
                case "gpuLabel":
                    labelTxt.Content = "Graphic card:";
                    break;
                case "ramLabel":
                    labelTxt.Content = "RAM:";
                    break;
                case "diskLabel":
                    labelTxt.Content = "Disk:";
                    break;
                case "mbLabel":
                    labelTxt.Content = "Motherboard:";
                    break;
                case "pasteLabel":
                    labelTxt.Content = "Paste:";
                    break;
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
                
                try { database.SaveData(); }
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
                
                try { database.SaveData(); }
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
                if (textBoxData.Text == RecievedData)
                    this.Close();
                else
                    Change();
                    this.Close();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();

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