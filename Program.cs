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
            
            Console.WriteLine("Hello World!");
        }
    }
}
