using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSharpGl
{
    class Point
    {
        public double x, y, h;

        public Point(double a = 0, double b = 0)
        {
            x = a;
            y = b;
        }

        public Point(double a = 0, double b = 0, double c = 0)
        {
            x = a;
            y = b;
            h = c;
        }

        public double DistanceTo(Point p)
        {
            return Math.Sqrt(
                Math.Pow(this.x - p.x, 2) +
                Math.Pow(this.y - p.y, 2)
                );
        }

        public static Point operator+(Point a, Point b)
        {
            return new Point(
                a.x + b.x,
                a.y + b.y
                );
        }
    }
}
