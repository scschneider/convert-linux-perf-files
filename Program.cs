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
            bool netStat = myNewCfg.getImportNetStats();
            bool pidStat = myNewCfg.getImportPidStat();
            string[] pidFilter = myNewCfg.getPidStatFilter();

            
            
            if (ioStat) {convertIoStatToTsv();}
            if (mpStat) {convertMpStatToTsv();}
            if (memFree) {convertMemFreeToTsv();}
            if (memSwap) {convertMemSwapToTsv();}
            if (netStat) {convertNetStatToTsv();}
            if (pidStat) {convertPidStatToTsv(pidFilter);}

            
            Console.WriteLine("Hello World!");
        
        }// END Main
        public static void convertIoStatToTsv()
        {
            string outIoStatFileName = "*iostat.out";
            Type1 type1OutFile = new Type1(outIoStatFileName);
            


        }

        public static void convertMpStatToTsv()
        {
            string outMpStatFileName = "*mpstats_cpu.out";

        }

        public static void convertMemFreeToTsv()
        {
            string outMemFreeFileName = "*memory_free.out";

        }

        public static void convertMemSwapToTsv()
        {
            string outMemSwapFileName = "*memory_swap.out";

        }
        
        public static void convertNetStatToTsv()
        {
            string outNetStatFileName = "*network_stats.out";
        }
        public static void convertPidStatToTsv(string[] filter)
        {
            string outPidStatFileName = "*pidstat.out";

        }
        
    }// END Program
}// END Namespace ConsoleApplication
