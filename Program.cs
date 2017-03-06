using System;
using ConvertLinuxPerfFiles.Logging;
using ConvertLinuxPerfFiles.Model;
using ConvertLinuxPerfFiles.Utility;

namespace ConvertLinuxPerfFiles
{
    public static class Globals
    {
        public static Log log = new Log();
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            LoggingConfig.LogFileName = "ImportLog.log";
            Config config = new Config();

            if (ConfigValues.ImportIoStat) { ImportIo(); }
            if (ConfigValues.ImportMemFree) { ImportMemFree(); }
            if (ConfigValues.ImportMemSwap) { ImportMemSwap(); }
            if (ConfigValues.ImportMpStat) { ImportMp(); }
            if (ConfigValues.ImportNetStats) { ImportNet(); }
            if (ConfigValues.ImportPidStat) { ImportPid(); }
            // Console.ReadKey();
        }

        private static void ImportIo()
        {
            string ioStatFileName = "*_iostat.out";
            LinuxOutFileIoStat ioStat = new LinuxOutFileIoStat(ioStatFileName);
            new FileUtility().WriteTsvFileByLine(ioStatFileName, ioStat.Header, ioStat.Metrics);
        }

        private static void ImportMemFree()
        {
            string memFreeFileName = "*_memory_free.out";
            LinuxOutFileMemFree memFree = new LinuxOutFileMemFree(memFreeFileName);
            new FileUtility().WriteTsvFileByLine(memFreeFileName, memFree.Header, memFree.Metrics);
        }
        private static void ImportMemSwap()
        {
            string memSwapFileName = "*_memory_swap.out";
            LinuxOutFileMemSwap memSwap = new LinuxOutFileMemSwap(memSwapFileName);
            new FileUtility().WriteTsvFileByLine(memSwapFileName, memSwap.Header, memSwap.Metrics);
        }

        private static void ImportMp()
        {
            string mpStatFileName = "*_mpstats_cpu.out";
            LinuxOutFileMpStat mpStat = new LinuxOutFileMpStat(mpStatFileName);
            new FileUtility().WriteTsvFileByLine(mpStatFileName, mpStat.Header, mpStat.Metrics);
        }

        private static void ImportNet()
        {
            string networkFileName = "*_network_stats.out";
            LinuxOutFileNetwork network = new LinuxOutFileNetwork(networkFileName);
            new FileUtility().WriteTsvFileByLine(networkFileName, network.Header, network.Metrics);
        }

        private static void ImportPid()
        {
            string pidStatFileName = "*_process_pidstat.out";
            LinuxOutFilePidStat pidStat = new LinuxOutFilePidStat(pidStatFileName);
            new FileUtility().WriteTsvFileByLine(pidStatFileName, pidStat.Header, pidStat.Metrics);
        }
    }
}
