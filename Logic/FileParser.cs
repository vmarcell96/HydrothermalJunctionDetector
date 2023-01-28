using HydrothermalJunctionDetector.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.Persistence
{
    internal class FileParser : IFileParser
    {
        private readonly IFileHandler _fileHandler;

        public FileParser(IFileHandler fileHandler)
        {
            _fileHandler = fileHandler;
        }

        public bool CheckFileValidity(string fileLocation)
        {
            throw new NotImplementedException();
        }

        public List<Coordinate> ParseFile(string fileLocation)
        {
            throw new NotImplementedException();
        }
    }
}
