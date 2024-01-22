using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSharpGl
{
    class Delaunay
    {        
        private Point a, b, c; 

        private List<Point> vertex = new List<Point>();

        public List<Triangle> triangle = new List<Triangle>();

        ////////////////
        //constructors//
        ////////////////

        public Delaunay()
        {
            
        }

        ///////////
        //methods//
        ///////////
        
        public void AddPoints(List<Point> points)
        {
            vertex = points;
        }        

        
        public void BaseIncremental()
        {
            double cx = vertex.Count / 2;
            double cy = vertex.Count / 2;
            double r = Math.Pow((Math.Pow(vertex.Count, 2) + Math.Pow(vertex.Count, 2)), 0.5) / 2; 
            
            a = new Point(cx - Math.Sqrt(3) * r, cy + r, 0);
            b = new Point(cx, cy - 2 * r, 0);
            c = new Point(cx + Math.Sqrt(3) * r, cy + r, 0);
            triangle.Add(new Triangle(a, b, c));
        }

        
        public void Incremental()
        {
            
            for (int i = 0; i < vertex.Count; i++)
            {
                List<Triangle> temporary_triangle = new List<Triangle>(0); 
                temporary_triangle.Clear();

                
                for (int j = 0; j < triangle.Count; j++)
                {
                    if (triangle[j].Center().DistanceTo(vertex[i]) < triangle[j].Radius() && triangle[j].A() != vertex[i] && triangle[j].B() != vertex[i] && triangle[j].C() != vertex[i])
                    {
                        temporary_triangle.Add(new Triangle(triangle[j].A(), triangle[j].B(), vertex[i]));
                        temporary_triangle.Add(new Triangle(triangle[j].B(), triangle[j].C(), vertex[i]));
                        temporary_triangle.Add(new Triangle(triangle[j].C(), triangle[j].A(), vertex[i]));
                        triangle.RemoveAt(j); 
                        j--;
                    }
                }
                
                for (int k = 0; k < i + 1; k++)
                {
                    for (int l = 0; l < temporary_triangle.Count; l++)
                    {
                        if (temporary_triangle[l].Center().DistanceTo(vertex[k]) <= temporary_triangle[l].Radius() && temporary_triangle[l].A() != vertex[k] && temporary_triangle[l].B() != vertex[k] && temporary_triangle[l].C() != vertex[k])
                        {
                            temporary_triangle.RemoveAt(l);
                            l--;
                        }
                    }
                }
                triangle.AddRange(temporary_triangle);
            }

            
            for (int i = 0; i < triangle.Count; i++)
            {
                if (triangle[i].A() == a || triangle[i].A() == b || triangle[i].A() == c || triangle[i].B() == a || triangle[i].B() == b || triangle[i].B() == c || triangle[i].C() == a || triangle[i].C() == b || triangle[i].C() == c)
                {
                    triangle.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < triangle.Count; i++)
            {
                for (int j = 0; j < triangle.Count; j++)
                {
                    if (i != j && triangle[i].A() + triangle[i].B() + triangle[i].C() == triangle[j].A() + triangle[j].B() + triangle[j].C())
                    {
                        triangle.RemoveAt(j);
                        j--;
                    }
                }
            }
        }
    }
}
