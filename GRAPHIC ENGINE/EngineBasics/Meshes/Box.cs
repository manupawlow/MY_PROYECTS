using GraphicsEngine;
using GraphicsEngine.Graphics;
using GraphicsEngine.Meshes;
using System;
using System.Collections.Generic;

namespace EngineBasics.Meshes
{
    public class Box : Mesh
    {
        public override void Init() { }

        public override void Update(int elapsedTime) { }

        public Box(double a, double b, double c, EngineColor color)
        {
            Triangles = new List<Triangle>();

            //Model Space
            var x = a / 2;
            var y = b / 2;
            var z = c / 2;

            var v1 = new Vertex(-x, y, z, color);
            var v2 = new Vertex(x, -y, z, color);
            var v3 = new Vertex(x, y, z, color);
            Triangles.Add(new Triangle(v1, v2, v3));//Front 1

            v1 = new Vertex(-x, y, z, color);
            v2 = new Vertex(-x, -y, z, color);
            v3 = new Vertex(x, -y, z, color);
            Triangles.Add(new Triangle(v1, v2, v3));//Front 2

            v1 = new Vertex(x, y, z, color);
            v2 = new Vertex(x, -y, -z, color);
            v3 = new Vertex(x, y, -z, color);
            Triangles.Add(new Triangle(v1, v2, v3));//Right 1

            v1 = new Vertex(x, y, z, color);
            v2 = new Vertex(x, -y, z, color);
            v3 = new Vertex(x, -y, -z, color);
            Triangles.Add(new Triangle(v1, v2, v3));//Right 2

            v1 = new Vertex(x, y, -z, color);
            v2 = new Vertex(x, -y, -z, color);
            v3 = new Vertex(-x, -y, -z, color);
            Triangles.Add(new Triangle(v1, v2, v3));//Back 1

            v1 = new Vertex(x, y, -z, color);
            v2 = new Vertex(-x, -y, -z, color);
            v3 = new Vertex(-x, y, -z, color);
            Triangles.Add(new Triangle(v1, v2, v3));//Back 2

            v1 = new Vertex(-x, y, -z, color);
            v2 = new Vertex(-x, -y, z, color);
            v3 = new Vertex(-x, y, z, color);
            Triangles.Add(new Triangle(v1, v2, v3));//Left 1

            v1 = new Vertex(-x, y, -z, color);
            v2 = new Vertex(-x, -y, -z, color);
            v3 = new Vertex(-x, -y, z, color);
            Triangles.Add(new Triangle(v1, v2, v3));//Left 2

            v1 = new Vertex(x, y, z, color);
            v2 = new Vertex(x, y, -z, color);
            v3 = new Vertex(-x, y, z, color);
            Triangles.Add(new Triangle(v1, v2, v3));//UP 1

            v1 = new Vertex(-x, y, z, color);
            v2 = new Vertex(x, y, -z, color);
            v3 = new Vertex(-x, y, -z, color);
            Triangles.Add(new Triangle(v1, v2, v3));//UP 2

            v1 = new Vertex(x, -y, z, color);
            v2 = new Vertex(-x, -y, z, color);
            v3 = new Vertex(-x, -y, -z, color);
            Triangles.Add(new Triangle(v1, v2, v3));//DOWN 1

            v1 = new Vertex(x, -y, z, color);
            v2 = new Vertex(-x, -y, -z, color);
            v3 = new Vertex(x, -y, -z, color);
            Triangles.Add(new Triangle(v1, v2, v3));//DOWN 2
        }
    }
}
