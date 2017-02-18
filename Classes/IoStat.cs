using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

class IoStat
{
    public string FileName {get; private set;}
    public string Header {get; private set;}
    public List<string> Devices {get; private set;}
    public List<string> Metrics {get; private set;}
    public List<string> FileContents {get; private set;}

    // class constructor
    public IoStat(string fileName)
    {
        SetFileName(fileName);
        SetFileContents();
        SetDevices();
        SetHeader();
        SetMetrics();
    }

    // class private set functions
    private void SetFileName(string fileName)
    {
        FileName = fileName;
    }
    private void SetFileContents()
    {
        FileHelper fileHelper = new FileHelper(FileName);
        FileContents = fileHelper.ReadFileByLine();
    }
    private void SetHeader()
    {
        Header = GenerateHeader();
    }
    private void SetDevices()
    {
        Devices = GenerateDevices();
    }

    private void SetMetrics()
    {
        Metrics = GenerateMetrics();
    }

// class functions

// this method gets the devices that the metrics are recorded for. Since they are in column format, we need to get them so that we can format them for TSV format
    private List<string> GenerateDevices()
    {
        string emptyLinePattern = "^\\s*$";
        string splitPattern = "\\s+";

        Regex rgxEmptyLine = new Regex(emptyLinePattern);
        Regex rgxSplitLine = new Regex(splitPattern);

        int blockCount = 1;
        int block = 0;
        int lineNumber = 4;

        List<string> devices = new List<string>();

        while (block < blockCount)
        {
            if (!rgxEmptyLine.IsMatch(FileContents[lineNumber]))
            {
                string[] thisLineValues = rgxSplitLine.Split(FileContents[lineNumber]);
                devices.Add(thisLineValues[0]);
                lineNumber++;
            }
            else {
                block++;
            }// END if rgxEmptyLine
        }// END while block

        return devices;
    }// END GenerateDevices

    // since the headers are recorded for every for every block, we need to collect these and combine them with devices to generate the header line for the tsv file
    private string GenerateHeader()
    {
        string splitPattern = "\\s+";
        Regex rgx = new Regex(splitPattern);
        string[] outHeader = rgx.Split(FileContents[3]);
        StringBuilder header = new StringBuilder();
        header.Append("(PDH-TSV 4.0) (Pacific Daylight Time)(420)\t");

        foreach (string device in Devices)
        {
            for (int i = 1; i < outHeader.Length; i++)
            {
                header.Append("\\\\MACHINENAME\\LogicalDisk(" + device + ")\\" + outHeader[i] + "\t");
            }
        }// END foreach device

        return header.ToString();
    }// END GenerateHeader
    private List<string> GenerateMetrics()
    {
        List<string> metrics = new List<string>();

        return metrics;
    }// END GenerateMetrics
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
