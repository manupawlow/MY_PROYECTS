using GraphicsEngine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEngine.Meshes
{
    public class Vertex
    {
        public Vector3 Position { get; set; }
        public EngineColor Color { get; set; }

        public Vertex(double x, double y, double z, EngineColor color)
        {
            Position = new Vector3(x, y, z);
            Color = color;
        }
        public Vertex(Vector3 v, EngineColor color)
        {
            Position = v.Copy();
            Color = color;
        }

        public Vertex Copy() => new Vertex(Position.X, Position.Y, Position.Z, Color.Copy());

        public static Vertex Origin  => new Vertex(Vector3.Origin, EngineColor.Black); 
    }
}
