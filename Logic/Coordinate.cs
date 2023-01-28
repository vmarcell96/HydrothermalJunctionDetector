using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.Logic
{
    internal struct Coordinate
    {
        //float takes up less space than double, and in the example we only use one number after the decimal point
        public float X;
        public float Y;
    }
}
