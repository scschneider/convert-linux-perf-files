using System;
using System.Collections.Generic;
using System.IO;

namespace ConvertLinuxPerfFiles.Utility
{

    // utility class that contains common methods used by multiple classes and/or methods
    // contains some basic logging
    class DateTimeUtility
    {
        public string FromUnixTime(long unixTime, int timeZone = 0)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime dt = new DateTime();
            // returns the converted time and adjusts it to the timezone
            dt = epoch.AddSeconds(unixTime).AddHours(timeZone);
            return DateTime24HourFormat(dt);
        }

        public string DateTime24HourFormat(DateTime dateToFormat)
        {
            return dateToFormat.ToString("MM/dd/yyyy H:mm:ss");
        }
    }

    class FileUtility
    {
        public List<string> ReadFileByLine(string file)
        {
            List<string> returnValue = new List<string>();
            try
            {
                FileStream fileStream = new FileStream(GetFullFilePath(file), FileMode.Open);
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string line = streamReader.ReadLine();
                        returnValue.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Globals.log.WriteLog(file + ": " + e.Message, "ReadFileByLine", "[Error]");
                throw;
            }

            return returnValue;
        }
        public void WriteTsvFileByLine(string file, string header, List<string> metrics)
        {
            try
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
            catch (Exception e)
            {
                Globals.log.WriteLog(file + ": " + e.Message, "WriteTsvFileByLine", "[Error]");
                throw;
            }

        }
        public static string GetFullFilePath(string file)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            try
            {
                string filePath = Directory.GetFiles(currentDirectory, file)[0];

                return filePath;
            }
            catch (Exception e)
            {
                Globals.log.WriteLog(file + ": " + e.Message, "GetFullFilePath", "[Error]");
                throw;
            }
        }
        public static string SetTsvFileName(string file)
        {
            try
            {
                string currentFileName = GetFullFilePath(file);
                string tsvFileName = currentFileName.Replace(".out", ".tsv");

                return tsvFileName;
            }
            catch (Exception e)
            {
                Globals.log.WriteLog(file + ": " + e.Message, "SetTsvFileName", "[Error]");
                throw;
            }
        }
    }

    class TypeConversionUtility
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