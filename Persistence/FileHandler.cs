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
            // Read each line of the file into a string array. Each element
            // of the array is one line of the file.
            string[] lines = await File.ReadAllLinesAsync(fileLocation);

            return lines;
        }

        public async Task WriteFileAsync(string outputLocation)
        {
            throw new NotImplementedException();
        }
    }
}
