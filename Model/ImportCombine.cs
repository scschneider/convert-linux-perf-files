/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ConvertLinuxPerfFiles.Utility;

namespace ConvertLinuxPerfFiles.Model
{
    class ImportCombine
    {
        // private void CleanUp()
        // {
        //     Progress progress = new Progress();

        //     try
        //     {
        //         IEnumerable<string> listOfTsv = Directory.EnumerateFiles(".\\", "*.tsv", SearchOption.AllDirectories);

        //         foreach (string file in listOfTsv)
        //         {
        //             Directory.Delete(file);
        //             Globals.log.WriteLog("Removing: " + file, "ImportCombine:CleanUp", "[Info]");
        //         }
        //     }
        //     catch (Exception e)
        //     {
        //         Globals.log.WriteLog("Nothing to clean up (TSV)", "ImportCombine:CleanUp", "[Info]");
        //     }

        //     try
        //     {
        //         IEnumerable<string> listOfBlg = Directory.EnumerateFiles(".\\", "*.blg", SearchOption.AllDirectories);

        //         foreach (string file in listOfBlg)
        //         {
        //             Directory.Delete(file);
        //             Globals.log.WriteLog("Removing: " + file, "ImportCombine:CleanUp", "[Info]");
        //         }
        //     }
        //     catch (Exception e)
        //     {
        //         Globals.log.WriteLog("Nothing to clean up (BLG)", "ImportCombine:CleanUp", "[Info]");
        //     }
        // }
        public void CreateOutputDirectory()
        {
            if (!Directory.Exists("Individual"))
            {
                Directory.CreateDirectory("Individual");
            }
        }
        public void RelogConvertToBlg()
        {
            List<Process> relogCollection = new List<Process>();

            try
            {
                IEnumerable<string> listOfTsv = Directory.EnumerateFiles(".\\", "*.tsv");

                foreach (string file in listOfTsv)
                {
                    string blgFileName = file.Replace("tsv", "blg");
                    string processCommand = "relog.exe";
                    string processArgs = file + " -o .\\Individual\\" + blgFileName + " -y";

                    relogCollection.Add(new Utility.ProcessUtility().StartProcess(processCommand, processArgs));

                    Globals.log.WriteLog("relog.exe" + file.ToString() + " -o .\\Individual\\" + blgFileName + " -y", "ImportCombine:RelogConvertToBlg", "[Info]");
                }

                foreach (Process p in relogCollection)
                {
                    p.WaitForExit();
                    p.Dispose();
                }
            }
            catch (Exception e)
            {
                Globals.log.WriteLog(e.Message, "ImportCombine:RelogConvertToBlg", "[Error]");
            }

            RelogCombineToBlg();
        }
        private void RelogCombineToBlg()
        {
            try
            {
                string processCommand = "relog.exe";
                string processArgs = " .\\Individual\\*.blg -o .\\" + ConfigValues.MachineName + "_AllPerfmonMetrics.blg -y";

                Process process = new ProcessUtility().StartProcess(processCommand, processArgs);
                process.WaitForExit();
                process.Dispose();

                Globals.log.WriteLog("relog.exe" + " .\\Individual\\*.blg -o .\\" + ConfigValues.MachineName + "_AllPerfmonMetrics.blg", "ImportCombine:RelogCombineToBlg", "[Info]");
            }
            catch (Exception e)
            {
                Globals.log.WriteLog(e.Message, "ImportCombine:RelogConvertToBlg", "[Error]");
            }
        }
    }
}