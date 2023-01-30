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

        public Dictionary<(float, float), int> CalculateCrossingPoints(List<VentLine> ventLines)
        {
            Dictionary<(float, float), int> crossingPointDict = new Dictionary<(float, float), int>();
            for (int i = 0; i < ventLines.Count-1; i++)
            {
                var currentLine = ventLines[i];
                for (int j = i+1; j < ventLines.Count; j++)
                {
                    (float, float)? crossingPoint = Utility.GetInterSectionOfSegments(currentLine.StartPoint, currentLine.EndPoint, ventLines[j].StartPoint, ventLines[j].EndPoint);
                    if (crossingPoint.HasValue)
                    {
                        AddPointToDictionary(crossingPoint.Value, crossingPointDict);
                    }
                }
            }
            return crossingPointDict;
        }

        private void AddPointToDictionary((float, float) point, Dictionary<(float, float), int> dict)
        {
            if (dict.ContainsKey(point))
            {
                dict[point]++;
            }
            else
            {
                dict[point] = 1;
            }
        }

        public void GetCoordinates()
        {
            throw new NotImplementedException();
        }

        public string GetInputFileLocation()
        {
            throw new NotImplementedException();
        }

        public void ReportCrossingPoints(Dictionary<(float, float), int> crossingPointDict)
        {
            var keyList = crossingPointDict.Keys.ToList();
            keyList.Sort();
            foreach (var key in keyList)
            {
                Console.WriteLine($"{key.ToString()} -> {crossingPointDict[key]}");
            }
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
