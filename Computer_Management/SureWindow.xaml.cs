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
                database.SaveData(database.DataPath);
                MsgBoxEditor.EditMessage("All data successfully deleted", "All data deleted");
                database.ListCountCheck();
            }

            if (Sender == "SaveBackup")
            {
                try { database.SaveData(database.BackUpPath); MsgBoxEditor.EditMessage("Backup successfully saved!", "Backup saved"); }
                catch { MsgBoxEditor.EditMessage("Saving backup failed...", "Saving backup failed"); }
            }

            if (Sender == "LoadBackup")
            {
                try { database.LoadData(database.BackUpPath); }
                catch { MsgBoxEditor.EditMessage("Loading backup failed...", "Loading backup failed"); }

                try { database.SaveData(database.DataPath); }
                catch { MsgBoxEditor.EditMessage("Uploading backup failed...", "Uploading backup failed"); }

                MsgBoxEditor.EditMessage("Backup uploaded successfully!", "Backup uploaded");
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