using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEngine.Graphics
{
    public class Light
    {
        public Light(Vector3 pos, EngineColor color)
        {
            Position = pos;

            AmbientColor = color;
            DiffuseColor = color;
            SpecularColor = color;
        }

        public Light(Vector3 pos, EngineColor ambient, EngineColor diffuse, EngineColor specular)
        {
            Position = pos;

            AmbientColor = ambient;
            DiffuseColor = diffuse;
            SpecularColor = specular;
        }

        public Vector3 Position { get; set; }

        public EngineColor AmbientColor { get; set; }
        public EngineColor DiffuseColor { get; set; }
        public EngineColor SpecularColor { get; set; }
    }
}
