using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.Logic
{
    internal interface IFileParser
    {
        Dictionary<(int, int), int> ParseFile(string fileLocation);

    }
}
