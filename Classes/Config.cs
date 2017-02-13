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

        // reads and sets timezone
        public Config()
        {
            setConfigVariables();
            setTimeZone();
        }
        private void setTimeZone()
        {
            List<string> configFileContents = readConfigFile("*timezone.out");
            string tz = configFileContents[0].Substring(0,3);
            
            timeZone = tz;
        }

        // reads and sets import iostats
        private void setImportIoStat()
        {

        }

        // reads and sets import mpstats
        private void setImportMpStat()
        {

        }

        // reads and sets import memfree
        private void setImportMemFree()
        {

        }

        // reads and set import memswap
        private void setImportMemSwap()
        {

        }

        // reads and sets import pidstat
        private void setImportPidStat()
        {

        }

        // reads and set pid stat filter. used to filter which pid stats to pull
        private void setPidStatFilter()
        {

        }

        public string getTimeZone()
        {
            return timeZone;
        }
        private void setConfigVariables()
        {
            // create the list that contains the lines from the config file.
            List<string> configFileContents = getFileContents("pssdiag.conf");

            // itterate through the config file line by line.
            foreach (string line in configFileContents)
            {
                // delimeter for parameter lines "parameter = value"
                char parameterDelimeter = '=';
                // delimeter for pidstat filter value "sqlservr,sqlcmd"
                char pidStatFilterDelimeter = ',';
                string[] splitValue = {};
                bool parameterValueBool;
                string parameterValueString;
                
                // Sets the default parameter value to false.
                parameterValueBool = false;

                // checks to see if the line contains an "=" and does not begin with "#", which allows to comment out lines in the text file.
                if (line.Contains(parameterDelimeter.ToString()) && !line.StartsWith("#"))
                {
                    // take the value of the line and split it. we can then compare the values on each side of the "="
                    splitValue = line.Split(parameterDelimeter);
                    // the configuration file allows for values of true/false and yes/no. this will convert those to bool values.
                    parameterValueBool = convertParameterValueToBool(splitValue[1].ToLower());
                    // get parameter name, converts to lowercase for comparing strings and trims white space.
                    string parameter = splitValue[0].ToLower().Trim();
                    // check parameter name and when it matches, set the value of the parameter.
                    switch (parameter)
                    {
                        case "import_iostat":
                            importIoStat = parameterValueBool;
                            break;
                        case "import_mpstat":
                            importMpStat = parameterValueBool;
                            break;
                        case "import_memfree":
                            importMemFree = parameterValueBool;
                            break;
                        case "import_memswap":
                            importMemSwap = parameterValueBool;
                            break;
                        case "import_pidstat":
                            importPidStat = parameterValueBool;
                            break;
                        case "import_pidstat_filter":
                            // since pidstat_filter accepts comma separated values, we need to capture this and turn it into an array.
                            parameterValueString = splitValue[1].ToLower();
                            pidStatFilter = parameterValueString.Split(pidStatFilterDelimeter);
                            break;
                        default: // deafult exit case.
                        break;
                    }// END switch 
                }// END if line
            };// END foreach line
        }// END setConfigVariables
        private List<string> getFileContents(string fileName)
        {
           List<string> fileContents = new List<string>();
           FileHelper fileHelper = new FileHelper(fileName);
           fileContents = fileHelper.readFileByLine();

           return fileContents; // configFileArray;
        }

    private bool convertParameterValueToBool(string parameterValue)
    {
        bool pv;
        switch (parameterValue)
        {
            case "1":
            case "true":
            case "yes":
                pv = true;
                break;
            case "0":
            case "false":
            case "no":
                pv = false;
                break;
            default:
                pv = false;
                break;
        }
        
        return pv;
    }

    }
