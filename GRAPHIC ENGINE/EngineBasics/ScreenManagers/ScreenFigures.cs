using System.Drawing;

namespace EngineBasics.ScreenManagers
{
    public class _Line
    {
        public int Id { get; set; }

        public _Line(PointF p1, PointF p2, Color color, int id)
        {
            P1 = p1;
            P2 = p2;
            Color = color;

            //Id = id;
            Id = (int)(p1.X * p2.X - p1.X * p1.Y - p1.Y * p1.X + p2.Y * p1.X);
        }

        public PointF P1 { get; set; }
        public PointF P2 { get; set; }
        public Color Color { get; set; }
    }

    public class _Triangle
    {
        public _Triangle(PointF p1, PointF p2, PointF p3, Color color)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            Color = color;
        }

        public PointF P1 { get; set; }
        public PointF P2 { get; set; }
        public PointF P3 { get; set; }
        public Color Color { get; set; }

        public PointF Center => new PointF((P1.X + P2.X + P3.X) / 3, (P1.Y + P2.Y + P3.Y) / 3);
    }
}
