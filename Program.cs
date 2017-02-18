using System;
//using Config;


namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Config myNewCfg = new Config();
            string timeZone = myNewCfg.TimeZone;
            bool ioStat = myNewCfg.ImportIoStat;
            bool mpStat = myNewCfg.ImportMpStat;
            bool memFree = myNewCfg.ImportMemFree;
            bool memSwap = myNewCfg.ImportMemSwap;
            bool netStat = myNewCfg.ImportNetStats;
            bool pidStat = myNewCfg.ImportPidStat;
            string[] pidFilter = myNewCfg.PidStatFilter;

            //test
            
            if (ioStat) {ConvertIoStatToTsv();}
            if (mpStat) {ConvertMpStatToTsv();}
            if (memFree) {ConvertMemFreeToTsv();}
            if (memSwap) {ConvertMemSwapToTsv();}
            if (netStat) {ConvertNetStatToTsv();}
            if (pidStat) {ConvertPidStatToTsv(pidFilter);}

            //Type1 t1 = new Type1("*iostat.out");

            
            Console.WriteLine("Hello World!");
        
        }// END Main
        public static void ConvertIoStatToTsv()
        {
            string outIoStatFileName = "*iostat.out";
            IoStat ioStat = new IoStat(outIoStatFileName);
            Console.WriteLine(ioStat.Header);
            //Type1 type1OutFile = new Type1(outIoStatFileName);
            


        }

        public static void ConvertMpStatToTsv()
        {
            string outMpStatFileName = "*mpstats_cpu.out";

        }

        public static void ConvertMemFreeToTsv()
        {
            string outMemFreeFileName = "*memory_free.out";

        }

        public static void ConvertMemSwapToTsv()
        {
            string outMemSwapFileName = "*memory_swap.out";

        }
        
        public static void ConvertNetStatToTsv()
        {
            string outNetStatFileName = "*network_stats.out";
        }
        public static void ConvertPidStatToTsv(string[] filter)
        {
            string outPidStatFileName = "*pidstat.out";

        }
        
    }// END Program
}// END Namespace ConsoleApplication
