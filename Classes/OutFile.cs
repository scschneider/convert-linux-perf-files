class OutFile
{
// class variables
    private string[] Header;
    private string[] Metrics;
    private string FileName;

// class constructor
    public OutFile(string fileName)
    {
        setFileName(fileName);
    }

// private set functions
    private void setFileName(string fileName)
    {
        FileName = fileName;
    }

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