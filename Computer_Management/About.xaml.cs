using System.Windows;

namespace Computer_Management
{
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
            Version.Content = "Version: " + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.github.com/JD1609");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
