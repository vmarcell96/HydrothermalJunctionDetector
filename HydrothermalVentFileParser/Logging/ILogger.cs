using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalVentFileParser.Logging
{
    public interface ILogger
    {
        void LogWriteLine(string message);
        public void LogWrite(string message);
        public void LogError(string message);
    }
}
