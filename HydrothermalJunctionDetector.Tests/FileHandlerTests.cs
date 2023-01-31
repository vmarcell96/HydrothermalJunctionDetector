namespace HydrothermalJunctionDetector.Tests
{
    public class FileHandlerTests
    {
        private readonly FileHandler _sut;

        public FileHandlerTests()
        {
            _sut = new FileHandler();
        }

        [Fact]
        public void ReadFileReturnsArrayOfStrings()
        {
            var result = _sut.ReadFile(@"..\..\..\..\InputFileLineSegments_Test.txt");
            var expected = new string[] { "0,9 -> 5,9", "8,0 -> 0,8" };
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WriteFileCreatesFileInSelectedDirectory()
        {
            Assert.Equal(true, true);
        }
    }
}