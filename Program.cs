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
                bool isQuitRequested = false;
                while (!isQuitRequested)
                {
                    uiPrinter.ClearConsole();
                    await mainLogic.Run();
                    while (true)
                    {
                        Console.WriteLine("Do you want to parse another file? Choose Y if yes, press ANY other key to quit.");
                        var response = Console.ReadKey();
                        if (response.Key == ConsoleKey.Y)
                        {
                            break;
                        }
                        else
                        {
                            isQuitRequested= true;
                            break;
                        }
                    }
                }
                uiPrinter.ClearConsole();
                uiPrinter.PrintLine("GoodBye!");
                

            }
            catch (Exception e)
            {
                consoleLogger.LogError(e.Message);
            }
        }
    }
}










