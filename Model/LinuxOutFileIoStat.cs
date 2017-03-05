using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ConvertLinuxPerfFiles.Utility;

namespace ConvertLinuxPerfFiles.Model
{
    class LinuxOutFileIoStat : LinuxOutFile
    {
        // class constructor
        public LinuxOutFileIoStat(string ioStatFileName) :
            base(ioStatFileName)
        {
            FileContents = GetIoStatFileContents();
            Devices = GetIoStatDevices();
            Header = GetIoStatHeader();
            Metrics = GetIoStatMetrics();
        }
        // class properties
        private List<string> Devices { get; set; }
        
        // class methods
        // Reads file contents
        private List<string> GetIoStatFileContents()
        {
            return new FileUtility().ReadFileByLine(FileName);
        }

        // since the devices are listed within the block, we need to get a list of the for generating the header
        private List<string> GetIoStatDevices()
        {
            int startingLine = 4;
            int deviceColumnNumber = 0;

            return new LinuxOutFileHelper().GetDevices(startingLine, FileContents, deviceColumnNumber);
        }

        // generates the header that gets written to the TSV file
        private string GetIoStatHeader()
        {
            // creating the outheader object and passing in variables on where to start parsing specific strings
            OutHeader outHeader = new OutHeader()
            {
                StartingColumn = 1,
                StartingRow = 3,
                FileContents = FileContents,
                Devices = Devices,
                ObjectName = "Logicaldisk"
            };

            return new LinuxOutFileHelper().GetHeader(outHeader);
        }

        // generates the metrics that get written to the tsv file
        private List<string> GetIoStatMetrics()
        {
            List<string> metrics = new List<string>();

            string emptyLinePattern = "^\\s*$";
            string splitPattern = "\\s+";

            Regex rgxEmptyLine = new Regex(emptyLinePattern);
            Regex rgxSplitLine = new Regex(splitPattern);

            int deviceCount = Devices.Count;

            // itterate through each line in filecontents
            for (int i = 0; i < FileContents.Count; i++)
            {
                DateTime timeStamp = new DateTime();
                StringBuilder thisMetricSample = new StringBuilder();

                // this file is in a block format and we use empty lines to determin when to start parsing the next metric
                if (rgxEmptyLine.IsMatch(FileContents[i]) && i < FileContents.Count - 1)
                {
                    string timeStampFormatted;
                    timeStamp = DateTime.Parse(FileContents[(i + 1)]);
                    timeStampFormatted = new DateTimeUtility().DateTime24HourFormat(timeStamp);
                    thisMetricSample.Append('"' + timeStampFormatted + '"' + "\t");

                    // looping through the logical disk devices 
                    for (int x = (i + 3); x < i + deviceCount; x++)
                    {
                        // splitting the contents of the current line to grab the metrics
                        string[] thisLineContents = rgxSplitLine.Split(FileContents[x]);

                        // looping through the split line contents and start at column 1
                        for (int z = 1; z < thisLineContents.Length; z++)
                        {
                            thisMetricSample.Append('"' + thisLineContents[z] + '"' + "\t");
                        }
                    }
                    metrics.Add(thisMetricSample.ToString());
                }
            }

            return metrics;
        }
    }
}

/* EXAMPLE of IoStat file
Linux 3.10.0-327.28.3.el7.x86_64 (dl380g802-v02) 	01/11/2017 	_x86_64_	(8 CPU)

01/11/2017 12:58:10 PM
Device:         rrqm/s   wrqm/s     r/s     w/s    rkB/s    wkB/s avgrq-sz avgqu-sz   await r_await w_await  svctm  %util
fd0               0.00     0.00    0.00    0.00     0.00     0.00     0.00     0.00    0.00    0.00    0.00   0.00   0.00
sda               0.00     0.10    3.50    1.20   242.80     6.80   106.21     0.01    2.13    2.49    1.08   0.66   0.31
sdb               0.00     0.00    0.00    0.00     0.00     0.00     0.00     0.00    0.00    0.00    0.00   0.00   0.00
sdc               0.00     0.00    0.00    0.00     0.00     0.00     0.00     0.00    0.00    0.00    0.00   0.00   0.00
dm-0              0.00     0.00    3.40    0.90   241.20     6.80   115.35     0.01    2.35    2.56    1.56   0.74   0.32
dm-1              0.00     0.00    0.00    0.00     0.00     0.00     0.00     0.00    0.00    0.00    0.00   0.00   0.00

01/11/2017 12:58:20 PM
Device:         rrqm/s   wrqm/s     r/s     w/s    rkB/s    wkB/s avgrq-sz avgqu-sz   await r_await w_await  svctm  %util
fd0               0.00     0.00    0.00    0.00     0.00     0.00     0.00     0.00    0.00    0.00    0.00   0.00   0.00
sda               0.00     0.30    0.00    3.30     0.00   160.80    97.45     0.01    2.18    0.00    2.18   0.18   0.06
sdb               0.00     0.00    0.00    0.00     0.00     0.00     0.00     0.00    0.00    0.00    0.00   0.00   0.00
sdc               0.00     0.00    0.00    0.00     0.00     0.00     0.00     0.00    0.00    0.00    0.00   0.00   0.00
dm-0              0.00     0.00    0.00    3.50     0.00   160.00    91.43     0.01    2.31    0.00    2.31   0.14   0.05
dm-1              0.00     0.00    0.00    0.00     0.00     0.00     0.00     0.00    0.00    0.00    0.00   0.00   0.00
 */
