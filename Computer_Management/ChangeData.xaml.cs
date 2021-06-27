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
            if (((Label)Sender).Name == "pasteLabel") 
            {
                loadComponent.Visibility = Visibility.Hidden;
            }
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
                    titleLabel.Content = "User:";
                    loadComponent.ToolTip += "user";
                    break;
                case "osLabel":
                    titleLabel.Content = "Operating system:";
                    loadComponent.ToolTip += "operating system";
                    break;
                case "cpuLabel":
                    titleLabel.Content = "Processor:";
                    loadComponent.ToolTip += "processor";
                    break;
                case "gpuLabel":
                    titleLabel.Content = "Graphic card:";
                    loadComponent.ToolTip += "graphic card";
                    break;
                case "ramLabel":
                    titleLabel.Content = "RAM:";
                    loadComponent.ToolTip += "RAM";
                    break;
                case "diskLabel":
                    titleLabel.Content = "Disk:";
                    loadComponent.ToolTip += "disk";
                    break;
                case "mbLabel":
                    titleLabel.Content = "Motherboard:";
                    loadComponent.ToolTip += "motherboard";
                    break;
                case "pasteLabel":
                    titleLabel.Content = "Paste:";
                    loadComponent.ToolTip += "paste";
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

        // --- LOADING COMPONENT --- | --- LOADING COMPONENT --- | --- LOADING COMPONENT --- | --- LOADING COMPONENT --- | --- LOADING COMPONENT --- | --- LOADING COMPONENT --- |
        private void loadComponent_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (((Label)Sender).Name)
            {
                case "osLabel":
                    textBoxData.Text = HwFinder.GetOS();
                    break;
                case "cpuLabel":
                    textBoxData.Text = HwFinder.GetCpu();
                    break;
                case "gpuLabel":
                    textBoxData.Text = HwFinder.GetGpu();
                    break;
                case "ramLabel":
                    textBoxData.Text = HwFinder.GetRam();
                    break;
                case "diskLabel":
                    textBoxData.Text = HwFinder.GetHDD();
                    break;
                case "mbLabel":
                    textBoxData.Text = HwFinder.GetMB();
                    break;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();

            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.L))
            {
                switch (((Label)Sender).Name)
                {
                    case "osLabel":
                        textBoxData.Text = HwFinder.GetOS();
                        break;
                    case "cpuLabel":
                        textBoxData.Text = HwFinder.GetCpu();
                        break;
                    case "gpuLabel":
                        textBoxData.Text = HwFinder.GetGpu();
                        break;
                    case "ramLabel":
                        textBoxData.Text = HwFinder.GetRam();
                        break;
                    case "diskLabel":
                        textBoxData.Text = HwFinder.GetHDD();
                        break;
                    case "mbLabel":
                        textBoxData.Text = HwFinder.GetMB();
                        break;
                }
            }

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