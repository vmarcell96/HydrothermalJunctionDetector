using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.Logic
{
    internal interface IFileParser
    {
        Task ParseFileParallelAsync(string mode = "normal");

    }
}
