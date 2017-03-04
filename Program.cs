using ConvertLinuxPerfFiles.Model;

namespace ConvertLinuxPerfFiles
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string ioStatFileName = "*iostat.out";
            string pidStatFileName = "*process_pidstat.out";

            Config config = new Config();

            IoStatFile ioStatFile = new IoStatFile(ioStatFileName).GetNormalizedContent();
            LinuxOutFilePidStatHelper linuxOutFilePidStatHelper = new LinuxOutFilePidStatHelper(pidStatFileName, config.TimeZone, config.PidStatFilter);
        
            TsvFileWriter tsvFileWriter = new TsvFileWriter();

            tsvFileWriter.Write(ioStatFileName, ioStatFile.Header, ioStatFile.Metrics);
            tsvFileWriter.Write(pidStatFileName, linuxOutFilePidStatHelper.Header, linuxOutFilePidStatHelper.Metrics);
        }// END Main
    }// END Program
}// END Namespace ConvertLinuxPerfFiles
