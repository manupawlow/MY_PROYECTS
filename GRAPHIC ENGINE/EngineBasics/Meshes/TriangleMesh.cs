using GraphicsEngine;
using GraphicsEngine.Graphics;
using GraphicsEngine.Meshes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineBasics.Meshes
{
    public class TriangleMesh : Mesh
    {
        public TriangleMesh(double x, double y, EngineColor color) : base()
        {
            Triangles = new List<Triangle>();

            var v1 = new Vertex(0, y / 2, 0, color);
            var v2 = new Vertex(x / 2, -y / 2, 0, color);
            var v3 = new Vertex(-x / 2, -y / 2, 0, color);

            Triangles.Add(new Triangle(v1, v2, v3));
        }

        public override void Init()
        {
            
        }

        public override void Update(int elapsedTime)
        {
            var scale = 10;
            WorldState.Scale = new Vector3(scale, scale, scale);

            var translationX = 0;
            var translationY = 0;
            var translationZ = 20;
            WorldState.Traslation = new Vector3(translationX, translationY, translationZ);

            var rotationX = 0 * Math.PI / 64;//90 * Math.Sin(elapsedTime * 0.05);
            var rotationY = 1 * Math.PI / 32; //Math.Sin(elapsedTime * 0.05 * Math.PI / 16);
            var rotationZ = 0 * Math.PI / 64;
            WorldState.Rotation += new Vector3(rotationX, rotationY, rotationZ);
            WorldState.Rotation.X = WorldState.Rotation.X % (2 * Math.PI);
            WorldState.Rotation.Y = WorldState.Rotation.Y % (2 * Math.PI);
            WorldState.Rotation.Z = WorldState.Rotation.Z % (2 * Math.PI);
        }
    }
}
