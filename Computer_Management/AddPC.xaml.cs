﻿using System;
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
                                    if (mb == "")
                                        mb = "None";

                                    string paste = pasteTypeComboBox.SelectedItem.ToString();
                                    string note = NoteCorrector.CorrectNote(noteTextBox.Text);

                                    database.Computers.Add(new Computer(user, os, cpu, gpu, ram, mb, paste, note, datePicker.SelectedDate.Value));

                                    mw.dustClearCheckBox.IsEnabled = true;
                                    mw.pasteChangeCheckBox.IsEnabled = true;

                                    Close();
                                }
                                catch { MsgBoxEditor.EditErrorMessage("An error has been occurred! Computer wasn't added...\nError[Ax00011001]", "Adding PC failed"); }

                                // --- SAVE DATA --- | --- SAVE DATA --- | --- SAVE DATA --- | --- SAVE DATA --- | --- SAVE DATA --- | --- SAVE DATA --- | --- SAVE DATA --- |
                                try
                                {
                                    database.SaveData();
                                    Close();
                                }
                                catch
                                { MsgBoxEditor.EditErrorMessage("An error has been occurred! Data wasn't saved.\nError[Ax00100001]", "Saving failed"); }
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

        // --- BUTTON --- | --- BUTTON --- | --- BUTTON --- | --- BUTTON --- |
        private async void thisPCBTN_Click(object sender, RoutedEventArgs e)
        {
            loadingBorder.Visibility = Visibility.Visible;
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
        }

        private void noteTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            int linesCount = noteTextBox.Text.Split('\n').Length;
            if (linesCount >= 19) { noteTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible; }
            else { noteTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden; }
        }
    }
}
