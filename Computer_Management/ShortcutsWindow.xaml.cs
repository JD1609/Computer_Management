using System.Windows;

namespace Computer_Management
{
    public partial class ShortcutsWindow : Window
    {
        public ShortcutsWindow()
        {
            InitializeComponent();
            if (Settings.Default.IsDarkModeEnabled) { Dark_mode.SetDarkMode(this, Settings.Default.Background, Settings.Default.Foreground); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
