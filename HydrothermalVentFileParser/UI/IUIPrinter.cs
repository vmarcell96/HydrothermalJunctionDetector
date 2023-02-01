using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalVentFileParser.UI
{
    public interface IUIPrinter
    {
        void PrintProgressBar(int percentLoaded);
        void PrintLine(string message);
        public void Print(string message);
        public string GetInputFileLocation();
        public string GetOutputFileDirectory();
        public void PrintPoints(Dictionary<(int, int), int> pointsDict);
        public void ClearConsole();
        public void ReportCrossingPoints(Dictionary<(int, int), int> crossingPointDict);

    }
}
