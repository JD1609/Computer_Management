using System.Windows;

namespace Computer_Management
{
    public partial class SureWindow : Window
    {
        private Database database;
        private string Sender;

        public SureWindow(string sender)
        {
            InitializeComponent();
            Sender = sender;
            SetMainText();
        }
        public SureWindow(Database database, string sender)
        {
            InitializeComponent();
            if (Settings.Default.IsDarkModeEnabled) { Dark_mode.SetDarkMode(this, Settings.Default.Background, Settings.Default.Foreground); }
            this.database = database;
            Sender = sender;
            SetMainText();
        }

        private void SetMainText() 
        {
            if (Sender == "removePC") 
                { mainText.Content = "Are you sure about removing PC?"; Title = "Remove PC"; }
            if (Sender == "menuItem_SaveBackup")
                { mainText.Content = "Are you sure you want overwrite old backup?"; Title = "Save backup"; }
            if (Sender == "menuItem_LoadBackup")
                { mainText.Content = "Are you sure you want load backup and\noverwrite current data?"; Title = "Load backup"; }
            if (Sender == "menuItem_DeleteALL")
                { mainText.Content = "Are you sure about deleting all data?"; Title = "Delete all data"; }
            if (Sender == "LoadSettings")
                { mainText.Content = "Settings file is corrupted...\nDo you want restore default settings?"; Title = "Restore settings"; }
        }

        private void Choice() 
        {
            if (Sender == "removePC")
                database.RemovePc();

            if (Sender == "menuItem_DeleteALL")
            {
                database.Computers.Clear();
                database.SaveData();
                MsgBoxEditor.EditInfoMessage("All data successfully deleted", "All data deleted");
                database.ListCountCheck();
            }

            if (Sender == "menuItem_SaveBackup")
            {
                try { database.SaveData(database.BackUpPath); MsgBoxEditor.EditInfoMessage("Backup successfully saved!", "Backup saved"); }
                catch { MsgBoxEditor.EditErrorMessage("Saving backup failed...", "Error"); }
            }

            if (Sender == "menuItem_LoadBackup")
            {
                try { database.Computers.Clear(); database.LoadData(database.BackUpPath); }
                catch { MsgBoxEditor.EditErrorMessage("Loading backup failed...", "Error"); }

                try { database.SaveData(); }
                catch { MsgBoxEditor.EditErrorMessage("Uploading backup failed...", "Error"); }

                MsgBoxEditor.EditInfoMessage("Backup uploaded successfully!", "Backup uploaded");
            }

            if (Sender == "LoadSettings") 
            {
                SettingsClass.CreateDefault();
                Sender = "Restored";
            }
                
            this.Close();
        }

        //  --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- |
        private void yesBTN_Click(object sender, RoutedEventArgs e)
        {
            Choice();
        }

        private void noBTN_Click(object sender, RoutedEventArgs e)
        {
            if (Sender == "LoadSettings")
                System.Environment.Exit(0);
            else
                this.Close();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                Choice();
            if (e.Key == System.Windows.Input.Key.Escape)
                this.Close();
        }

        private void surewindowWindow_Closed(object sender, System.EventArgs e)
        {
            if (Sender == "LoadSettings")
                System.Environment.Exit(0);
        }
    }
}