using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.Persistence
{
    internal interface IFileHandler
    {
        string ReadFile(string fileLocation);
        void WriteFile(string outputLocation);
    }
}
