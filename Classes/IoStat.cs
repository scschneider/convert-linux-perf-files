using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

class IoStat
{
    private string FileName;
    private string Header;
    private List<string> Devices;
    private List<string> Metrics;
    private List<string> FileContents;

    // class constructor
    public IoStat(string fileName)
    {
        setFileName(fileName);
        setFileContents();
        setDevices();
        setHeader();
        setMetrics();
    }

    // class private set functions
    private void setFileName(string fileName)
    {
        FileName = fileName;
        //MachineName = machineName;
    }
    private void setFileContents()
    {
        FileHelper fileHelper = new FileHelper(FileName);
        FileContents = fileHelper.readFileByLine();
    }
    private void setHeader()
    {
        Header = generateHeader();
    }
    private void setDevices()
    {
        Devices = generateDevices();
    }

    private void setMetrics()
    {
        Metrics = generateMetrics();
    }

    //class public get functions
    public string getFileName()
    {
        return FileName;
    }
    public string getHeader()
    {
        return Header;
    }
    public List<string> getFileContents()
    {
        return FileContents;
    }
    public List<string> getMetrics()
    {
        return Metrics;
    }
    public List<string> getDevices()
    {
        return Devices;
    }


    // class functions
    private List<string> generateDevices()
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
            else
            {
                block++;
            }
        }

        return devices;
    }
    private string generateHeader()
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
        }

        return header.ToString();
    }
    private List<string> generateMetrics()
    {
        List<string> metrics = new List<string>();

        return metrics;
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
