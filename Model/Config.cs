using System.Collections.Generic;
using System.Text.RegularExpressions;
using ConvertLinuxPerfFiles.Model;

namespace ConvertLinuxPerfFiles.Model
{
    class Config
    {
        // class variables
        public string MachineName { get; private set; }
        public string TimeZone { get; private set; }
        public bool ImportIoStat { get; private set; }
        public bool ImportMpStat { get; private set; }
        public bool ImportMemFree { get; private set; }
        public bool ImportMemSwap { get; private set; }
        public bool ImportNetStats { get; private set; }
        public bool ImportPidStat { get; private set; }
        public string[] PidStatFilter { get; private set; }

        // class constructor
        public Config()
        {
            SetConfigVariables();
            SetTimeZone();
        }

        // class private set functions
        private void SetMachineName(string value)
        {
            MachineName = value;
        }
        private void SetImportIoStat(bool value)
        {
            ImportIoStat = value;
        }
        private void SetImportMpStat(bool value)
        {
            ImportMpStat = value;
        }
        private void SetImportMemFree(bool value)
        {
            ImportMemFree = value;
        }
        private void SetImportMemSwap(bool value)
        {
            ImportMemSwap = value;
        }
        private void SetImportNetStats(bool value)
        {
            ImportNetStats = value;
        }
        private void SetImportPidStat(bool value)
        {
            ImportPidStat = value;
        }
        private void SetPidStatFilter(string[] value)
        {
            PidStatFilter = value;
        }

        // class functions
        private void SetConfigVariables()
        {
            TypeConversionHelper typeConversionHelper = new TypeConversionHelper();
            // need to read config file
            FileReader fileReader = new FileReader("pssdiag.conf");
            //FileHelper fileHelper = new FileHelper("pssdiag.conf");
            //List<string> configFileContents = fileHelper.ReadFileByLine();
            // delimeter for parameter lines "parameter = value"
            char parameterDelimeter = '=';
            // delimeter for pidstat filter value "sqlservr,sqlcmd"
            char pidStatFilterDelimeter = ',';

            // itterate through the config file line by line.        
            foreach (string line in fileReader.Read())
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
                    parameterValueBool = typeConversionHelper.ConvertTypeToBool(parameterValue);
                    // get parameter name, converts to lowercase for comparing strings and trims white space.
                    string parameter = splitValue[0].ToLower().Trim();
                    // check parameter name and when it matches, set the value of the parameter.
                    switch (parameter)
                    {
                        case "machine_name":
                            parameterValueString = splitValue[1];
                            SetMachineName(parameterValueString);
                            break;
                        case "import_iostat":
                            SetImportIoStat(parameterValueBool);
                            break;
                        case "import_mpstat":
                            SetImportMpStat(parameterValueBool);
                            break;
                        case "import_memfree":
                            SetImportMemFree(parameterValueBool);
                            break;
                        case "import_memswap":
                            SetImportMemSwap(parameterValueBool);
                            break;
                        case "import_network_stats":
                            SetImportNetStats(parameterValueBool);
                            break;
                        case "import_pidstat":
                            SetImportPidStat(parameterValueBool);
                            break;
                        case "import_pidstat_filter":
                            // since pidstat_filter accepts comma separated, dynamic values, we need to remove spaces, capture this and turn it into an array.
                            string spacePattern = "\\s+";
                            string spaceReplacement = "";
                            Regex rgx = new Regex(spacePattern);
                            parameterValueString = rgx.Replace(splitValue[1].ToLower(), spaceReplacement);
                            string[] pidStatFilerSplitValue = parameterValueString.Split(pidStatFilterDelimeter);

                            SetPidStatFilter(pidStatFilerSplitValue);
                            break;
                        default:
                            break;
                    }// END switch 
                }// END if line
            };// END foreach line
        }// END setConfigVariables

        // gets and sets timezone
        private void SetTimeZone()
        {
            FileReader fileReader = new FileReader("*timezone.out");
            string tz = fileReader.Read()[0].Substring(0, 3);

            TimeZone = tz;
        }// END setTimeZone
    }
}