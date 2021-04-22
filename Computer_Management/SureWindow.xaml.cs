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
            switch (Sender) 
            {
                case "removePC":
                    mainText.Content = "Are you sure about removing PC?"; Title = "Remove PC";
                    break;
                case "menuItem_SaveBackup":
                    mainText.Content = "Are you sure you want overwrite old backup?"; Title = "Save backup";
                    break;
                case "menuItem_LoadBackup":
                    mainText.Content = "Are you sure you want load backup and\noverwrite current data?"; Title = "Load backup";
                    break;
                case "menuItem_DeleteALL":
                    mainText.Content = "Are you sure about deleting all data?"; Title = "Delete all data";
                    break;
                case "LoadSettings":
                    mainText.Content = "Settings file is corrupted...\nDo you want restore default settings?"; Title = "Restore settings";
                    break;
            }
        }

        private void Choice() 
        {
            switch (Sender) 
            {
                case "removePC":
                    database.RemovePc();
                    break;
                case "menuItem_DeleteALL":
                    database.Computers.Clear();
                    database.SaveData();
                    MsgBoxEditor.EditInfoMessage("All data successfully deleted", "All data deleted");
                    database.ListCountCheck();
                    break;
                case "menuItem_SaveBackup":
                    try { database.SaveData(database.BackUpPath); MsgBoxEditor.EditInfoMessage("Backup successfully saved!", "Backup saved"); }
                    catch { MsgBoxEditor.EditErrorMessage("Saving backup failed...", "Error"); }
                    break;
                case "menuItem_LoadBackup":
                    try { database.Computers.Clear(); database.LoadData(database.BackUpPath); }
                    catch { MsgBoxEditor.EditErrorMessage("Loading backup failed...", "Error"); }

                    try { database.SaveData(); }
                    catch { MsgBoxEditor.EditErrorMessage("Uploading backup failed...", "Error"); }

                    MsgBoxEditor.EditInfoMessage("Backup uploaded successfully!", "Backup uploaded");
                    break;
                case "LoadSettings":
                    SettingsClass.CreateDefault();
                    Sender = "Restored";
                    break;
                default:
                    MsgBoxEditor.EditErrorMessage("Something went wrong", "Error");
                    break;
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