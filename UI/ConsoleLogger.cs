using HydrothermalVentFileParser.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.UI
{
    public class ConsoleLogger : ILogger
    {
        public void LogWriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public void LogWrite(string message)
        {
            Console.Write(message);
        }

        public void LogError(string message)
        {
            Console.WriteLine(message);
        }
    }
}
