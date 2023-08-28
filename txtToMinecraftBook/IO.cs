using System;
using System.Collections.Generic;
using System.IO;

namespace txtToMinecraftBook;

public static class IO
{
    public static bool TryLoad(string filePath, out string[] output)
    {
        try
        {
            output = System.IO.File.ReadAllLines(filePath);
            return true;
        }
        catch (Exception e)
        {
            output = new string[1];
            output[0] = e.ToString();
            return false;
        }
    }

    public static bool TrySave(List<(string filePath, List<string> fileContent)> files, out string error)
    {
        try
        {
            foreach (var file in files)
            {
                File.WriteAllLines(file.filePath, file.fileContent);
            }
            error = "";
            return true;
        }
        catch (Exception e)
        {
            error = e.ToString();
            return false;
        }
    }
}