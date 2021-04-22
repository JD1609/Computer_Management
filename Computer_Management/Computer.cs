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
            if (sender == "osLabel") 
            {
                OS = data;
            }
            if (sender == "cpuLabel") 
            {
                Cpu = data;
            }
            if (sender == "gpuLabel") 
            {
                Gpu = data;
            }
            if (sender == "ramLabel") 
            {
                Ram = data;
            }
            if (sender == "mbLabel") 
            {
                Motherboard = data;
            }
            if (sender == "pasteLabel") 
            {
                Paste = data;
            }
            if (sender == "changeNote") 
            {
                Note = data;
            }
            if (sender == "date")
            {
                Maintenance = DateTime.Parse(data);
            }
            if (sender == "dust")
            {
                DustClean = bool.Parse(data);
            }
        }

        public override string ToString()
        {
            return UserName;
        }
    }
}