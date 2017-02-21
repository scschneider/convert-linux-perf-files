using System.Collections.Generic;
using ConvertLinuxPerfFiles.Model;

namespace ConvertLinuxPerfFiles.Model
{
    class LinuxOutFile
    {
        string fileName;
        List<string> fileContents;
        string header;
        List<string> metrics;

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
            FileContents = new FileReader(fileName).Read();
            Devices = LinuxOutFileHelper.GetIoStatDevices(this);
            Header = LinuxOutFileHelper.GetIoStatHeader(this);
            Metrics = LinuxOutFileHelper.GetIoStatMetrics(this);
        }
    }
}