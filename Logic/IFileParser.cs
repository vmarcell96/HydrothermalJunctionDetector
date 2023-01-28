using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.Logic
{
    internal interface IFileParser
    {
        List<VentLine> ParseFile(string fileLocation);
        bool CheckFileValidity(string fileLocation);


    }
}
