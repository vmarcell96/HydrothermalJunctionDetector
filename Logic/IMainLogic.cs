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
        void CalculateCrossingPoints();
        void ReportCrossingPoints();
        void WriteOutReport();
    }
}
