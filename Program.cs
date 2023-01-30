// See https://aka.ms/new-console-template for more information
using HydrothermalJunctionDetector;
using HydrothermalJunctionDetector.Logic;
using HydrothermalJunctionDetector.Persistence;
using HydrothermalJunctionDetector.UI;


ILogger consoleLogger = new ConsoleLogger();
IFileHandler fileHandler = new FileHandler();
IFileParser fileParser = new HydrothermicVentFileParser(fileHandler);
IUIPrinter uiPrinter = new UIPrinter(consoleLogger);

IMainLogic mainLogic = new MainLogic(fileParser, uiPrinter);





try
{

    //Test not valid txt file
    var ventLines = fileParser.ParseFile(@"..\..\..\InputFileLineSegments_NotValid.txt");

    //var ventLines = fileParser.ParseFile();
    //foreach (var item in ventLines)
    //{
    //    Console.WriteLine(item.ToString());
    //}
    var crossingPoints = mainLogic.CalculateCrossingPoints(ventLines);
    mainLogic.ReportCrossingPoints(crossingPoints);


    
}
catch (Exception e)
{

    Console.WriteLine(e.Message);
}

