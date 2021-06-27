using System.Windows;
using System.Windows.Media;

namespace Computer_Management
{
    public static class Dark_mode
    {
        public static void SetDarkMode(MainWindow main_window, string background_color, string midground_color, string foreground_color)
        {
            // --- Switch
            main_window.darkModeSwitch_enabled_border.Visibility = Visibility.Visible;
            main_window.darkModeSwitch_enabled.Visibility = Visibility.Visible;
            main_window.darkModeSwitch_disabled.Visibility = Visibility.Hidden;

            // --- Main_Window
            main_window.Background = (Brush)new BrushConverter().ConvertFromString(background_color);
                // --- Menu header
            main_window.menu.Background = (Brush)new BrushConverter().ConvertFromString(background_color);
            main_window.menuDataHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.menuBackupsHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.menuOptionsHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
                // --- Data menu
            main_window.menuItem_ImportPC.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);
            main_window.menuItem_ImportPC.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.menuItem_ExportPC.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);
            main_window.menuItem_ExportPC.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.menuItem_DeleteALL.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);
            main_window.menuItem_DeleteALL.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.menuItem_OpenFolder.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);
            main_window.menuItem_OpenFolder.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
                // --- Backup menu
            main_window.menuItem_SaveBackup.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);
            main_window.menuItem_SaveBackup.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.menuItem_SaveBackupTEF.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);
            main_window.menuItem_SaveBackupTEF.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.menuItem_LoadBackup.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);
            main_window.menuItem_LoadBackup.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.menuItem_LoadBackupFEF.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);
            main_window.menuItem_LoadBackupFEF.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
                //--- Options menu
            main_window.menuItem_Settings.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);
            main_window.menuItem_Settings.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.menuItem_Shortcuts.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);
            main_window.menuItem_Shortcuts.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.menuItem_About.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);
            main_window.menuItem_About.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
                // --- PC list
            main_window.pcListHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.pcList.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.pcList.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);
            // --- dataStackpanel
            main_window.dataStackpanelBorder.BorderBrush = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.osLabelHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.osLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.cpuLabelHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.cpuLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.gpuLabelHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.gpuLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.ramLabelHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.ramLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.diskLabelHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.diskLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.mbLabelHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.mbLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.pasteLabelHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.pasteLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.nextCleaningDate.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.dustClearCheckBox.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.pasteChangeCheckBox.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
                // --- Note
            main_window.noteTextBox.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            main_window.noteTextBox.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);
            main_window.noteLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
        }

        public static void SetDarkMode(AddPC addpc_window, string background_color, string midground_color, string foreground_color)
        {
            // --- AddPC window
            addpc_window.Background = (Brush)new BrushConverter().ConvertFromString(background_color);
            // --- Label stackPanel
                // --- Specification Header
            addpc_window.specificationLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            addpc_window.typeLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);

            addpc_window.usernameLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            addpc_window.userNameTxtBox.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            addpc_window.userNameTxtBox.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);

            addpc_window.osLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            addpc_window.operatingSystemTxtBox.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            addpc_window.operatingSystemTxtBox.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);

            addpc_window.cpuLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            addpc_window.cpuTxtBox.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            addpc_window.cpuTxtBox.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);

            addpc_window.gpuLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            addpc_window.gpuTxtBox.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            addpc_window.gpuTxtBox.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);

            addpc_window.ramLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            addpc_window.ramTxtBox.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            addpc_window.ramTxtBox.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);

            addpc_window.diskLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            addpc_window.diskTxtBox.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            addpc_window.diskTxtBox.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);

            addpc_window.mbLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            addpc_window.mbTxtBox.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            addpc_window.mbTxtBox.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);

            addpc_window.pasteLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            addpc_window.pasteTypeComboBox.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            addpc_window.pasteTypeComboBox.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);

            addpc_window.maintenanceLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);

            addpc_window.pasteTypeComboBox.Foreground = (Brush)new BrushConverter().ConvertFromString("#FF000000");

            addpc_window.notesLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            addpc_window.noteTextBox.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            addpc_window.noteTextBox.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);
        }

        public static void SetDarkMode(SettingsWindow settings_window, string background_color, string midground_color, string foreground_color)
        {
            // --- Settings Window
            settings_window.settingsWindow.Background = (Brush)new BrushConverter().ConvertFromString(background_color);

            settings_window.dataPathHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            settings_window.dataPathLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            settings_window.ToolTipTxtBox.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);

            settings_window.nextPasteHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            settings_window.sortingHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            settings_window.startUpCheckBox.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);

        }

        public static void SetDarkMode(About about_window, string background_color, string foreground_color, string border_color)
        {
            // --- About Window
            about_window.aboutWindow.Background = (Brush)new BrushConverter().ConvertFromString(background_color);

            about_window.nameLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            about_window.VersionLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            about_window.releaseNotesLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
                // --- Created By
            about_window.createdBy.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            about_window.createdByName.Foreground = (Brush)new BrushConverter().ConvertFromString("#FFFF7400");
            about_window.createdByBorder.BorderBrush = (Brush)new BrushConverter().ConvertFromString(border_color);
        }

        public static void SetDarkMode(ChangeData changeData_window, string background_color, string midground_color, string foreground_color)
        {
            // --- ChangeData Window
            changeData_window.changedataWindow.Background = (Brush)new BrushConverter().ConvertFromString(background_color);
            changeData_window.textBoxData.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            changeData_window.textBoxData.Background = (Brush)new BrushConverter().ConvertFromString(midground_color);
                // --- ChangeData Title
                changeData_window.titleLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
        }

        public static void SetDarkMode(ShortcutsWindow shortcuts_window, string background_color, string foreground_color, string border_color)
        {
            // --- ShortCuts Window
            shortcuts_window.shortcutsWindow.Background = (Brush)new BrushConverter().ConvertFromString(background_color);
            shortcuts_window.border.BorderBrush = (Brush)new BrushConverter().ConvertFromString(border_color);
                // --- Headers
            shortcuts_window.importPCHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            shortcuts_window.exportPCHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            shortcuts_window.openFolderHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            shortcuts_window.deleteAllHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            shortcuts_window.addPCHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            shortcuts_window.removePCHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            shortcuts_window.duplicatePCHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            shortcuts_window.importBackupHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            shortcuts_window.exportBackupHeader.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
                // --- Shortcuts
            shortcuts_window.importPCLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            shortcuts_window.exportPCLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            shortcuts_window.openFolderLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            shortcuts_window.deleteAllLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            shortcuts_window.addPCLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            shortcuts_window.removePCLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            shortcuts_window.duplicatePCLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            shortcuts_window.importBackupLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
            shortcuts_window.exportBackupLabel.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
        }                    

        public static void SetDarkMode(SureWindow sureWindow_window, string background_color, string foreground_color)
        {
            sureWindow_window.surewindowWindow.Background = (Brush)new BrushConverter().ConvertFromString(background_color);
            sureWindow_window.mainText.Foreground = (Brush)new BrushConverter().ConvertFromString(foreground_color);
        }
        public static void Deactivate(MainWindow main_window)
        {
            // --- Switch
            main_window.darkModeSwitch_enabled_border.Visibility = Visibility.Hidden;
            main_window.darkModeSwitch_enabled.Visibility = Visibility.Hidden;
            main_window.darkModeSwitch_disabled.Visibility = Visibility.Visible;

            // --- Main_Window
            main_window.Background = Brushes.White;
                // --- Menu header
            main_window.menu.Background = Brushes.White;
            main_window.menuDataHeader.Foreground = Brushes.Black;
            main_window.menuBackupsHeader.Foreground = Brushes.Black;
            main_window.menuOptionsHeader.Foreground = Brushes.Black;
                // --- Data menu
            main_window.menuItem_ImportPC.Background = (Brush)new BrushConverter().ConvertFromString("#FFF0F0F0");
            main_window.menuItem_ImportPC.Foreground = Brushes.Black;            
            main_window.menuItem_ExportPC.Background = (Brush)new BrushConverter().ConvertFromString("#FFF0F0F0");
            main_window.menuItem_ExportPC.Foreground = Brushes.Black;
            main_window.menuItem_DeleteALL.Background = (Brush)new BrushConverter().ConvertFromString("#FFF0F0F0");
            main_window.menuItem_DeleteALL.Foreground = Brushes.Black;
            main_window.menuItem_OpenFolder.Background = (Brush)new BrushConverter().ConvertFromString("#FFF0F0F0");
            main_window.menuItem_OpenFolder.Foreground = Brushes.Black;
                // --- Backup menu
            main_window.menuItem_SaveBackup.Background = (Brush)new BrushConverter().ConvertFromString("#FFF0F0F0");
            main_window.menuItem_SaveBackup.Foreground = Brushes.Black;
            main_window.menuItem_SaveBackupTEF.Background = (Brush)new BrushConverter().ConvertFromString("#FFF0F0F0");
            main_window.menuItem_SaveBackupTEF.Foreground = Brushes.Black;
            main_window.menuItem_LoadBackup.Background = (Brush)new BrushConverter().ConvertFromString("#FFF0F0F0");
            main_window.menuItem_LoadBackup.Foreground = Brushes.Black;
            main_window.menuItem_LoadBackupFEF.Background = (Brush)new BrushConverter().ConvertFromString("#FFF0F0F0");
            main_window.menuItem_LoadBackupFEF.Foreground = Brushes.Black;
                //--- Options menu
            main_window.menuItem_Settings.Background = (Brush)new BrushConverter().ConvertFromString("#FFF0F0F0");
            main_window.menuItem_Settings.Foreground = Brushes.Black;
            main_window.menuItem_Shortcuts.Background = (Brush)new BrushConverter().ConvertFromString("#FFF0F0F0");
            main_window.menuItem_Shortcuts.Foreground = Brushes.Black;
            main_window.menuItem_About.Background = (Brush)new BrushConverter().ConvertFromString("#FFF0F0F0");
            main_window.menuItem_About.Foreground = Brushes.Black;
                // --- PC list
            main_window.pcListHeader.Foreground = Brushes.Black;
            main_window.pcList.Foreground = Brushes.Black;
            main_window.pcList.Background = Brushes.White;
                // --- dataStackpanel
            main_window.dataStackpanelBorder.BorderBrush = Brushes.Black;
            main_window.osLabelHeader.Foreground = Brushes.Black;
            main_window.osLabel.Foreground = Brushes.Black;
            main_window.cpuLabelHeader.Foreground = Brushes.Black;
            main_window.cpuLabel.Foreground = Brushes.Black;
            main_window.gpuLabelHeader.Foreground = Brushes.Black;
            main_window.gpuLabel.Foreground = Brushes.Black;
            main_window.ramLabelHeader.Foreground = Brushes.Black;
            main_window.ramLabel.Foreground = Brushes.Black;
            main_window.diskLabelHeader.Foreground = Brushes.Black;
            main_window.diskLabel.Foreground = Brushes.Black;
            main_window.mbLabelHeader.Foreground = Brushes.Black;
            main_window.mbLabel.Foreground = Brushes.Black;
            main_window.pasteLabelHeader.Foreground = Brushes.Black;
            main_window.pasteLabel.Foreground = Brushes.Black;
            main_window.nextCleaningDate.Foreground = Brushes.Black;
            main_window.dustClearCheckBox.Foreground = Brushes.Black;
            main_window.pasteChangeCheckBox.Foreground = Brushes.Black;
                // --- Note
            main_window.noteTextBox.Foreground = Brushes.Black;
            main_window.noteTextBox.Background = Brushes.White;
            main_window.noteLabel.Foreground = Brushes.Black;
        }
    }
}
