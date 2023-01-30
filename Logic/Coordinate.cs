using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.Logic
{
    internal struct Coordinate
    {
        //float takes up less space than double
        public float X;
        public float Y;

        public Coordinate(float x, float y)
        {
            X = x; Y = y;
        }
    }
}
