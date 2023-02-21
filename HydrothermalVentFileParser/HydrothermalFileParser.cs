
using HydrothermalVentFileParser.Persistence;
using HydrothermalVentFileParser.UI;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HydrothermalVentFileParser
{
    public class HydrothermalFileParser : IFileParser
    {
        private readonly IFileHandler _fileHandler;
        private readonly IUIPrinter _uiPrinter;
        private CancellationTokenSource? _calculationCts;
        private CancellationTokenSource? _keyboardTaskCts;
        private readonly Stopwatch _stopwatch;
        private Dictionary<(int, int), int> _pointDict;
        private List<string> _logs;

        public HydrothermalFileParser(IFileHandler fileHandler, IUIPrinter uiPrinter)
        {
            _fileHandler = fileHandler;
            _uiPrinter = uiPrinter;
            _stopwatch = new Stopwatch();
            //initializing the dictionary that will hold all the segment points
            _pointDict = new Dictionary<(int, int), int>();
            _logs = new List<string>();
        }

        /// <summary>
        ///     Main parsing logic. Calculating segment point tasks are running parallel.
        ///     Currently cancelling the method is only possible when the progress bar is shown inicating that
        ///     the segment point calculations are running.
        /// </summary>
        /// <param name="mode">If "print points" mode is chosen the method will print the segment points represented as the value of their occurrences.</param>
        /// <returns></returns>
        /// <exception cref="TaskCanceledException">Throws exception if the segment point calculation is cancelled with ESC keypress.</exception>
        public async Task ParseFileParallelAsync(string mode = "normal")
        {
            try
            {
                _calculationCts = new();
                _keyboardTaskCts = new();

                string[] lines = await GetLineData();

                // Before the main calculation begins, start listening to the abort keypress    
                RunKeyBoardTask();

                _uiPrinter.ClearConsole();

                _stopwatch.Start();

                // Parallel calculations
                (int, int)[][] results = await GetPointsOfSegments(lines);

                _stopwatch.Stop();
                _uiPrinter.ClearConsole();
                _logs.Add($"Parsing points in {_stopwatch.ElapsedMilliseconds} ms");


                ProcessPoints(results);

                ////reporting points to the console
                _uiPrinter.ReportCrossingPoints(_pointDict, _calculationCts.Token);

                //if "mode" variable equals "print points" the method will print the coordinates just like in the example
                if (mode == "print points")
                {
                    _uiPrinter.PrintPoints(_pointDict);
                }

                
                _uiPrinter.PrintLine("Parsing is complete press ENTER two times to continue");

                Console.ReadKey();

                #region Outputting the results
                await WriteOutReportAsync();
                #endregion

                ShowLogs();
            }
            //Catches error if user is unauthorized to write in selected directory
            //asks for output directory again
            catch (UnauthorizedAccessException e)
            {
                _uiPrinter.PrintLine(e.Message);
                await WriteOutReportAsync();
            }
            catch (Exception)
            {
                CleanupCancellationTokenSources();
                throw;
            }
        }

        private void CleanupCancellationTokenSources()
        {
            _keyboardTaskCts.Cancel();
            _keyboardTaskCts.Dispose();
            _keyboardTaskCts = null;
            _calculationCts.Dispose();
            _calculationCts = null;
        }


        /// <summary>
        ///     Starts a Task where the thread is listening to the user's keypress.
        ///     If key is pressed the function aborts.
        /// </summary>
        private void RunKeyBoardTask()
        {
            // Creating a task to listen to keyboard key press
            var keyBoardTask = Task.Run(() =>
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    _calculationCts.Cancel();
                }

            }, _keyboardTaskCts.Token);
        }

        private void ShowLogs()
        {
            _uiPrinter.PrintLine(Environment.NewLine);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Red;
            _uiPrinter.PrintLine("Performance Log:");
            _uiPrinter.PrintLine(Environment.NewLine);
            foreach (var log in _logs)
            {
                _uiPrinter.PrintLine(log);
            };
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            _logs.Clear();
        }

        private Task<string[]> GetLineData()
        {
            try
            {
                //asking user for input file location
                string filePath = _uiPrinter.GetInputFileLocation();

                //if the user selected 'default' as the input file location we are using the default path
                if (filePath == "default")
                {
                    filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\InputFileLineSegments.txt");
                }

                // input file's lines are being read
                return _fileHandler.ReadFileAsync(filePath);
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        private async Task<(int, int)[][]> GetPointsOfSegments(string[] lines)
        {
            var list = new List<(int, int)[]>();
            int invalidLineCount = 0;

            //CPU demanding task
            //Tasks are going to run parallel
            await Task.Run(() =>
            {
                try
                {
                    Task.Run(() =>
                    {
                        ShowLoadingBar(list, invalidLineCount, lines);
                    });


                    foreach (var line in lines)
                    {
                        CalculateSegmentPoints(list, ref invalidLineCount, line);
                    };
                }
                catch (TaskCanceledException)
                {
                    throw;
                }
                catch (AggregateException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
            });
            return list.ToArray();
        }

        private void CalculateSegmentPoints(List<(int, int)[]> bag, ref int invalidLineCount, string line)
        {
            int[] intArray = ConvertTextLineToIntArray(line);
            if (CheckLineSlopeValidity(intArray[0], intArray[1], intArray[2], intArray[3]))
            {
                //Thread.Sleep(1);
                var result = Utility.FindIntegerLineSegmentPoints(intArray[0], intArray[1], intArray[2], intArray[3]);
                bag.Add(result);
            }
            else
            {
                invalidLineCount++;
            }
        }

        private void ShowLoadingBar(List<(int, int)[]> bag, int invalidLineCount, string[] lines)
        {
            while (((bag.Count + invalidLineCount) * 100 / lines.Length) < 100)
            {
                if (_calculationCts.Token.IsCancellationRequested)
                    break;
                _uiPrinter.PrintProgressBar((bag.Count * 100) / lines.Length);
            }
            _uiPrinter.ClearConsole();
        }

        private void ProcessPoints((int, int)[][] pointArrays)
        {
            try
            {
                _stopwatch.Restart();

                //all the points are being added to pointDict dictionary
                _uiPrinter.ClearConsole();
                _uiPrinter.PrintLine("Adding points to dictionary...");
                foreach (var pointArray in pointArrays)
                {
                    if (_calculationCts.Token.IsCancellationRequested)
                    {
                        throw new OperationCanceledException();
                    }
                    AddPointsToDictionary(pointArray);
                }

                _stopwatch.Stop();
                _logs.Add($"Adding points to dictionary in: {_stopwatch.ElapsedMilliseconds} ms");

                //filtering the points that have more than 1 occurrences
                _stopwatch.Restart();

                _uiPrinter.ClearConsole();
                _uiPrinter.PrintLine("Filtering crossing points...");
                _pointDict = FilterCrossingPoints(_pointDict);

                _stopwatch.Stop();
                _logs.Add($"Filtering points done in: {_stopwatch.ElapsedMilliseconds} ms");
            }
            catch (Exception)
            {
                _uiPrinter.ClearConsole();
                //Only shows message later when I print this first.
                _uiPrinter.PrintLine("asd");
                throw;
            }
        }


        /// <summary>
        ///     Gets output directory, writes text file reporting the dangerous points.
        /// </summary>
        /// <param name="dict">Input dictionary.</param>
        /// <returns></returns>
        private async Task WriteOutReportAsync()
        {
            //getting the output file directory from the user
            string outputDirectoryPath = _uiPrinter.GetOutputFileDirectory();

            //if "default is chosen as path the default root directory will be used"
            if (outputDirectoryPath == "default")
            {
                outputDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\CrossingPoints" + $"-{Guid.NewGuid()}" + ".txt");
            }
            else
            {
                outputDirectoryPath = Path.Combine(outputDirectoryPath, $"CrossingPoints{Guid.NewGuid()}.txt");
            }

            _stopwatch.Restart();

            _uiPrinter.ClearConsole();
            _uiPrinter.PrintLine("Writing report to file...");
            try
            {
                await _fileHandler.WriteFileAsync(outputDirectoryPath, Utility.ConvertPointDictionaryToStringArray(_pointDict));
            }
            catch (Exception)
            {

                throw;
            }
            _stopwatch.Stop();
            _logs.Add($"Writing finished in: {_stopwatch.ElapsedMilliseconds} ms");
        }
        

        /// <summary>
        ///     Adds (int, int) types to a dictionary from a (int, int) array
        ///     as key and a value based on how many times they occurrenced.
        /// </summary>
        /// <param name="points">Array of (int, int)s.</param>
        /// <param name="pointsDict">Input dictionary.</param>
        private void AddPointsToDictionary((int, int)[] points)
        {

            foreach (var point in points)
            {
                if (_pointDict.ContainsKey(point))
                {
                    _pointDict[point]++;
                }
                else
                {
                    _pointDict[point] = 1;
                }
            }

            
        }


        /// <summary>
        ///     Checks if a line segment's slope is a multiple of 45 degrees or not.
        ///     If not it returns false.
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <returns>Returns true or false based on the segments slope.</returns>
        private bool CheckLineSlopeValidity(int startX, int startY, int endX, int endY)
        {
            int deltaY = endY - startY;
            int deltaX = endX - startX;
            return deltaX == 0 || deltaY == 0 || Math.Abs(deltaX) == Math.Abs(deltaY);
        }


        /// <summary>
        ///     This method tries to convert a string to an int array. If it finds
        ///     a format error throws an exception because if one line of the input
        ///     file is invalid the whole parsing process stops.
        /// </summary>
        /// <param name="line">Current line of the txt file which is being checked.</param>
        /// <param name="arrowFormat">Desired arrow format of the input txt file.</param>
        /// <returns>An int array with the length of 4.</returns>
        /// <exception cref="FormatException"></exception>
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


        /// <summary>
        ///     This method checks a string if it is contains the desired arrow format.
        /// </summary>
        /// <param name="line">Current line of the txt file which is being checked.</param>
        /// <param name="arrowFormat">Desired arrow format of the input txt file.</param>
        /// <returns>True if it contains, false otherwise.</returns>
        private bool HasValidArrowFormat(string line, string arrowFormat)
        {
            return line.Contains(arrowFormat);
        }


        /// <summary>
        ///     This method filters a disctionary based on it's values, if a key,value pair
        ///     has less value than the param: minimumOccurrences than this key,value pair is
        ///     filtered out.
        /// </summary>
        /// <param name="pointsDict">Input dictionary.</param>
        /// <param name="minimumOcurrencies">Input filter number, equals 2 by default.</param>
        /// <returns>Returns a Dictionary<(int, int), int> type object with the filtered key,value pairs.</returns>
        private Dictionary<(int, int), int> FilterCrossingPoints(Dictionary<(int, int), int> pointDict, int minimumOccurrences = 2)
        {
            return pointDict.Where(kvp => kvp.Value >= minimumOccurrences).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        


    }
}
