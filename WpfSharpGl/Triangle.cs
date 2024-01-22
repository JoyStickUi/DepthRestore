using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSharpGl
{
    class Triangle
    {
        //////////////
        //properties//
        //////////////

        private Point point_a;
        private Point point_b;
        private Point point_c;

        private double x1, y1, x2, y2, x3, y3;

        ////////////////
        //constructors//
        ////////////////

        public Triangle
            (
                Point _point_a,
                Point _point_b,
                Point _point_c
            )
        {
            point_a = _point_a;
            point_b = _point_b;
            point_c = _point_c;

            x1 = point_a.x;
            y1 = point_a.y;
            x2 = point_b.x;
            y2 = point_b.y;
            x3 = point_c.x;
            y3 = point_c.y;
        }

        ///////////
        //methods//
        ///////////        

        public Point Center()
        {
            double cn = 2 * ((x2 - x1) * (y3 - y1) - (y2 - y1) * (x3 - x1));
            double x = ((y3 - y1) * (x2 * x2 - x1 * x1 + y2 * y2 - y1 * y1) + (y1 - y2) * (x3 * x3 - x1 * x1 + y3 * y3 - y1 * y1)) / cn;
            double y = ((x1 - x3) * (x2 * x2 - x1 * x1 + y2 * y2 - y1 * y1) + (x2 - x1) * (x3 * x3 - x1 * x1 + y3 * y3 - y1 * y1)) / cn;

            Point center = new Point(x, y, 0);
            return center;
        }

        public double Radius()
        {
            Point center = Center();

            double radius = center.DistanceTo(point_a);
            return radius;
        }

        public Point A()
        {
            return point_a;
        }

        public Point B()
        {
            return point_b;
        }

        public Point C()
        {
            return point_c;
        }
    }
}
