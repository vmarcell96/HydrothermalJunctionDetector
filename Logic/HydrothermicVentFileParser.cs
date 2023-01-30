using HydrothermalJunctionDetector.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.Persistence
{
    internal class HydrothermicVentFileParser : IFileParser
    {
        private readonly IFileHandler _fileHandler;

        public HydrothermicVentFileParser(IFileHandler fileHandler)
        {
            _fileHandler = fileHandler;
        }

        public bool CheckFileValidity(string fileLocation)
        {
            throw new NotImplementedException();
        }

        public List<VentLine> ParseFile(string fileLocation = "default")
        {
            List<VentLine> ventLines = new List<VentLine>();
            try
            {
                // I had to use a random default value because default values must be constant at compile time
                if (fileLocation == "default")
                {
                    fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\InputFileLineSegments.txt");
                }
                string[] lines = _fileHandler.ReadFile(fileLocation);
                
                foreach (var line in lines)
                {
                    int[] coords = ConvertTextLineToIntArray(line);
                    // check if they are horizontal,vertical,diagonal
                    if (CheckLineSlopeValidity(coords[0], coords[1], coords[2], coords[3]))
                    {
                        ventLines.Add(new VentLine()
                        {
                            StartPoint = (coords[0], coords[1]),
                            EndPoint = (coords[2], coords[3])
                        });

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ventLines;
        }

       

        private bool CheckLineSlopeValidity(int startX, int startY, int endX, int endY)
        {
            int deltaY = endY - startY;
            int deltaX = endX - startX;
            return deltaX == 0 || deltaY == 0 || Math.Abs(deltaX) == Math.Abs(deltaY);
        }


        private int[] ConvertTextLineToIntArray(string line, string arrowFormat = " -> ")
        {
            if (!line.Contains(arrowFormat))
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


    }
}
