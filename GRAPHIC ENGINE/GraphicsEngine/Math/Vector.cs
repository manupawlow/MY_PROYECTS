using System;

namespace GraphicsEngine
{
    public class Vector3 : Matrix4x4
    {
        public Vector3(double x, double y, double z, double w = 1)
        {
            Matrix = new double[4][];
            Matrix[0] = new double[1] { x };
            Matrix[1] = new double[1] { y };
            Matrix[2] = new double[1] { z };
            Matrix[3] = new double[1] { w };
        }

        public static Vector3 operator + (Vector3 v1, Vector3 v2) => new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        public static Vector3 operator - (Vector3 v) => new Vector3(-v.X, -v.Y, -v.Z);
        public static Vector3 operator -(Vector3 v1, Vector3 v2) => v1 + (-v2);
        public static Vector3 operator / (Vector3 v, double n) => new Vector3(v.X / n, v.Y / n, v.Z / n);
        public static Vector3 operator * (Vector3 v, double n) => new Vector3(v.X * n, v.Y * n, v.Z * n);
        public static Vector3 operator * (Matrix4x4 m, Vector3 v)
        {
            var transformed = m.Multiply(v);

            return new Vector3(transformed.Matrix[0][0], transformed.Matrix[1][0], transformed.Matrix[2][0], transformed.Matrix[3][0]);
        }
        public static Vector3 Cross(Vector3 v1, Vector3 v2) => new Vector3 (v1.Y * v2.Z - v1.Z * v2.Y, v1.Z * v2.X - v1.X * v2.Z, v1.X * v2.Y - v1.Y * v2.X );
        public static double Dot(Vector3 v1, Vector3 v2) => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        public static Vector3 Origin => new Vector3(0, 0, 0);
        public static Vector3 VersorI => new Vector3(1, 0, 0);
        public static Vector3 VersorJ => new Vector3(0, 1, 0);
        public static Vector3 VersorK => new Vector3(0, 0, 1);

        public double Magnitud => Math.Sqrt(X*X + Y*Y + Z*Z);
        public static Vector3 UP => new Vector3(0, 1, 0);

        public Vector3 Copy() => new Vector3(X, Y, Z, W);
        public Vector3 Normalize() => this / this.Magnitud;


        public double X { get => Matrix[0][0]; set { Matrix[0][0] = value; } }
        public double Y { get => Matrix[1][0]; set { Matrix[1][0] = value; } }
        public double Z { get => Matrix[2][0]; set { Matrix[2][0] = value; } }
        public double W { get => Matrix[3][0]; set { Matrix[3][0] = value; } }


        public new Vector3 Multiply(Matrix4x4 transform)
        {
            var transformed = base.Multiply(transform);

            return new Vector3(transformed.Matrix[0][0], transformed.Matrix[1][0], transformed.Matrix[2][0]);
        }

        public static Vector3 VectorIntersectPlane(Vector3 plane_p, Vector3 plane_n, Vector3 lineStart, Vector3 lineEnd)
        {
            plane_n = plane_n.Normalize();
            var plane_d = -Dot(plane_n, plane_p);
            var ad = Dot(lineStart, plane_n);
            var bd = Dot(lineEnd, plane_n);
            var t = (-plane_d - ad) / (bd - ad);
            var lineStartToEnd = lineEnd - lineStart;
            var lineToIntersect = lineStartToEnd * t;
            return lineStart + lineToIntersect;
        }

    }
}
