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
            
            try
            {
                IUIPrinter uiPrinter = new UIPrinter(consoleLogger);

                IFileHandler fileHandler = new FileHandler();
                
                IFileParser fileParser = new HydrothermalFileParser(fileHandler, uiPrinter);

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










