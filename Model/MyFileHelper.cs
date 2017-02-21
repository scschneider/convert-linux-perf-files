using System.Collections.Generic;
using System.IO;

namespace ConvertLinuxPerfFiles.Model
{
    class MyFileHelper
    {
        public static List<string> ReadFileByLine(FileReader file)
        {
            List<string> returnValue = new List<string>();
            FileStream fileStream = new FileStream(GetFullFilePath(file), FileMode.Open);

            using (StreamReader streamReader = new StreamReader(fileStream))
            {
                while (!streamReader.EndOfStream)
                {
                    string line = streamReader.ReadLine();
                    returnValue.Add(line);
                }
            }

            return returnValue;
        }
        public static void WriteTsvFileByLine(TsvFileWriter file)
        {
            FileStream fileStream = new FileStream(SetTsvFileName(file), FileMode.Create, FileAccess.Write);

            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.WriteLine(file.Header);
                
                foreach (string line in file.Metrics)
                {
                    streamWriter.WriteLine(line);
                }
            }
        }
        public static string GetFullFilePath(MyFile file)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Directory.GetFiles(currentDirectory, file.FileName)[0];

            return filePath;
        }
        public static string SetTsvFileName(MyFile file)
        {
            string currentFileName = GetFullFilePath(file);
            string tsvFileName = currentFileName.Replace(".out", ".tsv");

            return tsvFileName;
        }
    }
}