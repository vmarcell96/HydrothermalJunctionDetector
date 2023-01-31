using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.UI
{
    internal class UIPrinter : IUIPrinter
    {
        private readonly ILogger _consoleLogger;
        
        public UIPrinter(ILogger consoleLogger)
        {
            _consoleLogger= consoleLogger;
        }
        public void PrintError(string message)
        {
            throw new NotImplementedException();
        }

        public void PrintMainMenu()
        {
            throw new NotImplementedException();
        }

        public void PrintMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void ClearConsole()
        {
            Console.Clear();
        }

        public void PrintProgressBar(int percentLoaded)
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(1, 1);

            Console.Write("[");
            for (int y = 0; y < percentLoaded; y++)
            {
                //Rename "pb" to "progBar" later or else
                //string pb = "\u2551";
                string pb = "=";
                Console.Write(pb);
            }
            Console.Write("] " + (percentLoaded + " / 100%"));
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
                Console.WriteLine("Please enter filepath or type in 'default' for the default ventfile: ");
                var filePath = Console.ReadLine();
                if (filePath == "default")
                {
                    break;
                }
                if (filePath == "test")
                {
                    correctFilePath = @"..\..\..\InputFileLineSegments_Test.txt";
                    break;
                }
                if (File.Exists(filePath))
                {
                    if (Path.GetExtension(filePath) == ".txt")
                    {
                        Console.WriteLine("Thank you, correct file format.");
                        correctFilePath = filePath;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Sorry, incorrect file format.");
                    }
                }
                else
                {
                    Console.WriteLine("There is no file found at the provided file path.");
                }

            } while (true);
            return correctFilePath;
        }
    }
}
