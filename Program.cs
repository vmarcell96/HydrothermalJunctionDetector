// See https://aka.ms/new-console-template for more information
using HydrothermalJunctionDetector;
using HydrothermalJunctionDetector.Logic;
using HydrothermalJunctionDetector.Persistence;
using HydrothermalJunctionDetector.UI;
using System.Diagnostics;

namespace HydrothermalJunctionDetector
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ILogger consoleLogger = new ConsoleLogger();
            
            try
            {
                IUIPrinter uiPrinter = new UIPrinter(consoleLogger);

                IFileHandler fileHandler = new FileHandler();
                
                IFileParser fileParser = new HydrothermalVentFileParser(fileHandler, uiPrinter);

                MainLogic mainLogic = new MainLogic(fileParser, uiPrinter);

                await mainLogic.Run();

            }
            catch (Exception e)
            {
                consoleLogger.LogError(e.Message);
            }
        }
    }
}










