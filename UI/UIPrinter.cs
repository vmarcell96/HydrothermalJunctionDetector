using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.UI
{
    internal class UIPrinter : IUIPrinter
    {
        private readonly ILogger _consoleLogger;
        
        public UIPrinter(ILogger consoleLogger)
        {
            _consoleLogger= consoleLogger;
        }
        public void PrintError(string message)
        {
            throw new NotImplementedException();
        }

        public void PrintMainMenu()
        {
            throw new NotImplementedException();
        }

        public void PrintMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void PrintProgressBar(int percentLoaded)
        {
            throw new NotImplementedException();
        }
    }
}
