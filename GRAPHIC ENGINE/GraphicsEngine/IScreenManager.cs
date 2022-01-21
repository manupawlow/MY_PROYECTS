using GraphicsEngine.Graphics;

namespace GraphicsEngine
{
    public interface IScreenManager
    {
        void Init();
        void Render();

        int Width();
        int Height();

        //void DrawPoint(int x, int y, EngineColor color);
        void DrawLine(int x1, int y1, int x2, int y2, EngineColor color);
        void FillTriangle(int x1, int y1, int x2, int y2, int x3, int y3, EngineColor color);
        void ClearScreen();
        bool IsClosed();
    }
}
