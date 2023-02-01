
using HydrothermalVentFileParser.Persistence;
using HydrothermalVentFileParser.UI;

namespace HydrothermalVentFileParser
{
    public class HydrothermalFileParser : IFileParser
    {
        private readonly IFileHandler _fileHandler;
        private readonly IUIPrinter _uiPrinter;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public HydrothermalFileParser(IFileHandler fileHandler, IUIPrinter uiPrinter)
        {
            _fileHandler = fileHandler;
            _uiPrinter = uiPrinter;
            _cancellationTokenSource = new CancellationTokenSource();
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
                    // Cancel the task
                    _uiPrinter.ClearConsole();
                    _cancellationTokenSource.Cancel();
                }
            });
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
            //declaring and initializing list for calculate tasks, and for the dictionary that will hold all the segment points
            List<Task<(int, int)[]>> tasks = new List<Task<(int, int)[]>>();
            Dictionary<(int, int), int> pointDict = new Dictionary<(int, int), int>();
            
            try
            {
                #region Getinput,Readfile

                //asking user for input file location
                string filePath = _uiPrinter.GetInputFileLocation();

                //if the user selected 'default' as the input file location we are using the default path
                if (filePath == "default")
                {
                    filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\InputFileLineSegments.txt");
                }

                // input file's lines are being read
                string[] lines = await _fileHandler.ReadFileAsync(filePath);

                #endregion

                _uiPrinter.ClearConsole();
                _uiPrinter.PrintProgressBar(0);

                // Before the main calculation begins, start listening to the abort keypress
                RunKeyBoardTask();

                #region Calculating all the integer points of a segment

                for (int i = 0; i < lines.Length; i++)
                {
                    int[] intArray = ConvertTextLineToIntArray(lines[i]);
                    if (CheckLineSlopeValidity(intArray[0], intArray[1], intArray[2], intArray[3]))
                    {
                        //CPU demanding task
                        //Tasks are going to run parallel
                        tasks.Add(Task.Run(() => Utility.FindIntegerLineSegmentPoints(intArray[0], intArray[1], intArray[2], intArray[3])));
                    }
                }

                #endregion


                #region Displaying progress bar as point calculation tasks are being finished(cancelling parsing is possible here)
                int processedLines = 0;

                //when a task completes the progress bar is printed
                while ((processedLines * 100 / lines.Length) < 100)
                {
                    if (_cancellationTokenSource.Token.IsCancellationRequested)
                        throw new TaskCanceledException();

                    var finishedTask = await Task.WhenAny(tasks);
                    processedLines++;
                    _uiPrinter.PrintProgressBar((processedLines * 100) / lines.Length);
                    //
                    //Thread.Sleep(1);
                }
                #endregion
                

                _uiPrinter.ClearConsole();

                //waiting for all tasks to finish
                var results = await Task.WhenAll(tasks);

                #region Processing all the found segment points
                //all the points are being added to pointDict dictionary
                foreach (var result in results)
                {
                    AddPointsToDictionary(result, pointDict);
                }

                //filtering the points with more than 1 occurrences
                pointDict = FilterCrossingPoints(pointDict);

                #endregion

                #region Report
                //reporting points to the console
                _uiPrinter.ReportCrossingPoints(pointDict);

                //if "mode" variable equals "print points" the method will print the coordinates just like in the example
                if (mode == "print points")
                {
                    _uiPrinter.PrintPoints(pointDict);
                }
                #endregion

                _uiPrinter.PrintLine("Parsing is complete press ENTER two times to continue");
                
                Console.ReadKey();

                #region Outputting the results
                await WriteOutReportAsync(pointDict);
                #endregion

            }
            //Catches error if user is unauthorized to write in selected directory
            //asks for output directory again
            catch (UnauthorizedAccessException e)
            {
                _uiPrinter.PrintLine(e.Message);
                await WriteOutReportAsync(pointDict);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///     Gets output directory, writes text file reporting the dangerous points.
        /// </summary>
        /// <param name="dict">Input dictionary.</param>
        /// <returns></returns>
        private async Task WriteOutReportAsync(Dictionary<(int,int), int> dict)
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
            //writing dangerous points into file
            await _fileHandler.WriteFileAsync(outputDirectoryPath, Utility.ConvertPointDictionaryToStringArray(dict));
        }
        

        /// <summary>
        ///     Adds (int, int) types to a dictionary from a (int, int) array
        ///     as key and a value based on how many times they occurrenced.
        /// </summary>
        /// <param name="points">Array of (int, int)s.</param>
        /// <param name="pointsDict">Input dictionary.</param>
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
        private Dictionary<(int, int), int> FilterCrossingPoints(Dictionary<(int, int), int> pointsDict, int minimumOccurrences = 2)
        {

            return pointsDict.Where(kvp => kvp.Value >= minimumOccurrences).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        


    }
}
