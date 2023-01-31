using HydrothermalJunctionDetector.Logic;
using HydrothermalJunctionDetector.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.Persistence
{
    internal class HydrothermalVentFileParser : IFileParser
    {
        private readonly IFileHandler _fileHandler;
        private readonly IUIPrinter _uiPrinter;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public HydrothermalVentFileParser(IFileHandler fileHandler, IUIPrinter uiPrinter)
        {
            _fileHandler = fileHandler;
            _uiPrinter = uiPrinter;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void RunKeyBoardTask()
        {
            // Creating a task to listen to keyboard key press
            var keyBoardTask = Task.Run(() =>
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    // Cancel the task
                    _cancellationTokenSource.Cancel();
                }
            });
        }

        public async Task ParseFileParallelAsync(string mode = "normal")
        {
            
            
            List<Task<(int, int)[]>> tasks = new List<Task<(int, int)[]>>();
            Dictionary<(int, int), int> pointDict = new Dictionary<(int, int), int>();
            
            try
            {

                string filePath = _uiPrinter.GetInputFileLocation();
                // I had to use a random default value because default values must be constant at compile time
                if (filePath == "default")
                {
                    filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\InputFileLineSegments.txt");
                }

                string[] lines = await _fileHandler.ReadFileAsync(filePath);


                int processedLines = 0;
                _uiPrinter.ClearConsole();
                _uiPrinter.PrintProgressBar(0);

                RunKeyBoardTask();

                for (int i = 0; i < lines.Length; i++)
                {
                    int[] intArray = ConvertTextLineToIntArray(lines[i]);
                    if (CheckLineSlopeValidity(intArray[0], intArray[1], intArray[2], intArray[3]))
                    {
                        //CPU demanding task
                        tasks.Add(Task.Run(() => Utility.FindIntegerLineSegmentPoints(intArray[0], intArray[1], intArray[2], intArray[3])));
                    }
                }

                while ((processedLines * 100 / lines.Length) < 100)
                {
                    if (_cancellationTokenSource.Token.IsCancellationRequested)
                        throw new TaskCanceledException();

                    var finishedTask = await Task.WhenAny(tasks);
                    processedLines++;
                    _uiPrinter.PrintProgressBar((processedLines * 100) / lines.Length);
                    Thread.Sleep(1);
                }

                _uiPrinter.ClearConsole();
                var results = await Task.WhenAll(tasks);


                foreach (var result in results)
                {
                    AddPointsToDictionary(result, pointDict);
                }


                pointDict = FilterCrossingPoints(pointDict);

                ReportCrossingPoints(pointDict);

                if (mode == "print points")
                {
                    _uiPrinter.PrintPoints(pointDict);
                }


            }
            catch (Exception)
            {
                throw;
            }
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

        private Dictionary<(int, int), int> FilterCrossingPoints(Dictionary<(int, int), int> pointsDict, int minimumOcurrencies = 2)
        {

            return pointsDict.Where(kvp => kvp.Value >= minimumOcurrencies).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
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


    }
}
