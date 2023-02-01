using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalVentFileParser.Persistence
{
    public interface IFileHandler
    {
        Task<string[]> ReadFileAsync(string fileLocation);
        Task WriteFileAsync(string outputLocation, string[] lines);
    }
}
