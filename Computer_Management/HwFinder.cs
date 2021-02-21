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
                OsName = obj["Caption"].ToString().Trim();
            }

            return OsName;
        }

        public static string GetCpu()
        {
            string cpuName = "";
            string cpuCore = "";
            string cpuThreat = "";
            float cpuClock = 0F;

            ManagementObjectSearcher cpu = new ManagementObjectSearcher("select * from Win32_Processor");
            foreach (ManagementObject obj in cpu.Get())
            {
                cpuName = obj["Name"].ToString().Trim();
                cpuClock = float.Parse(obj["MaxClockSpeed"].ToString()) / 1000;
                cpuCore = obj["NumberOfEnabledCore"].ToString().Trim();
                cpuThreat = obj["NumberOfLogicalProcessors"].ToString().Trim();
            }

            return cpuName + " @" + cpuClock + "GHz " + "[" + cpuCore + "C/" + cpuThreat + "T" + "]";
        }

        public static string GetGpu()
        {
            string gpuName = "";

            ManagementObjectSearcher gpu = new ManagementObjectSearcher("select * from Win32_VideoController");
            foreach (ManagementObject obj in gpu.Get())
            {
                gpuName = obj["Name"].ToString().Trim();
            }

            return gpuName;
        }

        public static string GetRam()
        {
            string ramName = "";
            string ramCap = "";
            string ramClock = "";
            string ramType = RAMinfo.RamType;

            ManagementObjectSearcher RAMCap = new ManagementObjectSearcher("select * from Win32_ComputerSystem");
            foreach (ManagementObject obj in RAMCap.Get())
            {
                Int64 memory = Int64.Parse(obj["TotalPhysicalMemory"].ToString()) / 1060568064;
                ramCap = memory.ToString().Trim() + "GB";
            }

            ManagementObjectSearcher RAMClock = new ManagementObjectSearcher("select * from Win32_PhysicalMemory");
            foreach (ManagementObject obj in RAMClock.Get())
            {
                ramName = obj["Manufacturer"].ToString().Trim();
                ramClock = obj["Speed"].ToString().Trim();
            }

            return ramName + " " + ramCap + " " + ramType + " " + "@" + ramClock + "Mhz";
        }

        public static string GetMB()
        {
            string mbName = "";

            ManagementObjectSearcher MB = new ManagementObjectSearcher("select * from Win32_BaseBoard");
            foreach (ManagementObject queryObj in MB.Get())
            {
                mbName = queryObj["Product"].ToString();
            }

            return mbName;
        }
    }
}
