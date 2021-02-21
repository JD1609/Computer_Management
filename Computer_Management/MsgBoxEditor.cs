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


        public static void EditMessage(string message, string title) 
        {
            for (int i = message.Length; i < 80; i++)
                message += " ";

            MessageBox.Show(message, title);
        }
    }
}