using System;
//using Config;


namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Config myNewCfg = new Config();
            string tz = myNewCfg.getTimeZone();
            bool iostat = myNewCfg.getImportIoStat();
            bool mpStat = myNewCfg.getImportMpStat();
            bool memFree = myNewCfg.getImportMemFree();
            bool memSwap = myNewCfg.getImportMemSwap();
            bool pidStat = myNewCfg.getImportPidStat();
            string[] pidFilter = myNewCfg.getPidStatFilter();
            
            
            Console.WriteLine("Hello World!");
        }
    }
}
