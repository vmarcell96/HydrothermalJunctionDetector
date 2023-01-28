using HydrothermalJunctionDetector.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.Logic
{
    internal class VentFileParser : FileParser
    {
        public VentFileParser(IFileHandler fileHandler) : base(fileHandler)
        {

        }
        public bool CheckCoordValidity(Coordinate coord)
        {
            return true;
        }
    }
}
