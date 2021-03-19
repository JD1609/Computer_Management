using System.Windows;

namespace Computer_Management
{
    public static class MsgBoxEditor
    {
        public static string EditText(string text) 
        {
            for (int i = text.Length; i < 80; i++)
                text += " ";

            return text;
        }

        public static void EditInfoMessage(string message, string title) 
        {
            string[] msgLine = message.Split('\n');
            for (int i = msgLine[0].Length; i < 80; i++)
                message += " ";

            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void EditErrorMessage(string message, string title)
        {
            string[] msgLine = message.Split('\n');
            for (int i = msgLine[0].Length; i < 80; i++)
                message += " ";

            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}