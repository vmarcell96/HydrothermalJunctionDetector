using HydrothermalJunctionDetector.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.Logic
{
    internal class MainLogic
    {
        private readonly IFileParser _fileParser;
        private readonly IUIPrinter _uiPrinter;

        public MainLogic(IFileParser fileParser, IUIPrinter uiPrinter)
        {
            _fileParser = fileParser;
            _uiPrinter = uiPrinter;
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
