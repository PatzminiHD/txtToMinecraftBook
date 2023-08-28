using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace txtToMinecraftBook;
internal static class Program
{
    private static Settings _settings = new Settings();
    private static string[] _inputTxt = Array.Empty<string>();
    private static string _bookTitle;
    private static string _bookAuthor;
    private static List<(string filePath, List<string> fileContent)> _outputs = new();
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
                    Console.WriteLine("Using default settings, but not writing to settings file...");
                    loadedSettings = new Settings();
                    break;
                }
                Console.WriteLine("[Y/n] ");
            }
        }
        
        Console.WriteLine("Enter the file path of the input txt file:");
        if(!IO.TryLoad(Console.ReadLine(), out _inputTxt))
        {
            Console.WriteLine($"Error loading file:\n{_inputTxt[0]}");
            Environment.Exit(-9);
        }

        if (_settings.paths.Output == "")
        {
            Console.WriteLine("Enter the directory of where to output the stendhal files:");
            _settings.paths.Output = Console.ReadLine();
        }
        
        Console.WriteLine("Enter the name of the Book:");
        _bookTitle = Console.ReadLine();

        var _outputs = Convert(_inputTxt, _bookTitle, _settings.paths.Output + _bookTitle);

        if(!IO.TrySave(_outputs, out string e))
        {
            Console.WriteLine("Error saving files:\n" + e);
            Environment.Exit(-99);
        }
        
        Console.WriteLine("Finished saving the files!");
        Environment.Exit(0);
    }


    private static List<(string filePath, List<string> fileContent)> Convert(string[] inputFile, string bookTitle, string outputPath)
    {
        List<(string filePath, List<string> fileContent)> output = new();
        var i = 0;
        List<string> fileContent = new()
        {
            $"title: {bookTitle}_{i:000}",
            $"author: {_settings.stendhalSpecifics.BookAuthor}",
            "pages:",
        };
        string page = "";
        for (int j = 0; j < inputFile.Length; j++)
        {
            for (int k = 0; k < inputFile[j].Length; k++)
            {
                page += inputFile[j][k];
                
                string tmpPage = page;
                int tmpK = k;
                
                if (page.Length > _settings.stendhalSpecifics.MaxCharOnPage - 1 || (j == inputFile.Length - 1 && k == inputFile[j].Length - 1) || page.Count(c => c == '\n') > _settings.stendhalSpecifics.MaxLineOnPage)
                {
                    for (int l = page.Length - 1; l > 0; l--)
                    {
                        if (page[l] == ' ')
                        {
                            page = page.Substring(0, l);
                            break;
                        }
                        k = tmpK - (page.Length - l);
                    }
                    if (k <= 0 || (j == inputFile.Length - 1 && page.Length + (tmpK - k) < _settings.stendhalSpecifics.MaxCharOnPage))
                    {
                        k = tmpK;
                        page = tmpPage;
                    }
                    page = page.Trim();
                    fileContent.Add(_settings.stendhalSpecifics.PageBeginning + page);
                    page = "";
                }
                if (fileContent.Count > _settings.stendhalSpecifics.MaxLineNumber - 1 || (j == inputFile.Length - 1 && tmpK == inputFile[j].Length - 1))
                {
                    string filePath = Path.Combine(outputPath + $"_{i:000}" + _settings.stendhalSpecifics.FileEnding);
                    output.Add((filePath, fileContent));
                    i++;
                    fileContent = new()
                    {
                        $"title: {bookTitle}_{i:000}",
                        $"author: {_settings.stendhalSpecifics.BookAuthor}",
                        "pages:",
                    };
                }
            }
            
            if(page != "" && page.Last() != ' ')
                page += " ";
        }
        
        

        return output;
    }
}