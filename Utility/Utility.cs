using System;
using System.Collections.Generic;
using System.IO;

namespace ConvertLinuxPerfFiles.Utility
{
    class DateTimeUtility
    {
        public string FromUnixTime(long unixTime, int timeZone = 0)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // returns the converted time and adjusts it to the timezone
            return epoch.AddSeconds(unixTime).AddHours(timeZone).ToString("MM/dd/yyyy H:mm:ss"); ;
        }
    }

    class FileUtility
    {
        public List<string> ReadFileByLine(string file)
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
        public void WriteTsvFileByLine(string file, string header, List<string> metrics)
        {
            FileStream fileStream = new FileStream(SetTsvFileName(file), FileMode.Create, FileAccess.Write);

            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.WriteLine(header);
                
                foreach (string line in metrics)
                {
                    streamWriter.WriteLine(line);
                }
            }
        }
        public static string GetFullFilePath(string file)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Directory.GetFiles(currentDirectory, file)[0];

            return filePath;
        }
        public static string SetTsvFileName(string file)
        {
            string currentFileName = GetFullFilePath(file);
            string tsvFileName = currentFileName.Replace(".out", ".tsv");

            return tsvFileName;
        }
    }

    class TypeConversion
    {
        public bool ConvertTypeToBool(string typeValue)
    {
        bool pv;
        switch (typeValue)
        {
            case "1":
            case "true":
            case "yes":
                pv = true;
                break;
            case "0":
            case "false":
            case "no":
                pv = false;
                break;
            default:
                pv = false;
                break;
        }
        
        return pv;
    }
    }
}