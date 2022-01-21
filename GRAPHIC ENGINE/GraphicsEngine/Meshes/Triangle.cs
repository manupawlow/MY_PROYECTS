using GraphicsEngine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEngine.Meshes
{
    public class Triangle
    {
        public Vertex V1 { get; set; }
        public Vertex V2 { get; set; }
        public Vertex V3 { get; set; }

        public Vector3 Center { get => CenterTriangle(); }
        public Vector3 Normal { get => NormalVector(); }
        //public Vector3 UpdatedNormal { get; set; }

        public EngineColor Color 
            => new EngineColor((V1.Color.R + V2.Color.R + V3.Color.R) / 3, (V1.Color.G + V2.Color.G + V3.Color.G) / 3, (V1.Color.B + V2.Color.B + V3.Color.B) / 3);


        public Triangle Copy() => new Triangle(V1.Copy(), V2.Copy(), V3.Copy());

        public Triangle(Vertex v1, Vertex v2, Vertex v3)
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
        }

        public Triangle(Vector3 v1, Vector3 v2, Vector3 v3, EngineColor color)
        {
            V1 = new Vertex(v1, color);
            V2 = new Vertex(v2, color);
            V3 = new Vertex(v3, color);
        }

        public Vector3 CenterTriangle()
        {

            var x = (V1.Position.X + V2.Position.X + V3.Position.X) / 3;
            var y = (V1.Position.Y + V2.Position.Y + V3.Position.Y) / 3;

            var nearZ = Math.Min(Math.Min(V1.Position.Z, V2.Position.Z), V3.Position.Z);
            var farZ = Math.Max(Math.Max(V1.Position.Z, V2.Position.Z), V3.Position.Z);

            return new Vector3(x, y, (farZ + nearZ) / 2);
        }

        public Vector3 NormalVector()
        {
            var normal = (Vector3.Cross(V2.Position - V1.Position, V3.Position - V1.Position) + new Vector3(0, 0, -1)).Normalize();

            return normal;
        }
    }
}
