/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System;
using System.IO;
using System.Text;

namespace ConvertLinuxPerfFiles.Logging
{
    // simple logging class to capture errors that may happen and write to a file
    public static class LoggingConfig
    {
        public static string LogFileName { get; set; }
    }

    public class Log
    {
        public void WriteLog(string logMessage, string section, string severity = "[Info]")
        {
            StreamWriter log;
            StringBuilder message = new StringBuilder();

            if (File.Exists(LoggingConfig.LogFileName))
            {
                FileStream fileStream = new FileStream(LoggingConfig.LogFileName, FileMode.Append, FileAccess.Write);
                log = new StreamWriter(fileStream);
            }
            else
            {
                FileStream fileStream = new FileStream(LoggingConfig.LogFileName, FileMode.Create, FileAccess.Write);
                log = new StreamWriter(fileStream);
            }
            string timeStamp = (DateTime.Now).ToString("MM/dd/yyyy H:mm:ss");
            message.Append(timeStamp + "\t" + severity + "\t" + section + "\t" + logMessage + "\r\n");
            log.Write(message);
            log.Dispose();

        }
    }
}