using GraphicsEngine.Meshes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GraphicsEngine
{
    public abstract class BaseGraphicsEngine
    {
        private const int FRAME_RATE = 1000 / 60;

        public bool GAME_OVER { get; set; } = false;

        public Camera Camera { get; set; }
        public Gpu GPU { get; set; }

        public List<Mesh> Meshes { get; set; }
        public int elapsedTime { get; set; } = 0;

        public void Start()
        {
            Init();

            while (!GAME_OVER)
            {
                Update();

                Thread.Sleep(50);

                Render();

                Thread.Sleep(FRAME_RATE);

                elapsedTime++;
            }
        }

        public abstract void Init();
        public abstract void Update();
        public abstract void Render();
    }
}
