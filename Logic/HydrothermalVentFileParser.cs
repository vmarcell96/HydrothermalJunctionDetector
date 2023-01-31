using HydrothermalJunctionDetector.Logic;
using HydrothermalJunctionDetector.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.Persistence
{
    internal class HydrothermalVentFileParser : IFileParser
    {
        private readonly IFileHandler _fileHandler;
        private readonly IUIPrinter _uiPrinter;

        public HydrothermalVentFileParser(IFileHandler fileHandler, IUIPrinter uiPrinter)
        {
            _fileHandler = fileHandler;
            _uiPrinter = uiPrinter;
        }

        public Dictionary<(int, int), int> ParseFile(string fileLocation)
        {
            Dictionary<(int, int), int> result = new Dictionary<(int, int), int>();
            try
            {
                // I had to use a random default value because default values must be constant at compile time
                if (fileLocation == "default")
                {
                    fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\InputFileLineSegments.txt");
                }

                string[] lines = _fileHandler.ReadFile(fileLocation);
                _uiPrinter.PrintProgressBar(0);
                for (int i = 0; i < lines.Length; i++)
                {
                    int[] intArray = ConvertTextLineToIntArray(lines[i]);
                    double processedPercentage = 0;
                    
                    // check if they are horizontal,vertical,diagonal, if not the vent line is skipped
                    if (CheckLineSlopeValidity(intArray[0], intArray[1], intArray[2], intArray[3]))
                    {
                        //CPU demanding task
                        CalculatePoints(intArray[0], intArray[1], intArray[2], intArray[3], result);
                        processedPercentage = (((i+1) * 100) / lines.Length);
                        _uiPrinter.PrintProgressBar((int)Math.Round(processedPercentage));
                        Thread.Sleep(5);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            _uiPrinter.PrintProgressBar(100);
            Thread.Sleep(500);
            return result;
        }

        private (int, int)[] CalculatePoints(int intA, int intB, int intC, int intD, Dictionary<(int, int), int> dict)
        {
            var result = Utility.FindIntegerLineSegmentPoints(intA, intB, intC, intD);
            AddPointsToDictionary(result, dict);
            return result;
        }


        private void AddPointsToDictionary((int, int)[] points, Dictionary<(int, int), int> pointsDict)
        {
            foreach (var point in points)
            {
                if (pointsDict.ContainsKey(point))
                {
                    pointsDict[point]++;
                }
                else
                {
                    pointsDict[point] = 1;
                }
            }
        }


        private bool CheckLineSlopeValidity(int startX, int startY, int endX, int endY)
        {
            int deltaY = endY - startY;
            int deltaX = endX - startX;
            return deltaX == 0 || deltaY == 0 || Math.Abs(deltaX) == Math.Abs(deltaY);
        }


        private int[] ConvertTextLineToIntArray(string line, string arrowFormat = " -> ")
        {
            if (!HasValidArrowFormat(line, arrowFormat))
            {
                throw new FormatException("Line format is invalid.");
            }

            string[] coords = line.Replace(arrowFormat, ",").Split(",");

            if (coords.Length != 4)
            {
                throw new FormatException("Line format is invalid.");
            }

            int[] coordInts = new int[4];

            for (int i=0;i<4;i++)
            {
                if (int.TryParse(coords[i], out int coordInt))
                {
                    coordInts[i] = coordInt;
                }
                else
                {
                    throw new FormatException("Line format is invalid.");
                }
            }
            return coordInts;
        }

        private bool HasValidArrowFormat(string line, string arrowFormat)
        {
            return line.Contains(arrowFormat);
        }


    }
}
