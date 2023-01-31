using HydrothermalJunctionDetector.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.Tests
{
    public class UtilityTests
    {
        [Fact]
        public void FindIntegerLineSegmentPointsReturnsPointsOfSegment()
        {
            var result = Utility.FindIntegerLineSegmentPoints(1, 1, 4, 4);
            var expected = new (int, int)[] { (1, 1), (2, 2), (3, 3), (4, 4) };
            Assert.Equal(expected, result);
        }

        //[Theory]
        //[InlineData(Array.CreateInstance((int), 4), 1, 1, 4, 4)]
        //public void FindIntegerLineSegmentPointsReturnsPointsOfSegmentTheory(
        //    (int, int)[] expected, int intA, int intB, int intC, int intD)
        //{
        //    var result = Utility.FindIntegerLineSegmentPoints(intA, intB, intC, intD);
        //    Assert.Equal(expected, result);
        //}
    }
}
