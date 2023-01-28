using HydrothermalJunctionDetector.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.Logic
{
    internal class MainLogic : IMainLogic
    {
        private readonly IFileParser _fileParser;
        private readonly IUIPrinter _uiPrinter;

        public MainLogic(IFileParser fileParser, IUIPrinter uiPrinter)
        {
            _fileParser = fileParser;
            _uiPrinter = uiPrinter;
        }

        private void AbortProcess()
        {
            throw new NotImplementedException();
        }

        public void CalculateCrossingPoints()
        {
            throw new NotImplementedException();
        }

        public void GetCoordinates()
        {
            throw new NotImplementedException();
        }

        public string GetInputFileLocation()
        {
            throw new NotImplementedException();
        }

        public void ReportCrossingPoints()
        {
            throw new NotImplementedException();
        }

        public void ValidateFile()
        {
            throw new NotImplementedException();
        }

        public void WriteOutReport()
        {
            throw new NotImplementedException();
        }
    }
}
