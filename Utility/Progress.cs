using System;

namespace ConvertLinuxPerfFiles.Utility
{
    public static class ProgressConfig
    {
        public static int progressLine = 0;
    }
    class Progress
    {
        public Progress()
        {
            ProgressConfig.progressLine++;
        }
        public void WriteTitle(string title)
        {
            Console.SetCursorPosition(1, ProgressConfig.progressLine);
            Console.WriteLine(title);
        }
        public void WriteProgress(int value, int total)
        {
            double percent = (Convert.ToDouble(value) / Convert.ToDouble(total) * 100);
            Console.SetCursorPosition(1, ProgressConfig.progressLine + 1);
            Console.Write(String.Format("{0:F2}", percent));
        }
    }
}