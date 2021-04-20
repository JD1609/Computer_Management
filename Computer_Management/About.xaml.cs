using System.Windows;

namespace Computer_Management
{
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
            string version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
            if (Settings.Default.IsDarkModeEnabled) { Dark_mode.SetDarkMode(this, Settings.Default.Background, Settings.Default.Foreground, Settings.Default.BorderColor); }
            VersionLabel.Content = "Version: " + version.Remove(version.Length - 2);
        }

        private void Release_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/JD1609/Computer_Management/releases/tag/v1.1.0");
        }
        private void JDslink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.github.com/JD1609");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
