using HydrothermalJunctionDetector.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HydrothermalJunctionDetector
{
    internal class FileHandler : IFileHandler
    {
        public string[] ReadFile(string fileLocation = "default")
        {
            // I had to use a random default value because default values must be constant at compile time
            if (fileLocation == "default")
            {
                fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\InputFileLineSegments.txt");
            }
            // Read each line of the file into a string array. Each element
            // of the array is one line of the file.
            string[] lines = File.ReadAllLines(fileLocation);
            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                Console.WriteLine("\t" + line);
            }

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
            return lines;
        }

        public void WriteFile(string outputLocation)
        {
            throw new NotImplementedException();
        }
    }
}
