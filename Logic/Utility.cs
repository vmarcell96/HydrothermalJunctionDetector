using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrothermalJunctionDetector.Logic
{
    public class Utility
    {
        public static (float, float)? GetInterSectionOfSegments((float, float) pointA, (float, float) pointB, (float, float) pointC, (float, float) pointD)
        {
            /*
            If we have an intersection point these two equations must be satisfied
            
            Linear interpolation equations
            have to find out u and t values
            1. Ix = Ax(Bx-Ax)*t = Cx+(Dx-Cx)*u    
            2. IY = Ay(By-Ay)*t = Cy+(Dy-Cy)*u   
            
            3. Ax+(Bx-Ax)*t = Cx+(Dx-Cx)*u /-Cx
            4. (Ax-Cx)+(Bx-Ax)*t = (Dx-Cx)*u 
    
            5. Ay(By-Ay)*t = Cy+(Dy-Cy)*u  /-Cy
            6. (Ay-Cy)+(By-Ay)*t = (Dy-Cy)*u / *(Dx-Cx)
            7. (Dx-Cx)(Ay-Cy)+(Dx-Cx)(By-Ay)*t = (Dy-Cy)(Dx-Cx)*u  -- (Dx-Cx)*u will be switched to the value in line 4.
            7. (Dx-Cx)(Ay-Cy)+(Dx-Cx)(By-Ay)*t = (Dy-Cy)(Ax-Cx)+(Dy-Cy)(Bx-Ax)*t /-(Dy-Cy)(Ax-Cx)   /-(Dx-Cx)(By-Ay)*t  -- we want t on one side
            8. (Dx-Cx)(Ay-Cy)-(Dy-Cy)(Ax-Cx) = (Dy-Cy)(Bx-Ax)*t-(Dx-Cx)(By-Ay)*t
            8. (Dx-Cx)(Ay-Cy)-(Dy-Cy)(Ax-Cx) = ((Dy-Cy)(Bx-Ax)-(Dx-Cx)(By-Ay))*t /:((Dy-Cy)(Bx-Ax)-(Dx-Cx)(By-Ay))
            9. ((Dx-Cx)(Ay-Cy)-(Dy-Cy)(Ax-Cx))/((Dy-Cy)(Bx-Ax)-(Dx-Cx)(By-Ay)) = t
    
            10. top = (Dx-Cx)(Ay-Cy)-(Dy-Cy)(Ax-Cx)
            11. bottom = (Dy-Cy)(Bx-Ax)-(Dx-Cx)(By-Ay)
    
            12. t = top/bottom

            13. We have to know the value of u too bacause otherwise its an intersection of a line and a section
            14. the bottom part will be the same for u
    
            */

            float tTop = (pointD.Item1 - pointC.Item1) * (pointA.Item2 - pointC.Item2) - (pointD.Item2 - pointC.Item2) * (pointA.Item1 - pointC.Item1);
            float uTop = (pointC.Item2 - pointA.Item2) * (pointA.Item1 - pointB.Item1) - (pointC.Item1 - pointA.Item1) * (pointA.Item2 - pointB.Item2);
            float bottom = (pointD.Item2 - pointC.Item2) * (pointB.Item1 - pointA.Item1) - (pointD.Item1 - pointC.Item1) * (pointB.Item2 - pointA.Item2);

            // if bottom is 0 the segments are parallel
            if (bottom != 0)
            {
                float t = tTop / bottom;
                float u = uTop / bottom;
                // if t and u are between 0 and 1, they are on the segments
                if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
                {
                    (float, float) intersection = (LinearInterpolation(pointA.Item1, pointB.Item1, t), LinearInterpolation(pointA.Item2, pointB.Item2, t));
                    return intersection;
                }
            }

            return null;


        }

        private static float LinearInterpolation(float coord1, float coord2, float t)
        {
            return coord1 + (coord2 - coord1) * t;
        }

        public static (int, int)[] FindIntegerLineSegmentPoints(int X1, int Y1, int X2, int Y2)
        {
            (int, int)[] points;
            //Segment is horizontal, Y1 = Y2
            if (Y1 == Y2)
            {
                points = new (int, int)[Math.Abs(X1 - X2)+1];
                for (int i = 0; i < points.Length; i++)
                {
                    if (X1 > X2)
                    {
                        points[i] = (X1 - i, Y1);
                    }
                    else
                    {
                        points[i] = (X1 + i, Y1);
                    }
                }
            }
            //Segment is vertical, X1 = X2
            else if (X1 == X2)
            {
                points = new (int, int)[Math.Abs(Y1 - Y2)+1];
                for (int i = 0; i < points.Length; i++)
                {
                    if (Y1 > Y2)
                    {
                        points[i] = (X1, Y1 - i);
                    }
                    else
                    {
                        points[i] = (X1, Y1 + i);
                    }
                }
            }
            //Line was validated before so it can only be diagonal
            //X and Y coordinates are both changing going from the start point to the end point
            else
            {
                points = new (int, int)[Math.Abs(Y1 - Y2)+1];
                for (int i = 0; i < points.Length; i++)
                {
                    int x, y;
                    if (Y1 > Y2)
                    {
                        y = Y1 - i;
                    }
                    else
                    {
                        y = Y1 + i;
                    }
                    if (X1 > X2)
                    {
                        x = X1 - i;
                    }
                    else
                    {
                        x = X1 + i;
                    }
                    points[i] = (x, y);
                }
            }
            return points;
        }
        

    }
}
