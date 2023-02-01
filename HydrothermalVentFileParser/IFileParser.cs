using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalVentFileParser
{
    public interface IFileParser
    {
        Task ParseFileParallelAsync(string mode = "normal");

    }
}
