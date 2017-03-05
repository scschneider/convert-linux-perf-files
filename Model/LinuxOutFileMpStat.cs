using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ConvertLinuxPerfFiles.Utility;

namespace ConvertLinuxPerfFiles.Model
{
    class LinuxOutFileMpStat : LinuxOutFile
    {
        // class constructor
        public LinuxOutFileMpStat(string mpStatFileName) :
            base(mpStatFileName)
        {
            FileContents = GetMpStatFileContents();
            Devices = GetMpStatDevices();
            Header = GetMpStatHeader();
            Metrics = GetMpStatMetrics();
        }

        // class properties
        private List<string> Devices { get; set; }

        // class methods
        // get filecontents
        private List<string> GetMpStatFileContents()
        {
            return new FileUtility().ReadFileByLine(FileName);
        }
        // get the list of devices, in this case device is the cpu
        private List<string> GetMpStatDevices()
        {
            int startingLine = 3;
            int deviceColumnNumber = 2;

            return new LinuxOutFileHelper().GetDevices(startingLine, FileContents, deviceColumnNumber);
        }
        // get the header that will be written to the tsv file
        private string GetMpStatHeader()
        {
            OutHeader outHeader = new OutHeader()
            {
                StartingColumn = 3,
                StartingRow = 2,
                FileContents = FileContents,
                Devices = Devices,
                ObjectName = "Processor"
            };

            return new LinuxOutFileHelper().GetHeader(outHeader);
        }
        // generate the metrics that will get written to the tsv file
        private List<string> GetMpStatMetrics()
        {
            List<string> metrics = new List<string>();

            string emptyLinePattern = "^\\s*$";
            string splitPattern = "\\s+";

            Regex rgxEmptyLine = new Regex(emptyLinePattern);
            Regex rgxSplitLine = new Regex(splitPattern);

            int deviceCount = Devices.Count;

            // looping through each line in the contents of this out file
            for (int i = 1; i < FileContents.Count;)
            {
                DateTime timeStamp;
                string timeStampFormatted = "";
                StringBuilder thisMetricSample = new StringBuilder();

                // this file is in a block format and we use empty lines to determin when to start parsing the next metric
                if (rgxEmptyLine.IsMatch(FileContents[i]) && i < FileContents.Count - 1)
                {
                    // advances to the line of the next metric
                    i = i + 2;

                    // grabbing timestamp information for this current metric
                    string[] thisLineContents = rgxSplitLine.Split(FileContents[i]);
                    timeStamp = DateTime.Parse(thisLineContents[0] + thisLineContents[1]);
                    timeStampFormatted = new DateTimeUtility().DateTime24HourFormat(timeStamp);
                    thisMetricSample.Append('"' + timeStampFormatted + '"' + "\t");
                }

                // this is where the metric data gets parsed and added to the collection
                for (int x = 1; x <= deviceCount; x++)
                {
                    string[] thisLineContents = rgxSplitLine.Split(FileContents[i]);

                    // read the contents of the split line, start at column 3 and read each string until the end of the array.
                    for (int j = 3; j < thisLineContents.Length; j++)
                    {
                        thisMetricSample.Append('"' + thisLineContents[j] + '"' + "\t");
                    }
                    
                    // the logic of incrementing the for loop for the file contents file is with in the for statement since we need to do some more complicated parsing.
                    i++;
                }

                metrics.Add(thisMetricSample.ToString());
            }
            return metrics;
        }
    }

}


/* EXAMPLE of MpStat_CPU file
Linux 3.10.0-327.28.3.el7.x86_64 (dl380g802-v02) 	01/11/2017 	_x86_64_	(8 CPU)

12:58:00 PM  CPU    %usr   %nice    %sys %iowait    %irq   %soft  %steal  %guest  %gnice   %idle
12:58:10 PM  all    0.48    0.00    0.36    0.03    0.00    0.01    0.00    0.00    0.00   99.12
12:58:10 PM    0    0.10    0.00    0.10    0.00    0.00    0.00    0.00    0.00    0.00   99.79
12:58:10 PM    1    2.40    0.00    0.70    0.10    0.00    0.00    0.00    0.00    0.00   96.80
12:58:10 PM    2    0.10    0.00    0.50    0.00    0.00    0.00    0.00    0.00    0.00   99.40
12:58:10 PM    3    0.80    0.00    0.20    0.00    0.00    0.00    0.00    0.00    0.00   99.00
12:58:10 PM    4    0.00    0.00    0.20    0.00    0.00    0.00    0.00    0.00    0.00   99.80
12:58:10 PM    5    0.10    0.00    0.20    0.00    0.00    0.00    0.00    0.00    0.00   99.70
12:58:10 PM    6    0.00    0.00    0.10    0.00    0.00    0.00    0.00    0.00    0.00   99.90
12:58:10 PM    7    0.20    0.00    0.90    0.00    0.00    0.00    0.00    0.00    0.00   98.90

12:58:10 PM  CPU    %usr   %nice    %sys %iowait    %irq   %soft  %steal  %guest  %gnice   %idle
12:58:20 PM  all    0.29    0.00    0.23    0.00    0.00    0.00    0.00    0.00    0.00   99.48
12:58:20 PM    0    0.00    0.00    0.10    0.00    0.00    0.00    0.00    0.00    0.00   99.90
12:58:20 PM    1    1.10    0.00    0.60    0.00    0.00    0.00    0.00    0.00    0.00   98.30
12:58:20 PM    2    0.20    0.00    0.40    0.00    0.00    0.00    0.00    0.00    0.00   99.40
12:58:20 PM    3    0.91    0.00    0.20    0.00    0.00    0.00    0.00    0.00    0.00   98.88
12:58:20 PM    4    0.00    0.00    0.10    0.00    0.00    0.00    0.00    0.00    0.00   99.90
12:58:20 PM    5    0.00    0.00    0.00    0.00    0.00    0.00    0.00    0.00    0.00  100.00
12:58:20 PM    6    0.10    0.00    0.00    0.00    0.00    0.00    0.00    0.00    0.00   99.90
12:58:20 PM    7    0.10    0.00    0.40    0.00    0.00    0.00    0.00    0.00    0.00   99.50
*/