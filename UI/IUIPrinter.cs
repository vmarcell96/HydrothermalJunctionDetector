using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.UI
{
    internal interface IUIPrinter
    {
        void PrintMainMenu();
        void PrintProgressBar(int percentLoaded);
        void PrintMessage(string message);

        public string GetInputFileLocation();

        public void PrintPoints(Dictionary<(int, int), int> pointsDict);

        public void ClearConsole();

    }
}
