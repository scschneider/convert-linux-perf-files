using System.Collections.Generic;
// using ConvertLinuxPerfFiles.Model;

namespace ConvertLinuxPerfFiles.Model
{
    class LinuxOutFile
    {
        private string fileName;
        private List<string> fileContents;
        private string header;
        private List<string> metrics;

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public List<string> FileContents
        {
            get { return fileContents; }
            set { fileContents = value; }
        }

        public string Header
        {
            get { return header; }
            set { header = value; }
        }

        public List<string> Metrics
        {
            get { return metrics; }
            set { metrics = value; }
        }

        public LinuxOutFile(string fileName)
        {
            
        }
    }

    class IoStatFile : LinuxOutFile
    {
        private List<string> devices;
        public List<string> Devices
        {
            get { return devices; }
            set { devices = value; }
        }
        public IoStatFile(string fileName) :
            base(fileName)
        {
            FileName = fileName;
        }
        public IoStatFile GetNormalizedContent()
        {
            FileContents = new FileReader().Read(FileName);
            Devices = LinuxOutFileIoStatHelper.GetIoStatDevices(this);
            Header = LinuxOutFileIoStatHelper.GetIoStatHeader(this);
            Metrics = LinuxOutFileIoStatHelper.GetIoStatMetrics(this);
            
            return this;
        }
    }

    class PidStatFile : LinuxOutFile
    {
        private List<long> pids;
        public List<long> Pids
        {
            get { return pids; }
            set { pids = value; }

        }
        public PidStatFile(string fileName) :
            base(fileName)
        {
            FileName = fileName;
        }

        public PidStatFile GetNormalizedContent()
        {
            FileContents = new FileReader().Read(FileName);
            return this;
        }
    }
}