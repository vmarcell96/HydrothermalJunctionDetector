using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.UI
{
    internal class ConsoleLogger : ILogger
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
            Console.Write(message);
        }
    }
}
