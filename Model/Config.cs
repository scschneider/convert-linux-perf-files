using System;
using System.Text.RegularExpressions;
using ConvertLinuxPerfFiles.Utility;

namespace ConvertLinuxPerfFiles.Model
{
    class Config
    {
        // class constructor
        public Config()
        {
            SetConfigVariables();
            SetTimeZone();
        }

        // class properties
        public string MachineName { get; private set; }
        public int TimeZone { get; private set; }
        public bool ImportIoStat { get; private set; }
        public bool ImportMpStat { get; private set; }
        public bool ImportMemFree { get; private set; }
        public bool ImportMemSwap { get; private set; }
        public bool ImportNetStats { get; private set; }
        public bool ImportPidStat { get; private set; }
        public string[] PidStatFilter { get; private set; }

        // class functions
        private void SetConfigVariables()
        {
            TypeConversion typeConversion = new TypeConversion();
            // need to read config file
            FileUtility fileUtility = new FileUtility();
            //FileHelper fileHelper = new FileHelper("pssdiag.conf");
            //List<string> configFileContents = fileHelper.ReadFileByLine();
            // delimeter for parameter lines "parameter = value"
            char parameterDelimeter = '=';
            // delimeter for pidstat filter value "sqlservr,sqlcmd"
            char pidStatFilterDelimeter = ',';

            // itterate through the config file line by line.        
            foreach (string line in fileUtility.ReadFileByLine("pssdiag.conf"))
            {
                string[] splitValue = { };
                bool parameterValueBool = false;
                string parameterValueString;

                // checks to see if the line contains an "=" and does not begin with "#", which allows to comment out lines in the text file.
                if (line.Contains(parameterDelimeter.ToString()) && !line.StartsWith("#"))
                {
                    // take the value of the line and split it. we can then compare the values on each side of the "="
                    splitValue = line.Split(parameterDelimeter);
                    // the configuration file allows for values of true/false and yes/no. this will convert those to bool values.
                    string parameterValue = splitValue[1].ToLower();
                    parameterValueBool = typeConversion.ConvertTypeToBool(parameterValue);
                    // get parameter name, converts to lowercase for comparing strings and trims white space.
                    string parameter = splitValue[0].ToLower().Trim();
                    // check parameter name and when it matches, set the value of the parameter.
                    switch (parameter)
                    {
                        case "machine_name":
                            parameterValueString = splitValue[1];
                            MachineName = parameterValueString;
                            break;
                        case "import_iostat":
                            ImportIoStat = parameterValueBool;
                            break;
                        case "import_mpstat":
                            ImportMpStat = parameterValueBool;
                            break;
                        case "import_memfree":
                            ImportMemFree = parameterValueBool;
                            break;
                        case "import_memswap":
                            ImportMemSwap = parameterValueBool;
                            break;
                        case "import_network_stats":
                            ImportNetStats = parameterValueBool;
                            break;
                        case "import_pidstat":
                            ImportPidStat = parameterValueBool;
                            break;
                        case "import_pidstat_filter":
                            // since pidstat_filter accepts comma separated, dynamic values, we need to remove spaces, capture this and turn it into an array.
                            string spacePattern = "\\s+";
                            string spaceReplacement = "";
                            Regex rgx = new Regex(spacePattern);
                            parameterValueString = rgx.Replace(splitValue[1].ToLower(), spaceReplacement);
                            string[] pidStatFilerSplitValue = parameterValueString.Split(pidStatFilterDelimeter);

                            PidStatFilter = pidStatFilerSplitValue;
                            break;
                        default:
                            break;
                    }
                }
            };
        }

        // gets and sets timezone
        private void SetTimeZone()
        {
            FileUtility fileUtility = new FileUtility();
            int tz = Convert.ToInt16(fileUtility.ReadFileByLine("*timezone.out")[0].Substring(0, 3));

            TimeZone = tz;
        }
    }
}