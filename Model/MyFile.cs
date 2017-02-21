

using System.Collections.Generic;

namespace ConvertLinuxPerfFiles.Model
{
    class MyFile
    {
        private string fileName { get; set; }
        public string FileName
        {
            get { return fileName; }
            private set { fileName = value; }
        }
        public MyFile(string fileName)
        {
            FileName = fileName;
        }
    }

    class FileReader : MyFile
    {
        private List<string> fileContents;
        public FileReader(string fileName) :
            base(fileName)
        {

        }

        public List<string> Read()
        {
            fileContents = MyFileHelper.ReadFileByLine(this);
            return fileContents;
        }
    }

    class TsvFileWriter : MyFile
    {
        private string header;
        private List<string> metrics;
        public string Header
        {
            get { return header; }
            set { header = value; }
        }
        public List<string> Metrics
        {
            get { return metrics; }
            private set { metrics = value; }
        }
        public TsvFileWriter(string fileName, string header, List<string> metrics) :
            base(fileName)
        {
            Header = header;
            Metrics = metrics;
        }

        public void Write()
        {
            MyFileHelper.WriteTsvFileByLine(this);
        }
    }
}