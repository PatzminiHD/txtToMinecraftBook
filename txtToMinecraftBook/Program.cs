using System;

namespace txtToMinecraftBook;
internal static class Program
{
    private static Settings _settings = new Settings();
    public static void Main(string[] args)
    {
        var loadedSettings = _settings.Load();
        
        if (loadedSettings != null)
        {
            _settings = loadedSettings;
        }
        else
        {
            Console.WriteLine("Error loading settings.\nDo you want to initialise with default settings? [Y/n] ");
            while (true)
            {
                var line = Console.ReadLine();
                if (line is "Y" or "")
                {
                    Console.WriteLine(_settings.Save()
                        ? "Settings file initialised"
                        : "Error initialising settings file");
                    break;
                }
                if (line == "N")
                {
                    break;
                }
                Console.WriteLine("[Y/n] ");
            }
        }
    }
}