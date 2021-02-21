using System;
using System.Windows;
using System.Windows.Controls;

namespace Computer_Management
{
    public partial class AddPC : Window
    {
        private Database database;
        private MainWindow mw;
        public AddPC(MainWindow mw, Database database, string username, string os, string cpu, string gpu, string ram, string mb)
        {
            InitializeComponent();
            this.database = database;
            this.mw = mw;
            pasteTypeComboBox.ItemsSource = database.Pastas;
            userNameTxtBox.Text = username;
            operatingSystemTxtBox.Text = os;
            cpuTxtBox.Text = cpu;
            gpuTxtBox.Text = gpu;
            ramTxtBox.Text = ram;
            mbTxtBox.Text = mb;
            datePicker.SelectedDate = DateTime.Today.AddMonths(3);
        }

        private void addPcBTN_Click(object sender, RoutedEventArgs e)
        {

            string user = userNameTxtBox.Text.Trim();
            bool sameName = false;

            // --- DUPLICATED NAME CHECK --- |--- DUPLICATED NAME CHECK --- |--- DUPLICATED NAME CHECK --- |--- DUPLICATED NAME CHECK --- |--- DUPLICATED NAME CHECK --- |
            foreach (Computer c in database.Computers) 
            {
                if (c.UserName == user)
                    sameName = true;
            }

            // --- ADDING PC --- | --- ADDING PC --- | --- ADDING PC --- | --- ADDING PC --- | --- ADDING PC --- | --- ADDING PC --- | --- ADDING PC --- | --- ADDING PC --- |
            if (sameName == true)
            {
                MessageBox.Show("Choosen username is already in computer list!", "Name duplication", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else 
            {
                if (user.Length > 4)
                {
                    if (user.Length < 26)
                    {
                        try
                        {
                            DateTime dateTime = datePicker.SelectedDate.Value;

                            if (datePicker.SelectedDate.Value <= DateTime.Today)
                                MessageBox.Show("Cleaning date must be in future!", "Wrong date", MessageBoxButton.OK, MessageBoxImage.Information);
                            else
                            {
                                //  --- ADD PC --- |  --- ADD PC --- |  --- ADD PC --- |  --- ADD PC --- |  --- ADD PC --- |  --- ADD PC --- |  --- ADD PC --- |  --- ADD PC --- |
                                try
                                {
                                    string os = operatingSystemTxtBox.Text.Trim();
                                        if (os == "")
                                            os = "None";

                                    string cpu = cpuTxtBox.Text.Trim();
                                        if (cpu == "")
                                            cpu = "None";

                                    string gpu = gpuTxtBox.Text.Trim();
                                        if (gpu == "")
                                            gpu = "None";

                                    string ram = ramTxtBox.Text.Trim();
                                        if (ram == "")
                                            ram = "None";

                                    string mb = mbTxtBox.Text.Trim();
                                        if(mb == "")
                                            mb = "None";

                                    string paste = pasteTypeComboBox.SelectedItem.ToString();
                                    string note = NoteCorrector.CorrectNote(noteTextBox.Text);

                                    database.Computers.Add(new Computer(user, os, cpu, gpu, ram, mb, paste, note, datePicker.SelectedDate.Value));

                                    mw.dustClearCheckBox.IsEnabled = true;
                                    mw.pasteChangeCheckBox.IsEnabled = true;

                                    Close();
                                }
                                catch
                                {
                                    MessageBox.Show("An error has been occurred! Computer wasn't added...\nError[Ax00011001]", "Adding PC failed", MessageBoxButton.OK, MessageBoxImage.Error);
                                }

                                // --- SAVE DATA --- | --- SAVE DATA --- | --- SAVE DATA --- | --- SAVE DATA --- | --- SAVE DATA --- | --- SAVE DATA --- | --- SAVE DATA --- |
                                try
                                {
                                    database.SaveData(database.DataPath);
                                    Close();
                                }
                                catch
                                {
                                    MessageBox.Show("An error has been occurred! Data wasn't saved.\nError[Ax00100001]", "Saving failed", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Wrong selected date.\nPlease enter date in format: DD.MM.YYYY", "Date selection error", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                        MessageBox.Show("Username can't be longer than 25 characters.", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show("Username can't be shortly than 4 characters.", "UserName", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // --- BUTTON --- | --- BUTTON --- | --- BUTTON --- | --- BUTTON --- |
        private void thisPCBTN_Click(object sender, RoutedEventArgs e)
        {
            userNameTxtBox.Text = HwFinder.GetUser();
            operatingSystemTxtBox.Text = HwFinder.GetOS();
            cpuTxtBox.Text = HwFinder.GetCpu();
            gpuTxtBox.Text = HwFinder.GetGpu();
            ramTxtBox.Text = HwFinder.GetRam();
            mbTxtBox.Text = HwFinder.GetMB();
        }

        private void noteTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            int linesCount = noteTextBox.Text.Split('\n').Length;
            if (linesCount >= 19) { noteTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible; }
            else { noteTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden; }
        }
    }
}
