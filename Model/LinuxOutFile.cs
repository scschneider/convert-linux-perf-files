using System.Collections.Generic;

namespace ConvertLinuxPerfFiles.Model
{
    class LinuxOutFile
    {
        public LinuxOutFile(string fileName, int timeZone)
        {
            TimeZone = timeZone;
            FileName = fileName;
        }
        public string FileName { get; set; }
        public List<string> FileContents { get; set; }
        public int TimeZone { get; set; }
        public string Header { get; set; }
        public List<string> Metrics { get; set; }
    }
}