using System;
namespace Computer_Management
{
    public class Computer
    {
        public DateTime Added { get; private set; }
        public string UserName { get; private set; }
        public string OS { get; private set; }
        public string Cpu { get; private set; }
        public string Gpu { get; private set; }
        public string Ram { get; private set; }
        public string Motherboard { get; private set; }
        public string Paste { get; private set; }
        public string Note { get; private set; }
        public DateTime Maintenance { get; private set; }
        public bool DustClean { get; private set; }

        public Computer(DateTime added, string user, string os, string cpu, string gpu, string ram, string mb, string paste, string note, bool dustClean, DateTime maintenance) 
        {
            Added = added;
            UserName = user;
            OS = os;
            Cpu = cpu;
            Gpu = gpu;
            Ram = ram;
            Motherboard = mb;
            Paste = paste;
            Note = note;
            Maintenance = maintenance;
            DustClean = dustClean;
        }

        public void Change(string sender, string data) 
        {
            switch (sender) 
            {
                case "osLabel":
                    OS = data;
                    break;
                case "cpuLabel":
                    Cpu = data;
                    break;
                case "gpuLabel":
                    Gpu = data;
                    break;
                case "ramLabel":
                    Ram = data;
                    break;
                case "mbLabel":
                    Motherboard = data;
                    break;
                case "pasteLabel":
                    Paste = data;
                    break;
                case "changeNote":
                    Note = data;
                    break;
                case "date":
                    Maintenance = DateTime.Parse(data);
                    break;
                case "dust":
                    DustClean = bool.Parse(data);
                    break;
                default:
                    MsgBoxEditor.EditErrorMessage("Something went wrong...", "Changing error");
                    break;
            }
        }

        public override string ToString()
        {
            return UserName;
        }
    }
}