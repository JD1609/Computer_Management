using System.Windows;

namespace Computer_Management
{
    public partial class SureWindow : Window
    {
        private Database database;
        private string Sender;
        public SureWindow(Database database, string sender)
        {
            InitializeComponent();
            if (Settings.Default.IsDarkModeEnabled) { Dark_mode.SetDarkMode(this, Settings.Default.Background, Settings.Default.Midground, Settings.Default.Foreground); }
            this.database = database;
            Sender = sender;
            SetMainText();
        }

        private void SetMainText() 
        {
            if (Sender == "removePC") 
                { mainText.Content = "Are you sure about removing PC?"; Title = "Remove PC"; }
            if (Sender == "SaveBackup")
                { mainText.Content = "Are you sure you want overwrite old backup?"; Title = "Save backup"; }
            if (Sender == "LoadBackup")
                { mainText.Content = "Are you sure you want load backup and\noverwrite current data?"; Title = "Load backup"; }
            if (Sender == "deleteALL")
                { mainText.Content = "Are you sure about deleting all data?"; Title = "Delete all data"; }
        }

        private void Choice() 
        {
            if (Sender == "removePC")
                database.RemovePc();

            if (Sender == "deleteALL")
            {
                database.Computers.Clear();
                database.SaveData();
                MsgBoxEditor.EditInfoMessage("All data successfully deleted", "All data deleted");
                database.ListCountCheck();
            }

            if (Sender == "SaveBackup")
            {
                try { database.SaveData(database.BackUpPath); MsgBoxEditor.EditInfoMessage("Backup successfully saved!", "Backup saved"); }
                catch { MsgBoxEditor.EditErrorMessage("Saving backup failed...", "Error"); }
            }

            if (Sender == "LoadBackup")
            {
                try { database.Computers.Clear(); database.LoadData(database.BackUpPath); }
                catch { MsgBoxEditor.EditErrorMessage("Loading backup failed...", "Error"); }

                try { database.SaveData(); }
                catch { MsgBoxEditor.EditErrorMessage("Uploading backup failed...", "Error"); }

                MsgBoxEditor.EditInfoMessage("Backup uploaded successfully!", "Backup uploaded");
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
            this.Close();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                Choice();
            if (e.Key == System.Windows.Input.Key.Escape)
                this.Close();
        }
    }
}