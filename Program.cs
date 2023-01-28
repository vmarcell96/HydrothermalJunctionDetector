// See https://aka.ms/new-console-template for more information
using HydrothermalJunctionDetector;
using HydrothermalJunctionDetector.Logic;
using HydrothermalJunctionDetector.Persistence;
using HydrothermalJunctionDetector.UI;

Console.WriteLine("Hello, World!");

ILogger consoleLogger = new ConsoleLogger();
IFileHandler fileHandler = new FileHandler();
IFileParser fileParser = new VentFileParser(fileHandler);
IUIPrinter uiPrinter = new UIPrinter(consoleLogger);

IMainLogic mainLogic = new MainLogic(fileParser, uiPrinter);

mainLogic.GetInputFileLocation();
mainLogic.ValidateFile();
