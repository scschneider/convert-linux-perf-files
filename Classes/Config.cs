using System.Collections.Generic;

class Config
{
    private string timeZone;
    private bool importIoStat;
    private bool importMpStat;
    private bool importMemFree;
    private bool importMemSwap;
    private bool importPidStat;
    private string[] pidStatFilter;

// class constructors    
    public Config()
    {
        setConfigVariables();
        setTimeZone();
    }

// class private set functions
    private void setImportIoStat(bool value)
    {
        importIoStat = value;
    }
    private void setImportMpStat(bool value)
    {
        importMpStat = value;
    }
    private void setImportMemFree(bool value)
    {
        importMemFree = value;
    }
    private void setImportMemSwap(bool value)
    {
        importMemSwap = value;
    }
    private void setImportPidStat(bool value)
    {
        importPidStat = value;
    }
    private void setPidStatFilter(string[] value)
    {
        pidStatFilter = value;
    }

// class public get functions
    public bool getImportIoStat()
    {
        return importIoStat;
    }
    public bool getImportMpStat()
    {
        return importMpStat;
    }
    public bool getImportMemFree()
    {
        return importMemFree;
    }
    public bool getImportMemSwap()
    {
        return importMemSwap;
    }
    public bool getImportPidStat()
    {
        return importPidStat;   
    }
    public string[] getPidStatFilter()
    {
        return pidStatFilter;
    }
    public string getTimeZone()
    {
        return timeZone;
    }

// class functions
    private void setConfigVariables()
    {
        // create the list that contains the lines from the config file.
        TypeConversionHelper typeConversionHelper = new TypeConversionHelper();
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
                    case "import_pidstat":
                        setImportPidStat(parameterValueBool);
                        break;
                    case "import_pidstat_filter":
                        // since pidstat_filter accepts comma separated values, we need to capture this and turn it into an array.
                        parameterValueString = splitValue[1].ToLower();
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
        
        timeZone = tz;
    }// END setTimeZone
}
