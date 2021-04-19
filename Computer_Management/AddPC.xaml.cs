using System;
using System.Windows;
using System.Threading.Tasks;
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
            if (Settings.Default.IsDarkModeEnabled) { Dark_mode.SetDarkMode(this, Settings.Default.Background, Settings.Default.Midground, Settings.Default.Foreground); }
            loadingBorder.Visibility = Visibility.Hidden;
            loadingProgressBar.Value = 0;
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

        private void EnableElements() 
        {
            mw.userLabel.IsEnabled = true;
            mw.osLabel.IsEnabled = true;
            mw.cpuLabel.IsEnabled = true;
            mw.gpuLabel.IsEnabled = true;
            mw.ramLabel.IsEnabled = true;
            mw.mbLabel.IsEnabled = true;
            mw.pasteLabel.IsEnabled = true;

            mw.dustClearCheckBox.IsEnabled = true;
            mw.pasteChangeCheckBox.IsEnabled = true;
        }

        // --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- | --- BUTTONS --- |
        private void addPcBTN_Click(object sender, RoutedEventArgs e)
        {

            string user = userNameTxtBox.Text.Trim();
            bool sameName = false;

            // --- DUPLICATED NAME CHECK ---
            foreach (Computer c in database.Computers)
            {
                if (c.UserName == user)
                    sameName = true;
            }

            // --- ADDING PC ---
            if (sameName == true)
            {
                MsgBoxEditor.EditInfoMessage("Choosen username is already in computer list!", "Name duplication");
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
                                MsgBoxEditor.EditInfoMessage("Cleaning date must be in future!", "Wrong date");
                            else
                            {
                                //  --- ADD PC ---
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
                                    if (mb == "")
                                        mb = "None";

                                    string paste = pasteTypeComboBox.SelectedItem.ToString();
                                    string note = noteTextBox.Text;

                                    database.Computers.Add(new Computer(DateTime.Now, user, os, cpu, gpu, ram, mb, paste, note, datePicker.SelectedDate.Value));

                                    EnableElements();

                                    Close();
                                }
                                catch { MsgBoxEditor.EditErrorMessage("An error has been occurred! Computer wasn't added...\nError[Ax00011001]", "Adding PC failed"); }

                                // --- SAVE DATA ---
                                database.SaveData();
                                database.ListCountCheck();
                                Close();
                            }
                        }
                        catch { MsgBoxEditor.EditInfoMessage("Wrong selected date.\nPlease enter date in format: DD.MM.YYYY", ""); }
                    }
                    else
                        MsgBoxEditor.EditInfoMessage("Username can't be longer than 25 characters.", "");
                }
                else
                    MsgBoxEditor.EditInfoMessage("Username can't be shortly than 4 characters.", "");
            }
        }
        private async void thisPCBTN_Click(object sender, RoutedEventArgs e)
        {
            addPcBTN.IsEnabled = false;
            textBoxStackPanel.IsEnabled = false;
            noteTextBox.IsEnabled = false;
            loadingBorder.Visibility = Visibility.Visible;
            addPcBTN.IsEnabled = false;
            await Task.Delay(1);

            userNameTxtBox.Text = HwFinder.GetUser();
            {
                loadingProgressBar.Value += 5;
                await Task.Delay(1);
                loadingProgressBar.Value += 5;
                await Task.Delay(1);
                loadingProgressBar.Value += 7;
                await Task.Delay(1);
            }

            operatingSystemTxtBox.Text = HwFinder.GetOS();
            {
                loadingProgressBar.Value += 5;
                await Task.Delay(1);
                loadingProgressBar.Value += 5;
                await Task.Delay(1);
                loadingProgressBar.Value += 7;
                await Task.Delay(1);
            }

            cpuTxtBox.Text = HwFinder.GetCpu();
            {
                loadingProgressBar.Value += 5;
                await Task.Delay(1);
                loadingProgressBar.Value += 5;
                await Task.Delay(1);
                loadingProgressBar.Value += 7;
                await Task.Delay(1);
            }

            gpuTxtBox.Text = HwFinder.GetGpu();
            {
                loadingProgressBar.Value += 5;
                await Task.Delay(1);
                loadingProgressBar.Value += 5;
                await Task.Delay(1);
                loadingProgressBar.Value += 7;
                await Task.Delay(1);
            }

            ramTxtBox.Text = HwFinder.GetRam();
            {
                loadingProgressBar.Value += 5;
                await Task.Delay(1);
                loadingProgressBar.Value += 5;
                await Task.Delay(1);
                loadingProgressBar.Value += 7;
                await Task.Delay(1);
            }

            mbTxtBox.Text = HwFinder.GetMB();
            {
                loadingProgressBar.Value += 5;
                await Task.Delay(1);
                loadingProgressBar.Value += 5;
                await Task.Delay(1);
                loadingProgressBar.Value += 7;
                await Task.Delay(100);
            }

            loadingProgressBar.Value = 0;
            loadingBorder.Visibility = Visibility.Hidden;
            textBoxStackPanel.IsEnabled = true;
            noteTextBox.IsEnabled = true;
            addPcBTN.IsEnabled = true;
        }

        private void noteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int linesCount = noteTextBox.Text.Split('\n').Length;
            if (linesCount >= 19) { noteTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible; }
            else { noteTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden; }
        }

        private void addPCwindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.Escape))
            {
                this.Close();
            }
        }
    }
}
