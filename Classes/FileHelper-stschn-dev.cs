using System.IO;
using System.Collections.Generic;
class FileHelper
{
    string FileName;
    string FullFilePath;

    public FileHelper(string fileName)
    {
        setFileName(fileName);
        setFullFilePath();
    }
    private void setFileName(string fileName)
    {
        FileName = fileName;
    }

    private void setFullFilePath()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string[] filePath = Directory.GetFiles(currentDirectory,FileName);

        FullFilePath = filePath[0];
    }
    public string getFileName()
    {
        return FileName;
    }

    public string getFullFilePath()
    {
        return FullFilePath;
    }

    public List<string> readFileByLine()
    {
// ***** Need to add logic to check if file exists. *****
        List<string> returnValue = new List<string>();
        // opens the file
        FileStream fileStream = new FileStream(FullFilePath,FileMode.Open);

        // reads each line of the file and loads them into a list that gets returned    
        using (StreamReader streamReader = new StreamReader(fileStream))
        {
            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine();
                returnValue.Add(line);
            }
        }

        // array of lines that got read and is returned
        return returnValue;
    }
}