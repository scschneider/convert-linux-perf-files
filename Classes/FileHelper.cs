using System.IO;
using System.Collections.Generic;
class FileHelper
{
// class properties
    public string FileName {get; private set;}

// class constructor
    public FileHelper(string fileName)
    {
        SetFileName(fileName);
    }

// class private setters
    private void SetFileName(string fileName)
    {
        FileName = fileName;
    }

// class methods
    public List<string> ReadFileByLine()
    {
// ***** Need to add logic to check if file exists. *****
        // gets the current directory that the script is executed from
        string currentDirectory = Directory.GetCurrentDirectory();
        // creates the full file path of the file that will be opened
        string[] filePath = Directory.GetFiles(currentDirectory,FileName);
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
        }// END using StreamReader

        // array of lines that got read and is returned
        return returnValue;
    }// END ReadFileByLine
}