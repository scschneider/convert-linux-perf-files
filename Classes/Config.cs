using System.Collections.Generic;
using System.Text.RegularExpressions;

class Config
{
// class variables
    private string MachineName;
    private string TimeZone;
    private bool ImportIoStat;
    private bool ImportMpStat;
    private bool ImportMemFree;
    private bool ImportMemSwap;
    private bool ImportNetStats;
    private bool ImportPidStat;
    private string[] PidStatFilter;

// class constructors    
    public Config()
    {
        setConfigVariables();
        setTimeZone();
    }

// class private set functions
    private void setMachineName (string value)
    {
        MachineName = value;
    }
    private void setImportIoStat(bool value)
    {
        ImportIoStat = value;
    }
    private void setImportMpStat(bool value)
    {
        ImportMpStat = value;
    }
    private void setImportMemFree(bool value)
    {
        ImportMemFree = value;
    }
    private void setImportMemSwap(bool value)
    {
        ImportMemSwap = value;
    }
    private void setImportNetStats(bool value)
    {
        ImportNetStats = value;
    }
    private void setImportPidStat(bool value)
    {
        ImportPidStat = value;
    }
    private void setPidStatFilter(string[] value)
    {
        PidStatFilter = value;
    }

// class public get functions
    public string getMachineName()
    {
        return MachineName;
    }
    public bool getImportIoStat()
    {
        return ImportIoStat;
    }
    public bool getImportMpStat()
    {
        return ImportMpStat;
    }
    public bool getImportMemFree()
    {
        return ImportMemFree;
    }
    public bool getImportMemSwap()
    {
        return ImportMemSwap;
    }
    public bool getImportNetStats()
    {
        return ImportNetStats;
    }
    public bool getImportPidStat()
    {
        return ImportPidStat;   
    }
    public string[] getPidStatFilter()
    {
        return PidStatFilter;
    }
    public string getTimeZone()
    {
        return TimeZone;
    }

// class functions
    private void setConfigVariables()
    {
        TypeConversionHelper typeConversionHelper = new TypeConversionHelper();
        // create the list that contains the lines from the config file.
        FileHelper fileHelper = new FileHelper("pssdiag.conf");
        List<string> configFileContents = fileHelper.readFileByLine();
        // delimeter for parameter lines "parameter = value"
        char parameterDelimeter = '=';
        // delimeter for pidstat filter value "sqlservr,sqlcmd"
        char pidStatFilterDelimeter = ',';
        
        // itterate through the config file line by line.        
        foreach (string line in configFileContents)
        {
            string[] splitValue = {};
            bool parameterValueBool = false;
            string parameterValueString;
            
            // checks to see if the line contains an "=" and does not begin with "#", which allows to comment out lines in the text file.
            if (line.Contains(parameterDelimeter.ToString()) && !line.StartsWith("#"))
            {
                // take the value of the line and split it. we can then compare the values on each side of the "="
                splitValue = line.Split(parameterDelimeter);
                // the configuration file allows for values of true/false and yes/no. this will convert those to bool values.
                string parameterValue = splitValue[1].ToLower();
                parameterValueBool = typeConversionHelper.convertTypeToBool(parameterValue);
                // get parameter name, converts to lowercase for comparing strings and trims white space.
                string parameter = splitValue[0].ToLower().Trim();
                // check parameter name and when it matches, set the value of the parameter.
                switch (parameter)
                {
                    case "machine_name":
                        parameterValueString = splitValue[1];
                        setMachineName(parameterValueString);
                        break;
                    case "import_iostat":
                        setImportIoStat(parameterValueBool);
                        break;
                    case "import_mpstat":
                        setImportMpStat(parameterValueBool);
                        break;
                    case "import_memfree":
                        setImportMemFree(parameterValueBool);
                        break;
                    case "import_memswap":
                        setImportMemSwap(parameterValueBool);
                        break;
                    case "import_network_stats":
                        setImportNetStats(parameterValueBool);
                        break;
                    case "import_pidstat":
                        setImportPidStat(parameterValueBool);
                        break;
                    case "import_pidstat_filter":
                        // since pidstat_filter accepts comma separated values, we need to remove spaces, capture this and turn it into an array.
                        string spacePattern = "\\s+";
                        string spaceReplacement = "";
                        Regex rgx = new Regex(spacePattern);
                        parameterValueString = rgx.Replace(splitValue[1].ToLower(),spaceReplacement);
                        string[] pidStatFilerSplitValue = parameterValueString.Split(pidStatFilterDelimeter);
                        
                        setPidStatFilter(pidStatFilerSplitValue);
                        break;
                    default: // deafult exit case.
                    break;
                }// END switch 
            }// END if line
        };// END foreach line
    }// END setConfigVariables

    // gets and sets timezone
    private void setTimeZone()
    {
        FileHelper fileHelper = new FileHelper("*timezone.out");
        List<string> configFileContents = fileHelper.readFileByLine();
        string tz = configFileContents[0].Substring(0,3);
        
        TimeZone = tz;
    }// END setTimeZone
}
