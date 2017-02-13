using System.IO;
using System.Collections.Generic;
class FileHelper
{
    string fName;

    public FileHelper(string fileName)
    {
        fName = fileName;
    }
    private void setFileName()
    {

    }
    public string getFileName()
    {
        return fName;
    }

    public List<string> readFileByLine()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string[] filePath = Directory.GetFiles(currentDirectory,fName);
        List<string> returnValue = new List<string>();
        FileStream fs = new FileStream(filePath[0],FileMode.Open);
            
        using (StreamReader sr = new StreamReader(fs))
        {
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                returnValue.Add(line);
            }
        }

            return returnValue;
    }
}