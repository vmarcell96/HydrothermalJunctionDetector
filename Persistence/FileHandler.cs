using HydrothermalJunctionDetector.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HydrothermalJunctionDetector
{
    public class FileHandler : IFileHandler
    {
        public async Task<string[]> ReadFileAsync(string fileLocation)
        {
            try
            {
                // Read each line of the file into a string array. Each element
                // of the array is one line of the file.
                string[] lines = await File.ReadAllLinesAsync(fileLocation);

                return lines;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task WriteFileAsync(string outputLocation, string[] lines)
        {
            try
            {
                using (StreamWriter outputFile = new StreamWriter(outputLocation, true))
                {
                    Console.WriteLine("Writing file...");
                    await outputFile.WriteAsync($"Number of dangerous points: {lines.Length}\r\n\n\n");
                    foreach (string line in lines)
                    {
                        await outputFile.WriteAsync(line);
                    }
                    Console.WriteLine("Writing is complete.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
