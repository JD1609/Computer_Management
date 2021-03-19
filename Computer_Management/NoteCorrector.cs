using System;

namespace Computer_Management
{
    public class NoteCorrector
    {
        public static string CorrectNote(string data) //for saving
        {
            string note = "";

            string[] noteField = data.Trim().Split('\n');
            foreach (string row in noteField)
            {
                if (String.IsNullOrEmpty(row.Trim()))
                    note = note + "$";
                else 
                {
                    if (row.Trim().StartsWith("_"))
                        note = note + "$" + row.Trim();
                    else
                    {
                        if (!row.Trim().StartsWith("-"))
                        {
                            note = note + "$-" + row.Trim();
                        }
                        else
                            note = note + "$" + row.Trim();
                    }
                }
            }

            return note.Trim();
        }
    }
}
