using System;
using System.Management;

namespace Computer_Management
{
    public static class HwFinder
    {
        public static string GetUser()
        {
            return Environment.UserName.ToString();
        }

        public static string GetOS()
        {
            string OsName = "";
            ManagementObjectSearcher os = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            foreach (ManagementObject obj in os.Get())
            {
                try { OsName = obj["Caption"].ToString().Trim(); }
                catch { MsgBoxEditor.EditErrorMessage("Failed to load OS...!\nError[Ax00001001]", ""); }
            }

            return OsName;
        }

        public static string GetCpu()
        {
            string cpuName = "";
            float cpuClock = 0F;
            string cpuCore = "";
            string cpuThreat = "";

            bool name = true;
            bool clock = true;
            bool coreThreat = true;

            ManagementObjectSearcher cpu = new ManagementObjectSearcher("select * from Win32_Processor");
            foreach (ManagementObject obj in cpu.Get())
            {
                try{ cpuName = obj["Name"].ToString().Trim(); }
                catch{ MsgBoxEditor.EditErrorMessage("Failed to load CPU...!\nError[Ax00010001]", ""); name = false; }

                try{ cpuClock = float.Parse(obj["MaxClockSpeed"].ToString()) / 1000; }
                catch{ MsgBoxEditor.EditErrorMessage("Failed to load CPU clock...!\nError[Ax00010010]", ""); clock = false; }

                try{ cpuCore = obj["NumberOfEnabledCore"].ToString().Trim(); }
                catch{ MsgBoxEditor.EditErrorMessage("Failed to load number of CPU cores...!\nError[Ax00010101]", ""); coreThreat = false; } //R-PC

                try{ cpuThreat = obj["NumberOfLogicalProcessors"].ToString().Trim(); }
                catch{ MsgBoxEditor.EditErrorMessage("Failed to load number of CPU threats...!\nError[Ax00010110]", ""); coreThreat = false; }
            }

            if (name && clock && coreThreat) { return cpuName + " @" + cpuClock + "GHz " + "[" + cpuCore + "C/" + cpuThreat + "T" + "]"; }
            else if (name && clock) { return cpuName + " @" + cpuClock + "GHz "; }
            else if (name) { return cpuName; }
            else { return ""; }
        }

        public static string GetGpu()
        {
            string gpuName = "";

            ManagementObjectSearcher gpu = new ManagementObjectSearcher("select * from Win32_VideoController");
            foreach (ManagementObject obj in gpu.Get())
            {
                try { gpuName = obj["Name"].ToString().Trim(); }
                catch { MsgBoxEditor.EditErrorMessage("Failed to load GPU...!\nError[Ax00011110]", ""); }
            }

            return gpuName;
        }

        public static string GetRam()
        {
            string ramName = "";
            string ramCap = "";
            string ramClock = "";
            string ramType = RAMinfo.RamType;

            ManagementObjectSearcher RAMClock = new ManagementObjectSearcher("select * from Win32_PhysicalMemory");
            foreach (ManagementObject obj in RAMClock.Get())
            {
                try { ramName = obj["Manufacturer"].ToString().Trim(); }
                catch { MsgBoxEditor.EditErrorMessage("Failed to load RAM...!\nError[Ax00100001]", ""); }

                try { ramClock = obj["Speed"].ToString().Trim(); }
                catch { MsgBoxEditor.EditErrorMessage("Failed to load RAM...!\nError[Ax00100010]", ""); }
            }

            ManagementObjectSearcher RAMCap = new ManagementObjectSearcher("select * from Win32_ComputerSystem");
            foreach (ManagementObject obj in RAMCap.Get())
            {
                try 
                {
                    Int64 memory = Int64.Parse(obj["TotalPhysicalMemory"].ToString()) / 1060568064;
                    ramCap = memory.ToString().Trim() + "GB";
                }
                catch { MsgBoxEditor.EditErrorMessage("Failed to load RAM...!\nError[Ax00100011]", ""); }
            }

            return ramName + " " + ramCap + " " + ramType + " " + "@" + ramClock + "Mhz";
        }

        public static string GetHDD() 
        {
            //... make code here
            return "";
        }

        public static string GetMB()
        {
            string mbName = "";

            ManagementObjectSearcher MB = new ManagementObjectSearcher("select * from Win32_BaseBoard");
            foreach (ManagementObject queryObj in MB.Get())
            {
                try { mbName = queryObj["Product"].ToString(); }
                catch { MsgBoxEditor.EditErrorMessage("Failed to load RAM...!\nError[Ax001001001]", ""); }
            }

            return mbName;
        }
    }
}
