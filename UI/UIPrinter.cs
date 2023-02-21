using HydrothermalVentFileParser.Logging;
using HydrothermalVentFileParser.UI;

namespace HydrothermalJunctionDetector.UI
{
    public class UIPrinter : IUIPrinter
    {
        private readonly ILogger _consoleLogger;
        
        public UIPrinter(ILogger consoleLogger)
        {
            _consoleLogger= consoleLogger;
        }

        public void PrintLine(string message)
        {
            _consoleLogger.LogWriteLine(message);
        }

        public void Print(string message)
        {
            _consoleLogger.LogWrite(message);
        }

        public void ClearConsole()
        {
            Console.Clear();
        }

        public void PrintProgressBar(int percentLoaded)
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(1, 1);
            
            for (int y = 0; y < percentLoaded; y++)
            {
                //Rename "pb" to "progBar" later or else
                //string pb = "\u2551";
                string pb = "\u2588";
                Console.Write(pb);
            }
            Console.Write((percentLoaded + " / 100"));
            Console.WriteLine("\nLoading ...");
            Console.WriteLine("Press ESC to abort process.");
            Console.SetCursorPosition(1, 1);
            
        }

        public void PrintPoints(Dictionary<(int, int), int> pointsDict)
        {
            var maxXCoord = pointsDict.Keys.Select(tuple => tuple.Item1).Max();
            var maxYCoord = pointsDict.Keys.Select(tuple => tuple.Item2).Max();
            int[,] array2D = new int[maxYCoord + 1, maxXCoord + 1];
            for (int i = 0; i < maxYCoord + 1; i++)
            {
                for (int j = 0; j < maxXCoord + 1; j++)
                {
                    if (pointsDict.ContainsKey((i, j)))
                    {
                        array2D[i, j] = pointsDict[(i, j)];
                    }
                    else
                    {
                        array2D[i, j] = 0;
                    }
                    Console.Write(array2D[i, j] == 0 ? "." : array2D[i, j].ToString() + "");
                }
                Console.Write("\n");
            }
        }

        public string GetInputFileLocation()
        {
            var correctFilePath = "default";
            do
            {
                Console.WriteLine("\nPlease enter a filepath (ending with a filename with a .txt file extension)");
                Console.WriteLine("or type in 'default' for the default ventfile input: ");
                var filePath = Console.ReadLine();
                if (filePath == "default")
                {
                    if (File.Exists(@"..\..\..\InputFileLineSegments.txt"))
                    {
                        correctFilePath = @"..\..\..\InputFileLineSegments.txt";
                        break;
                    }
                    Console.WriteLine("\nSorry cannot find the default file.");
                }
                if (filePath == "test")
                {
                    if (File.Exists(@"..\..\..\InputFileLineSegments_Test.txt"))
                    {
                        correctFilePath = @"..\..\..\InputFileLineSegments_Test.txt";
                        break;
                    }
                    Console.WriteLine("\nSorry cannot find the test file.");
                }
                if (File.Exists(filePath))
                {
                    if (Path.GetExtension(filePath) == ".txt")
                    {
                        Console.WriteLine("\nThank you, correct file format.");
                        correctFilePath = filePath;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\nSorry, incorrect file format.");
                    }
                }
                else
                {
                    Console.WriteLine("\nThere is no file found at the provided file path.");
                }

            } while (true);
            return correctFilePath;
        }

        /// <summary>
        ///     Gets output file directory input from user.
        /// </summary>
        /// <returns></returns>
        public string GetOutputFileDirectory()
        {
            var correctFilePath = "default";
            do
            {
                Console.WriteLine("\nDefault file location is the root directory ");
                Console.WriteLine("File name will be: CrossingPoints-RandomID.txt");
                Console.WriteLine("Please enter a path to a directory or type in 'default' for the default output location: ");
                var filePath = Console.ReadLine();
                if (filePath == "default")
                {
                    Console.WriteLine("\nDefault directory selected!");
                    Console.WriteLine("\nFile will be in the project's root directory.");
                    break;
                }
                if (Directory.Exists(filePath))
                {
                    correctFilePath = filePath;
                    break;
                }

            } while (true);
            return correctFilePath;
        }

        /// <summary>
        ///     Displays the dangerous crossing points in the console.
        /// </summary>
        /// <param name="crossingPointDict">Dictionary containing the crossing points with their occurrences.</param>
        public void ReportCrossingPoints(Dictionary<(int, int), int> crossingPointDict, CancellationToken token)
        {
            var keyList = crossingPointDict.Keys.ToList();
            keyList.Sort();
            Console.WriteLine($"Number of dangerous points: {keyList.Count}");
            foreach (var key in keyList)
            {
                if (token.IsCancellationRequested) throw new OperationCanceledException();
                Console.WriteLine($"{key.ToString()} -> {crossingPointDict[key]}");
            }
        }
    }
}
