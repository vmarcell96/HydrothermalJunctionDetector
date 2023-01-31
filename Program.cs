// See https://aka.ms/new-console-template for more information
using HydrothermalJunctionDetector;
using HydrothermalJunctionDetector.Logic;
using HydrothermalJunctionDetector.Persistence;
using HydrothermalJunctionDetector.UI;
using System.Diagnostics;

ILogger consoleLogger = new ConsoleLogger();
IFileHandler fileHandler = new FileHandler();
IUIPrinter uiPrinter = new UIPrinter(consoleLogger);
IFileParser fileParser = new HydrothermalVentFileParser(fileHandler, uiPrinter);

MainLogic mainLogic = new MainLogic(fileParser, uiPrinter);





try
{
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();
    mainLogic.Run("default");
    stopWatch.Stop();
    // Get the elapsed time as a TimeSpan value.
    TimeSpan ts = stopWatch.Elapsed;
    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
    Console.WriteLine("RunTime " + elapsedTime);
    //Test not valid txt file
    //var points = fileParser.ParseFile(@"..\..\..\InputFileLineSegments_NotValid.txt");

    //var ventLines = fileParser.ParseFile();
    //foreach (var item in ventLines)
    //{
    //    Console.WriteLine(item.ToString());
    //}
    //var crossingPoints = mainLogic.CalculateCrossingPoints(ventLines);
    //mainLogic.ReportCrossingPoints(crossingPoints);

}
catch (Exception e)
{

    Console.WriteLine(e.Message);
}

