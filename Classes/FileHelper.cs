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
// ***** Need to add logic to check if file exists. *****
        // gets the current directory that the script is executed from
        string currentDirectory = Directory.GetCurrentDirectory();
        // creates the full file path of the file that will be opened
        string[] filePath = Directory.GetFiles(currentDirectory,fName);
        List<string> returnValue = new List<string>();
        // opens the file
        FileStream fs = new FileStream(filePath[0],FileMode.Open);

        // reads each line of the file and loads them into a list that gets returned    
        using (StreamReader sr = new StreamReader(fs))
        {
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                returnValue.Add(line);
            }
        }

        // array of lines that got read and is returned
        return returnValue;
    }
}