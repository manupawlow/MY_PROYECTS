using GraphicsEngine;
using GraphicsEngine.Graphics;
using GraphicsEngine.Meshes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EngineBasics.Meshes
{
    public static class MeshGenerator
    {
        public static Box[] GenerateWall(int nx, int ny, EngineColor color, int a = 1, int b = 1, int c = 1)
        {
            var boxes = new Box[nx * ny];
            for(int i = 0; i < ny; i++)
            {
                for(int j = 0; j < nx; j++)
                {
                    boxes[j + i * nx] = new Box(a, b, c, color); 
                }
            }

            return boxes;
        }

        public static Mesh MeshFromTxt(string path)
        {
            var triangles = new List<Triangle>();

            var vertexs = new List<Vertex>();

            var formatProvider = new System.Globalization.NumberFormatInfo() { NumberDecimalSeparator = "." };

            foreach (var line in File.ReadLines(path))
            {
                //V1:5,0,0,255,0,0
                //T1:V1,V2,V3
                if (!string.IsNullOrEmpty(line))
                {
                    var values = line.Split(' ');

                    switch (values[0][0])
                    {
                        case 'v':
                            var r = 255;//onvert.ToInt16(values[3]);
                            var g = 0;//onvert.ToInt16(values[4]);
                            var b = 0;//Convert.ToInt16(values[5]);

                            var x = Convert.ToDouble(values[1], formatProvider);
                            var y = Convert.ToDouble(values[2], formatProvider);
                            var z = Convert.ToDouble(values[3], formatProvider);

                            var v = new Vertex(x, y, z, new EngineColor(r, g, b));

                            vertexs.Add(v);

                            break;

                        case 'f':

                            var index2 = Convert.ToInt16(values[2].Split('/')[0]) - 1;
                            var index3 = Convert.ToInt16(values[3].Split('/')[0]) - 1;
                            var index1 = Convert.ToInt16(values[1].Split('/')[0]) - 1;

                            var t = new Triangle(vertexs[index1], vertexs[index2], vertexs[index3]);

                            triangles.Add(t);

                            break;

                        default:
                            break;
                    }
                }
            }

            return new MeshInstance(triangles);
        }

        public static MeshInstance MeshFromTxt2(string path)
        {
            var triangles = new List<Triangle>();

            var vertexs = new List<Vertex>();

            foreach (var line in File.ReadLines(path))
            {
                //V1:5,0,0,255,0,0
                //T1:V1,V2,V3

                var split = line.Split(':');

                var values = split[1].Split(',');

                switch (split[0][0])
                {
                    case 'V':
                        var r = Convert.ToInt16(values[3]);
                        var g = Convert.ToInt16(values[4]);
                        var b = Convert.ToInt16(values[5]);

                        var x = Convert.ToInt16(values[0]);
                        var y = Convert.ToInt16(values[1]);
                        var z = Convert.ToInt16(values[2]);

                        var v = new Vertex(x, y, z, new EngineColor(r, g, b));

                        vertexs.Add(v);

                        break;

                    case 'T':

                        var index1 = Convert.ToInt16(values[0].Remove(0, 1)) - 1;
                        var index2 = Convert.ToInt16(values[1].Remove(0, 1)) - 1;
                        var index3 = Convert.ToInt16(values[2].Remove(0, 1)) - 1;

                        var t = new Triangle(vertexs[index1], vertexs[index2], vertexs[index3]);

                        triangles.Add(t);

                        break;

                    default:
                        break;
                }

                //var values = line.Split(',');

                //var r = Convert.ToInt16(values[9]);
                //var g = Convert.ToInt16(values[10]);
                //var b = Convert.ToInt16(values[11]);

                //var color = new EngineColor(r, g, b);

                //var x1 = Convert.ToInt16(values[0]);
                //var y1 = Convert.ToInt16(values[1]);
                //var z1 = Convert.ToInt16(values[2]);

                //var v1 = new Vertex(x1, y1, z1, color);

                //var x2 = Convert.ToInt16(values[3]);
                //var y2 = Convert.ToInt16(values[4]);
                //var z2 = Convert.ToInt16(values[5]);

                //var v2 = new Vertex(x2, y2, z2, color);

                //var x3 = Convert.ToInt16(values[6]);
                //var y3 = Convert.ToInt16(values[7]);
                //var z3 = Convert.ToInt16(values[8]);

                //var v3 = new Vertex(x3, y3, z3, color);

                //triangles.Add(new Triangle(v1, v2, v3));
            }

            return new MeshInstance(triangles);
        }
    }
}
