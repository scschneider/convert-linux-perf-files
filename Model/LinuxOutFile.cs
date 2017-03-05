using System.Collections.Generic;

namespace ConvertLinuxPerfFiles.Model
{
    // parent class for the Linux out file child classes. The common properties and constructor used by the child classes
    class LinuxOutFile
    {
        // class constructor
        public LinuxOutFile(string fileName)
        {
            TimeZone = ConfigValues.TimeZone;
            FileName = fileName;
        }
        // class properties
        public string FileName { get; set; }
        public List<string> FileContents { get; set; }
        public int TimeZone { get; set; }
        public string Header { get; set; }
        public List<string> Metrics { get; set; }
    }
}