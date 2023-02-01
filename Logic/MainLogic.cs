using HydrothermalVentFileParser.UI;
using HydrothermalVentFileParser;
using HydrothermalVentFileParser.Persistence;

namespace HydrothermalJunctionDetector.Logic
{
    internal class MainLogic
    {
        private readonly IFileParser _fileParser;

        public MainLogic(IFileHandler fileHandler, IUIPrinter uiPrinter)
        {
            _fileParser = new HydrothermalFileParser(fileHandler, uiPrinter);
        }

        public async Task Run()
        {
            try
            {
                await _fileParser.ParseFileParallelAsync();
            }
            catch (TaskCanceledException)
            {
                throw;
            }

        }

    }
}
