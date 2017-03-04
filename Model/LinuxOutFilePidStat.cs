using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ConvertLinuxPerfFiles.Utility;

namespace ConvertLinuxPerfFiles.Model
{
    class LinuxOutFilePidStat : LinuxOutFile
    {
        // class constructor
        public LinuxOutFilePidStat(string pidStatFileName, int timeZone, string[] pidFilter) :
            base(pidStatFileName, timeZone)
        {
            
            PidFilter = pidFilter;
            FileContents = GetPidStatFileContents();
            Processes = GetProcessMetrics();
            UniquePids = GetUniquePids();
            Header = GetPidStatHeader();
            Metrics = GetPidStatMetrics();
        }

        // class properties
        private string[] PidFilter { get; set; }
        private List<long> BlockCount = new List<long>();
        private List<Process> Processes = new List<Process>();
        private Dictionary<long, string> UniquePids;

        // class methods
        // Reads file contents
        private List<string> GetPidStatFileContents()
        {
            return new Utility.FileUtility().ReadFileByLine(FileName);
        }
        // Reads each line where there are metrics, creates a process object and adds it to the objects collection
        public List<Process> GetProcessMetrics()
        {
            List<Process> processes = new List<Process>();

            DateTimeUtility utility = new DateTimeUtility();

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
                    string thisTimeStamp = utility.FromUnixTime(Convert.ToInt32(thisProcessLine[1]),TimeZone);
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
                    if (PidFilter.Count() != -1 && PidFilter.Contains(process.ProcessName))
                    {
                        // once we are done generating the process object, we add the object to the collection of processes
                        processes.Add(process);
                    }

                    if (PidFilter[0] == "" || PidFilter[0] == "false")
                    {
                        // once we are done generating the process object, we add the object to the collection of processes
                        processes.Add(process);
                    }


                    i++;
                }
            }

            return processes;
        }

        // we need to get the unique pids and order them so that we can generate the header.
        // Since each block of results in the pidstat out file can contain new or removed pids
        // from previous blocks, we need to get this information up front before processing the results
        private Dictionary<long,string> GetUniquePids()
        {
            return Processes.Select(x => new { x.Pid, x.ProcessName }).Distinct().OrderBy(Pid => Pid.Pid).ToDictionary(x => x.Pid, x => x.ProcessName);
        }
        // now that we have the unique pids, we can create the header
        private string GetPidStatHeader()
        {
            string splitPattern = "\\s+";
            Regex rgxSplitLine = new Regex(splitPattern);
            // splitting the contents of the line that has the raw header data
            string[] rawHeader = rgxSplitLine.Split(FileContents[3]);
            StringBuilder header = new StringBuilder();
            header.Append('"' + "(PDH-TSV 4.0) (Pacific Daylight Time)(420)" + '"' + "\t");

            foreach (var i in UniquePids)
            {
                // loop to read the header names that we will us in Perfmon. we start at the first 
                // occurence of a header name and go unitl the hit the column before the 
                // last one since the last column contains the process name.
                for (int j = 4; j <= (rawHeader.Count() - 2); j++)
                {
                    // this generated the header that will get used for the TSV file. example output "\\MACHINENAME\Process(sqlservr#19155)\%usr"
                    header.Append('"' + "\\\\MACHINENAME\\Process(" + i.Value + "#" + i.Key + ")\\" + rawHeader[j] + '"' + "\t");
                }
            }
            return header.ToString();
        }

        // generating the useful data for the metrics section of the TSV file
        private List<string> GetPidStatMetrics()
        {
            // create the collection that each generated line will be placed in
            List<string> metrics = new List<string>();
            // loop through every process in processes
            foreach (Process process in Processes)
            {
                // create the object that each metric will get appended to
                StringBuilder metric = new StringBuilder();
                // each line starts with a timestamp
                metric.Append('"' + process.TimeStamp.ToString() + '"' + "\t");

                // looping through each unique pid
                foreach (var p in UniquePids)
                {
                    // if the unique pid matches the pid from the current process, this will write thatpid's data, collected from the out file
                    if (process.Pid == p.Key)
                    {
                        foreach (string m in process.Metrics)
                        {
                            metric.Append('"' + m + '"' + "\t");
                        }
                    }

                    // if the unique pid does not match the pid from the current process, we write 0.00
                    if (process.Pid != p.Key)
                    {
                        for (int i = 0; i <= process.Metrics.Count() - 1; i++)
                        {
                            metric.Append('"' + "0.00" + '"' + "\t");
                        }
                    }
                }

                // adding the data to the metrics object
                metrics.Add(metric.ToString());
            }

            return metrics;
        }
    }

    // Process class used when creating a new process. These get added to the Processes collection.
    class Process
    {
        public long Pid { get; set; }
        public string ProcessName { get; set; }
        public string TimeStamp { get; set; }
        public string[] Metrics { get; set; }
    }
}