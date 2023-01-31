using HydrothermalJunctionDetector.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.Logic
{
    internal class MainLogic
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

        

        public void Run(string mode)
        {
            string filePath = _uiPrinter.GetInputFileLocation();
            _uiPrinter.ClearConsole();
            var pointsDict = _fileParser.ParseFile(filePath);
            _uiPrinter.ClearConsole();
            //By default this feature is disabled because console window is too small to display all the points
            if (mode == "print points")
            {
                _uiPrinter.PrintPoints(pointsDict);
            }
            var crossingPoints = FilterCrossingPoints(pointsDict);
            ReportCrossingPoints(crossingPoints);
        }

        public void ReportCrossingPoints(Dictionary<(int, int), int> crossingPointDict)
        {
            var keyList = crossingPointDict.Keys.ToList();
            keyList.Sort();
            Console.WriteLine($"Number of dangerous points: {keyList.Count}");
            foreach (var key in keyList)
            {
                Console.WriteLine($"{key.ToString()} -> {crossingPointDict[key]}");
            }
        }

        private Dictionary<(int, int), int> FilterCrossingPoints(Dictionary<(int, int), int> pointsDict, int minimumOcurrencies = 2)
        {

            return pointsDict.Where(kvp => kvp.Value >= minimumOcurrencies).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public void WriteOutReport()
        {
            throw new NotImplementedException();
        }

    }
}
