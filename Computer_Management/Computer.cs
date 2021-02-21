using System;

namespace Computer_Management
{
    public class Computer
    {
        public string UserName { get; private set; }
        public string OS { get; private set; }
        public string Cpu { get; private set; }
        public string Gpu { get; private set; }
        public string Ram { get; private set; }
        public string Motherboard { get; private set; }
        public string Paste { get; private set; }
        public string Note { get; private set; }
        public DateTime NextCleaning { get; private set; }

        public Computer(string user, string os, string cpu, string gpu, string ram, string mb, string paste, string note, DateTime nextCleaning) 
        {
            UserName = user;
            OS = os;
            Cpu = cpu;
            Gpu = gpu;
            Ram = ram;
            Motherboard = mb;
            Paste = paste;
            Note = note;
            NextCleaning = nextCleaning;
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
                NextCleaning = DateTime.Parse(data);
            }
        }

        public override string ToString()
        {
            return UserName;
        }
    }
}