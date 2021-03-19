using System.Windows;

namespace Computer_Management
{
    public partial class ShortcutsWindow : Window
    {
        public ShortcutsWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
