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
        public string[] ReadFile(string fileLocation)
        {
            // Read each line of the file into a string array. Each element
            // of the array is one line of the file.
            string[] lines = File.ReadAllLines(fileLocation);


            return lines;
        }

        public void WriteFile(string outputLocation)
        {
            throw new NotImplementedException();
        }
    }
}
