using System;
using System.IO;
using System.Xml.Serialization;

namespace txtToMinecraftBook;

public class Settings
{
    private readonly string _settingsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "txtToMinecraftBook", "Settings.xml");

    public ProgramInfo programInfo = new();
    public Paths paths = new();
    public StendhalSpecifics stendhalSpecifics = new();
    public class ProgramInfo
    {
        public string Version = "v0.0.1";
    }
    public class Paths
    {
        public string Output = "";
    }

    public class StendhalSpecifics
    {
        public string FileEnding = ".stendhal";
        public string BookAuthor = "txtToMinecraftBook";
        public string PageBeginning = "#- ";
        public int MaxLineNumber = 103;
        public int MaxCharOnPage = 256;
        public int MaxLineOnPage = 14;
    }

    public bool Save()
    {
        try
        {
            var fullName = Directory.GetParent(_settingsFilePath)?.FullName;
            if (fullName != null)
                Directory.CreateDirectory(fullName);
            using var writer = new StreamWriter(_settingsFilePath);
            var xmlSerializer = new XmlSerializer(this.GetType());
            xmlSerializer.Serialize(writer, this);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Unable to save the Settings:\n");
            Console.WriteLine(e);
            return false;
        }
    }

    public Settings? Load()
    {
        try
        {
            using var reader = new StreamReader(_settingsFilePath);
            var xmlSerializer = new XmlSerializer(this.GetType());
            return (Settings)xmlSerializer.Deserialize(reader);
        }
        catch
        {
            return null;
        }
    }
}