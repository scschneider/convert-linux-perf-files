using System;
//using Config;


namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Config myNewCfg = new Config();
            string timeZone = myNewCfg.getTimeZone();
            bool ioStat = myNewCfg.getImportIoStat();
            bool mpStat = myNewCfg.getImportMpStat();
            bool memFree = myNewCfg.getImportMemFree();
            bool memSwap = myNewCfg.getImportMemSwap();
            bool pidStat = myNewCfg.getImportPidStat();
            string[] pidFilter = myNewCfg.getPidStatFilter();

            if(ioStat)
            {
                Type1 type1 = new Type1("asdf");
            }

            
            Console.WriteLine("Hello World!");
        }
    }
}
