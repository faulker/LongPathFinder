using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace LongPathFinder
{
    class Program
    {
        static int _ErrorCount = 0;
        static int _AccessErrors = 0;
        const int _MaxNameLength = 255;
        const int _MaxPathLength = 260;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No path given!");
                Console.WriteLine("Usage:\nLongPathFinder.exe [path to check]");
            }
            else
            {
                if (Directory.Exists(args[0]))
                {
                    ProcessDirectory(args[0]);
                    Console.WriteLine("Finished search for files/folder names that are to long and folder paths that are to long.");
                    Console.WriteLine("There were {0} path errors and {1} access errors.", _ErrorCount, _AccessErrors);
                }
                else
                {
                    Console.WriteLine("Path doesn't exist!");
                }
            }

            //Console.ReadKey();
        }

        public static void ProcessDirectory(string targetDirectory)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                try
                {
                    if (fileName.Length > _MaxNameLength)
                    {
                        Console.WriteLine("File name too long: " + fileName);
                        Console.WriteLine("--------------------");
                        _ErrorCount++;
                    }
                }
                catch (PathTooLongException ex)
                {
                    Console.WriteLine("Name too long\n" + ex.Message);
                    Console.WriteLine("--------------------");
                    _ErrorCount++;
                }
                catch (UnauthorizedAccessException ae)
                {
                    Console.WriteLine(ae.Message);
                    Console.WriteLine("--------------------");
                    _AccessErrors++;
                }
            }

            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                try
                {
                    if (subdirectory.Length > _MaxPathLength)
                    {
                        Console.WriteLine("Path is too long: " + subdirectory);
                        Console.WriteLine("--------------------");
                        _ErrorCount++;
                    }
                    else
                    {
                        ProcessDirectory(subdirectory);
                    }
                }
                catch (PathTooLongException ex)
                {
                    Console.WriteLine("Path too long\n" + ex.Message);
                    Console.WriteLine("--------------------");
                    _ErrorCount++;
                }
                catch (UnauthorizedAccessException ae)
                {
                    Console.WriteLine(ae.Message);
                    Console.WriteLine("--------------------");
                    _AccessErrors++;
                }
            }
        }
    }
}
