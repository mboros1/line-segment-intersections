using System;
using System.Windows;
using System.Collections;
using System.IO;
using System.Linq;
class Solution
{

    // This program takes 2 line segments defined by 4 coordinates for one line segment, and 2 coordinates for the second that has one endpoint anchored at the origin

    static void Main(String[] args)
    {
        // takes keyboard input to set n, the number of test cases
        int n = int.Parse(Console.ReadLine());

        for (int i = 0; i < n; i++)
        {

            // next input is 6 numbers, the first 2 are one endpoint of the line segment, the second 2 are the other endpoint. The last 2 coordinates are the other endpoint
            // of the line segment anchored at the origin. So basically the lines look like this:
            // (X1,Y1)------(X2,Y2),    and  
            // (0,0)------(Xm, Ym)
            double[] coords = Array.ConvertAll(Console.ReadLine().Split(' '), double.Parse);
            double X1 = coords[0];
            double Y1 = coords[1];
            double X2 = coords[2];
            double Y2 = coords[3];
            double Xm = coords[4];
            double Ym = coords[5];

            // answer is set to false, if the lines intersect it is changed to true
            bool answer = false;

            // the cross product is needed to check if the lines are parallel (the case where crossproduct = 0). If they are, some equations will break down
            double crossproduct = (Xm * (Y2 - Y1)) - (Ym * (X2 - X1));

            // this is the standard case, where neither line is vertical so they both have a slope, and the lines are not parallel since the crossproduct != 0
            if (X1 != X2 && Xm != 0 && crossproduct != 0)
            {
                // this is an adaption from a hackerrank problem about 'lasers' and 'walls', so for future reference the line segment anchored at the origin will be
                // referred to as a 'laser', the other line segment a 'wall'.
                // following standard line notation, y = M*x+B, all slopes will be referred to as M, and y-intercepts as B. These are the equations for the lines
                // that contain the line segments being considered. 
                double laserM = (Ym) / (Xm);
                double wallM = (Y2 - Y1) / (X2 - X1);
                double laserB = 0;
                double wallB = -1 * wallM * X1 + Y1;

                // here we find the point where the 2 lines intersect. This is where the equations break down if the cross product is 0; in that case, the lines will
                // be parallel, so both lines will have equal slopes, therefore laserM = wallM and laserM - wallM = 0, creating a situation where we divide by 0.
                double XOfIntercept = (wallB - laserB) / (laserM - wallM);
                double YOfIntercept = laserM * XOfIntercept;

                // this long string of inequalities, and statements, and so on is a logic test to see if the point of intercept is contained inside of the both line segments               
                if (
                (XOfIntercept >= Math.Min(X2, X1) && XOfIntercept <= Math.Max(X2, X1) && XOfIntercept <= Math.Max(Xm, 0) && XOfIntercept >= Math.Min(Xm, 0)) &&
                (YOfIntercept >= Math.Min(Y2, Y1) && YOfIntercept <= Math.Max(Y2, Y1) && YOfIntercept <= Math.Max(Ym, 0) && YOfIntercept >= Math.Min(Ym, 0))
                )
                {
                    answer = true;
                }
            }

            // this is the case if the 'wall' line segment is vertical.
            else if (X1 == X2)
            {
                // Instead of relying on a point of intercept, we just rely on the fact that the 'wall' is a vertical line and check to see if any piece of it is between
                // the two points of the 'laser'
                if (Math.Min(Xm, 0) <= X1 && Math.Max(Xm, 0) >= X1 && Math.Min(0, Ym) <= Math.Min(Y1, Y2) && Math.Max(0, Ym) >= Math.Max(Y2, Y1))
                {
                    answer = true;
                }
            }
            // this is the case that the 'laser' is a vertical line segment
            else if (Xm == 0)
            {
                // pretty much the same inequality as the case of the vertical 'wall', just reversing the order of the points
                if (Math.Min(X2, X1) <= 0 && Math.Max(X2, X1) >= 0 && Math.Min(0, Ym) <= Math.Min(Y1, Y2) && Math.Max(0, Ym) >= Math.Max(Y2, Y1))
                {
                    answer = true;
                }
            }

            // this is the case where the 2 lines are parallel.
            else if (crossproduct == 0)
            {
                double wallM = (Y2 - Y1) / (X2 - X1);
                double wallB = -1 * wallM * X1 + Y1;

                // since the 'laser' has a point fixed on the origin, we know its y-intercept is always 0. Therefore, if the 'wall's y-intercept (wallB) is
                // not 0 then we know they cannot possibly intercept. So we just check for the case where wallB==0
                if (wallB == 0)
                {
                    // we know that both the 'wall' and 'laser' are on lines with identical slope, passing through the origin. Now we are just checking for
                    // the cases where the line segments may have some overlap
                    if ((Math.Min(Xm, 0) <= Math.Min(X2, X1) && Math.Max(Xm, 0) >= Math.Min(X2, X1)) ||
                       (Math.Min(X1, X2) <= Math.Min(Xm, 0) && Math.Max(X1, X2) >= Math.Min(Xm, 0)) ||
                        (Math.Min(Xm, 0) <= Math.Min(X2, X1) && Math.Max(0, Xm) >= Math.Max(X2, X1))
                        )
                    {
                        answer = true;
                    }
                }
            }

            if (answer)
            {
                Console.WriteLine("NO");
            }
            else
            {
                Console.WriteLine("YES");
            }
        }
    }
}
