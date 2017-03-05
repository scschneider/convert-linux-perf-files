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
            
            string ioStatFileName = "*_iostat.out";
            string pidStatFileName = "*_process_pidstat.out";
            string mpStatFileName = "*_mpstats_cpu.out";

            LinuxOutFileIoStat ioStat = new LinuxOutFileIoStat(ioStatFileName);
            LinuxOutFilePidStat pidStat = new LinuxOutFilePidStat(pidStatFileName);
            LinuxOutFileMpStat mpStat = new LinuxOutFileMpStat(mpStatFileName);
        
            FileUtility fileUtility = new FileUtility();
            
            fileUtility.WriteTsvFileByLine(ioStatFileName, ioStat.Header, ioStat.Metrics);
            fileUtility.WriteTsvFileByLine(pidStatFileName, pidStat.Header, pidStat.Metrics);
            fileUtility.WriteTsvFileByLine(mpStatFileName, mpStat.Header, mpStat.Metrics);
        }
    }
}
