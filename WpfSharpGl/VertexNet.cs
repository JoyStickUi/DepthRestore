using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Web.UI;

namespace WpfSharpGl
{
    public class VertexNet
    {

        private List<float> heights;
        // Distance - distance between centers of photos
        private double distance;
        public List<Pair> LPoints;
        public List<Pair> RPoints;
        public VertexNet()
        {
        }

        public List<float> GetHeights()
        {
            return heights;
        }

        public bool Generate(List<Pair> LPoints, List<Pair> RPoints, double distance)
        {
            this.LPoints = LPoints;
            this.RPoints = RPoints;
            if (LPoints != null && RPoints != null && LPoints.Count == RPoints.Count)
            {
                this.distance = distance;
                heights = CalculateTrianglesHeights(
                    ProcessPointsList(LPoints),
                    ProcessPointsList(RPoints)
                );
                return true;
            }
            return false;
        }

        private List<Pair> ProcessPointsList(List<Pair> points)
        {
            List<Pair> resultList = new List<Pair>();
            foreach (Pair point in points)
            {
                double az = Math.Sqrt(1 - Math.Pow((double)point.First, 2)); // z for alpha                

                double a = Math.Atan(az / (double)point.First); // alpha angle                
                resultList.Add(new Pair(a, point.Second));
            }
            return resultList;
        }

        private List<float> CalculateTrianglesHeights(List<Pair> LDirAnglesList, List<Pair> RDirAnglesList)
        {
            List<float> heights = new List<float>();

            if (LDirAnglesList.Count == RDirAnglesList.Count)
            {
                for (int i = 0; i < LDirAnglesList.Count; ++i)
                {
                    Vector2 v_base = new Vector2(-(float)distance, 0);

                    float maxVectorLength = 2f;

                    for (float r = .001f; r < maxVectorLength; r+=.001f)
                    {
                        float x = (float)(r * Math.Cos((double)LDirAnglesList[i].First));
                        float y = (float)(r * Math.Sin((double)LDirAnglesList[i].First));
                        Vector2 v_dir = new Vector2(x, y);
                        Vector2 v_opposite = v_base + v_dir;

                        double opposite_cos = 1f - (Vector2.Dot(v_opposite, v_base) / (v_opposite.Length() * v_base.Length()));
                        
                        if (
                            Math.Abs(opposite_cos - Math.Cos((double)RDirAnglesList[i].First)) < 0.005f
                        )
                        {
                            heights.Add(r);
                            break;
                        }
                    }
                }
            }

            return heights;
        }
    }
}