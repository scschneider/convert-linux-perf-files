using ConvertLinuxPerfFiles.Model;

namespace ConvertLinuxPerfFiles
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string ioStatFileName = "*iostat.out";
            
            IoStatFile ioStatFile = new IoStatFile(ioStatFileName);
            TsvFileWriter tsvFileWriter = new TsvFileWriter(ioStatFileName,ioStatFile.Header,ioStatFile.Metrics);
            tsvFileWriter.Write();        
        }// END Main
    }// END Program
}// END Namespace ConvertLinuxPerfFiles
