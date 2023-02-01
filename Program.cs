using HydrothermalJunctionDetector.Logic;
using HydrothermalJunctionDetector.UI;
using HydrothermalVentFileParser;
using HydrothermalVentFileParser.Logging;
using HydrothermalVentFileParser.Persistence;
using HydrothermalVentFileParser.UI;

namespace HydrothermalJunctionDetector
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ILogger consoleLogger = new ConsoleLogger();

            IUIPrinter uiPrinter = new UIPrinter(consoleLogger);

            IFileHandler fileHandler = new FileHandler();

            try
            {

                MainLogic mainLogic = new MainLogic(fileHandler, uiPrinter);

                await mainLogic.Run();

            }
            catch (Exception e)
            {
                consoleLogger.LogError(e.Message);
            }
        }
    }
}










