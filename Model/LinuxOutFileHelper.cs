using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ConvertLinuxPerfFiles.Model
{
    // contains common methods used by the linuxoutfile classes to get devices and/or headers.
    class LinuxOutFileHelper
    {
        public List<string> GetDevices(int startingLine, List<string> fileContents, int deviceColumnNumber)
        {
            List<string> devices = new List<string>();

            string emptyLinePattern = "^\\s*$";
            string splitPattern = "\\s+";

            Regex rgxEmptyLine = new Regex(emptyLinePattern);
            Regex rgxSplitLine = new Regex(splitPattern);

            int blockCount = 1;
            int block = 0;
            int lineNumber = startingLine;

            while (block < blockCount)
            {
                if (!rgxEmptyLine.IsMatch(fileContents[lineNumber]))
                {
                    string[] thisLineValues = rgxSplitLine.Split(fileContents[lineNumber]);
                    devices.Add(thisLineValues[deviceColumnNumber]);
                    lineNumber++;
                }
                else
                {
                    block++;
                }
            }

            return devices;
        }

        public string GetHeader(OutHeader outHeader)
        {
            string splitPattern = "\\s+";
            Regex rgx = new Regex(splitPattern);
            string[] outHeaderSplit = rgx.Split(outHeader.FileContents[outHeader.StartingRow]);
            StringBuilder header = new StringBuilder();
            header.Append('"' + "(PDH-TSV 4.0) (Pacific Daylight Time)(420)" + '"' + "\t");

            foreach (string device in outHeader.Devices)
            {
                for (int i = outHeader.StartingColumn; i < outHeaderSplit.Length; i++)
                {
                    header.Append('"' + "\\\\" + ConfigValues.MachineName + "\\" + outHeader.ObjectName + "(" + device + ")\\" + outHeaderSplit[i] + '"' + "\t");
                }
            }// END foreach device

            return header.ToString();
        }
    }

    class OutHeader
    {
        public int StartingColumn { get; set; }
        public int EndColumn { get; set; }
        public int StartingRow { get; set; }
        public List<string> FileContents { get; set; }
        public List<string> Devices { get; set; }
        public string ObjectName { get; set; }

    }

}