using GraphicsEngine;
using GraphicsEngine.Graphics;
using GraphicsEngine.Meshes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EngineBasics.Meshes
{
    public class Sphere : Mesh
    {
        public override void Init() { }

        public override void Update(int elapsedTime) { }

        public Sphere(int radius, int tessellation, EngineColor color) : base()
        {
            Triangles = new List<Triangle>();

            tessellation = 2 * tessellation;

            var positions = new Vector3[tessellation + 1][];

            for (int m = 0; m < tessellation + 1; m++)
            {
                positions[m] = new Vector3[tessellation + 1];

                for (int n = 0; n < tessellation + 1; n++)
                {
                    var x = Math.Sin(Math.PI * m / tessellation) * Math.Cos(2 * Math.PI * n / tessellation);
                    var y = Math.Sin(Math.PI * m / tessellation) * Math.Sin(2 * Math.PI * n / tessellation);
                    var z = Math.Cos(Math.PI * m / tessellation);

                    positions[m][n] = new Vector3(x, y, z) * radius;
                }
            }

            var vertexs = new List<Vector3>();

            for (int i = 0; i < tessellation; i++)
            {
                for (int j = 0; j < tessellation + 1; j++)
                {
                    vertexs.Add(positions[i][j]);
                    vertexs.Add(positions[i + 1][j]);
                }
            }

            var flag = true;
            for (int i = 0; i < vertexs.Count() - 2; i++)
            {
                var v1 = vertexs[i + 0].Copy();
                var v2 = vertexs[i + 1].Copy();
                var v3 = vertexs[i + 2].Copy();

                var t = flag ? 
                    new Triangle(new Vertex(v1, color), new Vertex(v2, color), new Vertex(v3, color)) 
                    : new Triangle(new Vertex(v3, color), new Vertex(v2, color), new Vertex(v1, color));

                flag = !flag;

                Triangles.Add(t);
            }

        }


    }
}
