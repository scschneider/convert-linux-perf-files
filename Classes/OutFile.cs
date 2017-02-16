using System.Collections.Generic;
abstract class OutFile
{
// class variables
    private string[] Header;
    private string[] Metrics;
    private string FileName;
    private List<string> FileContents;

// class constructor
    public OutFile(string fileName)
    {
        setFileName(fileName);
    }

// public set functions
    public void setFileName(string fileName)
    {
        FileName = fileName;
    }
    public void setFileContents()
    {
        FileHelper fileHelper = new FileHelper(FileName);
        FileContents = fileHelper.readFileByLine();

    }
// private set functions
    private void setHeader(string[] header)
    {
        Header = header;
    }

    private void setMetrics(string[] metrics)
    {
        Metrics = metrics;
    }
// public get functions
    public string getFileName()
    {
        return FileName;
    }

    public string[] getHeader()
    {
        return Header;
    }

    public string[] getMetrics()
    {
        return Metrics;
    }


}