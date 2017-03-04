using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConvertLinuxPerfFiles.Model
{
    class LinuxOutFilePidStatHelper
    {
        // class constructor
        public LinuxOutFilePidStatHelper(string pidStatFileName, int timeZone, string[] pidFilter)
        {
            TimeZone = timeZone;
            FileName = pidStatFileName;
            PidFilter = pidFilter;

            SetPidStatFileContents();
            ProcessPidStatFile();
            GetUniquePids();
            GetPidStatHeader();
            GetPidStatMetrics();
        }

        // class properties
        private string FileName { get; set; }
        private List<string> FileContents { get; set; }
        private int TimeZone { get; set; }
        private string[] PidFilter { get; set; }
        //private string[] RawHeader { get; set; }
        private List<long> BlockCount = new List<long>();
        private List<Process> Processes = new List<Process>();
        private Dictionary<long, string> UniquePids;

        public string Header { get; set; }
        public List<string> Metrics { get; set; }

        // class methods
        private void SetPidStatFileContents()
        {
            FileContents = new FileReader().Read(FileName);
        }
        public void ProcessPidStatFile()
        {
            string emptyLinePattern = "^\\s*$";
            string splitPattern = "\\s+";

            Regex rgxEmptyLine = new Regex(emptyLinePattern);
            Regex rgxSplitLine = new Regex(splitPattern);

            // starting at the first line where # appears
            for (int i = 3; i <= FileContents.Count - 1;)
            {
                if (FileContents[i].StartsWith("#"))
                {
                    // add the current line position to the block count array. We will use this to spin off multiple threads for processing the pid stat file
                    BlockCount.Add(i + 1);
                    i++;
                }
                // just skip empty lines and increment to the next line
                else if (rgxEmptyLine.IsMatch(FileContents[i]))
                {
                    i++;
                }
                else
                {
                    // takes the value of the current line and splits on whitespace
                    string[] thisProcessLine = rgxSplitLine.Split(FileContents[i]);

                    // reads the line timestamp, converts it from ephoc/unix time
                    string thisTimeStamp = FromUnixTime(Convert.ToInt32(thisProcessLine[1]));
                    // reads this line's pid
                    int thisPid = Convert.ToInt32(thisProcessLine[3]);
                    // reads this lines process name
                    string thisProcessName = thisProcessLine[thisProcessLine.Length - 1];
                    // declares a new array so that we can populate this array with metrics with array.copy
                    string[] theseMetrics = new string[15];

                    // copies the metrics from the current line to theseMetrics array. We need to do this since we split the line to get other metrics.
                    Array.Copy(thisProcessLine, 4, theseMetrics, 0, 15);

                    // create a new process object and set its properties from the vairables we declared above
                    Process process = new Process()
                    {
                        TimeStamp = thisTimeStamp,
                        Pid = thisPid,
                        ProcessName = thisProcessName,
                        Metrics = theseMetrics
                    };

                    // we need to filter out what gets collected based on fthe PidFilter in the config file.
                    if (PidFilter.Contains(process.ProcessName))
                    {
                        // once we are done generating the process object, we add the object to the collection of processes
                        Processes.Add(process);
                    }

                    i++;
                }
            }
        }

        // we need to get the unique pids and order them so that we can generate the header.
        // Since each block of results in the pidstat out file can contain new or removed pids
        // from previous blocks, we need to get this information up front before processing the results
        private void GetUniquePids()
        {
            UniquePids = Processes.Select(x => new { x.Pid, x.ProcessName }).Distinct().OrderBy(Pid => Pid.Pid).ToDictionary(x => x.Pid, x => x.ProcessName);
        }
        // now that we have the unique pids, we can create the header
        private void GetPidStatHeader()
        {
            string splitPattern = "\\s+";
            Regex rgxSplitLine = new Regex(splitPattern);
            string[] rawHeader = rgxSplitLine.Split(FileContents[3]);
            StringBuilder header = new StringBuilder();
            header.Append('"' + "(PDH-TSV 4.0) (Pacific Daylight Time)(420)" + '"' + "\t");

            foreach (var i in UniquePids)
            {
                for (int j = 4; j <= (rawHeader.Count() - 2); j++)
                {

                    header.Append('"' + "\\\\MACHINENAME\\Process(" + i.Value + "#" + i.Key + ")\\" + rawHeader[j] + '"' + "\t");
                }
            }
            Header = header.ToString();
        }

        private void GetPidStatMetrics()
        {
            List<string> metrics = new List<string>();
            foreach (Process process in Processes)
            {
                StringBuilder metric = new StringBuilder();
                metric.Append('"' + process.TimeStamp.ToString() + '"' + "\t");

                foreach (var p in UniquePids)
                {
                    if (process.Pid == p.Key)
                    {
                        foreach (string m in process.Metrics)
                        {
                            metric.Append('"' + m + '"' + "\t");
                        }
                    }
                    if (process.Pid != p.Key)
                    {
                        for (int i = 0; i <= process.Metrics.Count() - 1; i++)
                        {
                            metric.Append('"' + "0.00" + '"' + "\t");
                        }
                    }
                }

                metrics.Add(metric.ToString());
            }
            Metrics = metrics;
        }
        public string FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // returns the converted time and adjusts it to the timezone of the machine the metrics were colelcted from
            return epoch.AddSeconds(unixTime).AddHours(TimeZone).ToString("MM/dd/yyyy H:mm:ss");;
        }

    }

    // Process class used when creating a new process. These get added to the Processes collection.
    class Process : Object
    {
        public long Pid { get; set; }
        public string ProcessName { get; set; }
        public string TimeStamp { get; set; }
        public string[] Metrics { get; set; }
    }
}