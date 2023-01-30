using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.Logic
{
    internal interface IMainLogic
    {
        string GetInputFileLocation();
        void ValidateFile();

        void GetCoordinates();
        Dictionary<(float, float), int> CalculateCrossingPoints(List<VentLine> ventLines);
        void ReportCrossingPoints(Dictionary<(float, float), int> crossingPoints);
        void WriteOutReport();
    }
}
