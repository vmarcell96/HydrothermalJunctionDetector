using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.Logic
{
    internal struct VentLine
    {
        //float takes up less space than double, and in the example we only use one number after the decimal point
        public (float, float) StartPoint;
        public (float, float) EndPoint;

        public override string ToString()
        {
            return $"StartPoint: ({StartPoint.Item1},{StartPoint.Item2}) EndPoint: ({EndPoint.Item1},{EndPoint.Item2}).";
        }
    }
}
