using ConvertLinuxPerfFiles.Model;
using ConvertLinuxPerfFiles.Utility;

namespace ConvertLinuxPerfFiles
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string ioStatFileName = "*iostat.out";
            string pidStatFileName = "*process_pidstat.out";

            Config config = new Config();

            LinuxOutFileIoStat ioStat = new LinuxOutFileIoStat(ioStatFileName);
            LinuxOutFilePidStat pidStat = new LinuxOutFilePidStat(pidStatFileName, config.TimeZone, config.PidStatFilter);
        
            FileUtility fileUtility = new FileUtility();
            
            fileUtility.WriteTsvFileByLine(ioStatFileName, ioStat.Header, ioStat.Metrics);
            fileUtility.WriteTsvFileByLine(pidStatFileName, pidStat.Header, pidStat.Metrics);
        }// END Main
    }// END Program
}// END Namespace ConvertLinuxPerfFiles
