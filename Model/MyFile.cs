using System.Collections.Generic;

namespace ConvertLinuxPerfFiles.Model
{
    class MyFile
    {
        public MyFile()
        {

        }
        public string FileName { get; set; }
    }

    class FileReader : MyFile
    {
        private List<string> FileContents;
        public FileReader() :
            base()
        {

        }
        public List<string> Read(string fileName)
        {
            FileName = fileName;
            FileContents = MyFileHelper.ReadFileByLine(this);
            return FileContents;
        }
    }

    class TsvFileWriter : MyFile
    {
        public TsvFileWriter() :
                    base()
        {

        }
        public string Header { get; set; }
        public List<string> Metrics { get; set; }

        public void Write(string fileName, string header, List<string> metrics)
        {
            FileName = fileName;
            Header = header;
            Metrics = metrics;
            MyFileHelper.WriteTsvFileByLine(this);
        }
    }
}