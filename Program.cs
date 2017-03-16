/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using ConvertLinuxPerfFiles.Logging;
using ConvertLinuxPerfFiles.Model;
using ConvertLinuxPerfFiles.Utility;

namespace ConvertLinuxPerfFiles
{
    public static class Globals
    {
        public static Log log = new Log();
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                string workingDirectory = args[0];
                Directory.SetCurrentDirectory(workingDirectory);
            }
            catch (System.Exception e)
            {
                string message = "Expects 1 argument: Please specify the directory where the .out files are.";

                System.Console.WriteLine(message);
                System.Console.WriteLine(e.Message);

                System.Environment.Exit(0);
            }

            Console.WriteLine(DateTime.Now.ToString());

            // progress.WriteTitle("Converting files located in: " + Directory.GetCurrentDirectory());
            LoggingConfig.LogFileName = "Import.log";
            Config config = new Config();

            List<Thread> threads = new List<Thread>();

            if (ConfigValues.ImportIoStat)
            {
                Thread thread = new Thread(new ThreadStart(ImportIo));
                thread.Name = "Import IO Stat";
                thread.Start();
                Console.WriteLine("Started " + DateTime.Now + ": " + thread.Name);

                threads.Add(thread);
            }

            if (ConfigValues.ImportMemFree)
            {
                Thread thread = new Thread(new ThreadStart(ImportMemFree));
                thread.Name = "Import Memory Free";
                thread.Start();
                Console.WriteLine("Started " + DateTime.Now + ": " + thread.Name);

                threads.Add(thread);
            }
            if (ConfigValues.ImportMemSwap)
            {
                Thread thread = new Thread(new ThreadStart(ImportMemSwap));
                thread.Name = "Import Memory Swap";
                thread.Start();
                Console.WriteLine("Started " + DateTime.Now + ": " + thread.Name);

                threads.Add(thread);
            }
            if (ConfigValues.ImportMpStat)
            {
                Thread thread = new Thread(new ThreadStart(ImportMp));
                thread.Name = "Import MP Stat CPU";
                thread.Start();
                Console.WriteLine("Started " + DateTime.Now + ": " + thread.Name);

                threads.Add(thread);
            }
            if (ConfigValues.ImportNetStats)
            {
                Thread thread = new Thread(new ThreadStart(ImportNet));
                thread.Name = "Import Network";
                thread.Start();
                Console.WriteLine("Started " + DateTime.Now + ": " + thread.Name);

                threads.Add(thread);
            }
            if (ConfigValues.ImportPidStat)
            {
                Thread thread = new Thread(new ThreadStart(ImportPid));
                thread.Name = "Import PID Stat";
                thread.Start();
                Console.WriteLine("Started " + DateTime.Now + ": " + thread.Name);

                threads.Add(thread);
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
                Console.WriteLine("Completed " + DateTime.Now + ": " + thread.Name);
            }

            if (ConfigValues.ImportCombine) { ImportCombine(); }
            Console.WriteLine(DateTime.Now.ToString());
        }

        private static void ImportIo()
        {
            string ioStatFileName = "*_iostat.out";
            LinuxOutFileIoStat ioStat = new LinuxOutFileIoStat(ioStatFileName);
            new FileUtility().WriteTsvFileByLine(ioStatFileName, ioStat.Header, ioStat.Metrics);
        }

        private static void ImportMemFree()
        {
            string memFreeFileName = "*_memory_free.out";
            LinuxOutFileMemFree memFree = new LinuxOutFileMemFree(memFreeFileName);
            new FileUtility().WriteTsvFileByLine(memFreeFileName, memFree.Header, memFree.Metrics);
        }
        private static void ImportMemSwap()
        {
            string memSwapFileName = "*_memory_swap.out";
            LinuxOutFileMemSwap memSwap = new LinuxOutFileMemSwap(memSwapFileName);
            new FileUtility().WriteTsvFileByLine(memSwapFileName, memSwap.Header, memSwap.Metrics);
        }

        private static void ImportMp()
        {
            string mpStatFileName = "*_mpstats_cpu.out";
            LinuxOutFileMpStat mpStat = new LinuxOutFileMpStat(mpStatFileName);
            new FileUtility().WriteTsvFileByLine(mpStatFileName, mpStat.Header, mpStat.Metrics);
        }

        private static void ImportNet()
        {
            string networkFileName = "*_network_stats.out";
            LinuxOutFileNetwork network = new LinuxOutFileNetwork(networkFileName);
            new FileUtility().WriteTsvFileByLine(networkFileName, network.Header, network.Metrics);
        }

        private static void ImportPid()
        {
            string pidStatFileName = "*_process_pidstat.out";
            LinuxOutFilePidStat pidStat = new LinuxOutFilePidStat(pidStatFileName);
            new FileUtility().WriteTsvFileByLine(pidStatFileName, pidStat.Header, pidStat.Metrics);
        }

        private static void ImportCombine()
        {
            ImportCombine ic = new ImportCombine();
            ic.CreateOutputDirectory();
            ic.RelogConvertToBlg();
        }
    }
}
