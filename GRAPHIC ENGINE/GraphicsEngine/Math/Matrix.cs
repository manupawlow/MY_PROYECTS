using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsEngine
{
    public class Matrix4x4
    {
        public double[][] Matrix { get; set; }

        //Dimensions
        public int n { get => Matrix.Length; }
        public int m { get => Matrix[0].Length; }

        public double[] this[int i]
        {
            get => Matrix[i];
            set => Matrix[i] = value;
        }
        public Matrix4x4(double[][] matrix)
        {
            Matrix = matrix;
        }
        public Matrix4x4() { }


        public static Matrix4x4 operator * (Matrix4x4 m1, Matrix4x4 m2) => m1.Multiply(m2);
        public static Matrix4x4 operator *(Matrix4x4 m, double n)
        {
            for(int i=0; i<m.n; i++)
                for(int j=0; j<m.m; j++)
                    m[i][j] *= n;
            return m;
        }
        public static Matrix4x4 Identity { get {
                var identity = new double[4][];
                identity[0] = new double[4] { 1, 0, 0, 0 };
                identity[1] = new double[4] { 0, 1, 0, 0 };
                identity[2] = new double[4] { 0, 0, 1, 0 };
                identity[3] = new double[4] { 0, 0, 0, 1 };
                return new Matrix4x4(identity);
        } }

        public static void ShowMatrix(Matrix4x4 m)
        {
            for (int i = 0; i < m.n; i++)
            {
                for (int j = 0; j < m.m; j++)
                    Console.Write(m[i][j].ToString("0.##").PadLeft(5, ' ') + " ");
                Console.Write("\n");
            }
        }

        public static Matrix4x4 TranslateMatrix(double tx = 0, double ty = 0, double tz = 0)
        {
            var translate = new double[4][];
            translate[0] = new double[4] { 1, 0, 0, tx };
            translate[1] = new double[4] { 0, 1, 0, ty };
            translate[2] = new double[4] { 0, 0, 1, tz };
            translate[3] = new double[4] { 0, 0, 0,  1 };

            return new Matrix4x4(translate);
        }

        public static Matrix4x4 ScaleMatrix(double scale) => ScaleMatrix(scale, scale, scale);
        public static Matrix4x4 ScaleMatrix(double sx = 1, double sy = 1, double sz = 1)
        {
            var translate = new double[4][];
            translate[0] = new double[4] { sx,  0,  0, 0 };
            translate[1] = new double[4] {  0, sy,  0, 0 };
            translate[2] = new double[4] {  0,  0, sz, 0 };
            translate[3] = new double[4] {  0,  0,  0, 1 };

            return new Matrix4x4(translate);
        }

        public static Matrix4x4 PitchRotationMatrix(double ax = 0)
        {
            //X AXIS
            var pitchRotation = new double[4][];
            pitchRotation[0] = new double[4] { 1,            0,             0,  0 };
            pitchRotation[1] = new double[4] { 0,  Math.Cos(ax), Math.Sin(ax),  0 };
            pitchRotation[2] = new double[4] { 0, -Math.Sin(ax), Math.Cos(ax),  0 };
            pitchRotation[3] = new double[4] { 0,            0,             0,  1 };

            return new Matrix4x4(pitchRotation);
        }

        public static Matrix4x4 YawRotationMatrix(double ay = 0)
        {
            //Z AXIS
            var yawRotation = new double[4][];
            yawRotation[0] = new double[4] { Math.Cos(ay), 0, -Math.Sin(ay), 0 };
            yawRotation[1] = new double[4] {            0, 1,             0, 0 };
            yawRotation[2] = new double[4] { Math.Sin(ay), 0,  Math.Cos(ay), 0 };
            yawRotation[3] = new double[4] {            0, 0,             0, 1 };

            return new Matrix4x4(yawRotation);
        }

        public static Matrix4x4 RollRotationMatrix(double az = 0)
        {
            //Z AXIS
            var yawRotation = new double[4][];
            yawRotation[0] = new double[4] {  Math.Cos(az), Math.Sin(az), 0, 0 };
            yawRotation[1] = new double[4] { -Math.Sin(az), Math.Cos(az), 0, 0 };
            yawRotation[2] = new double[4] {            0,             0, 1, 0 };
            yawRotation[3] = new double[4] {            0,             0, 0, 1 };

            return new Matrix4x4(yawRotation);
        }

        public static Matrix4x4 ProyectionMatrix(double visionRadAngle, double zNear, double zFar, int w, int h)
        {
            var cotan = 1 / Math.Tan(visionRadAngle / 2);
            double dz = zFar - zNear;

            double f = zFar;
            double n = zNear;

            var proyected = new double[4][];
            proyected[0] = new double[4] { 2.0 * zNear / w,               0,                  0, 0 };
            proyected[1] = new double[4] {               0, 2.0 * zNear / h,                  0, 0 };
            proyected[2] = new double[4] {               0,               0,          zFar / dz, 1 };
            proyected[3] = new double[4] {               0,               0, -zFar * zNear / dz, 0 };

            var proyected2 = new double[4][];
            proyected2[0] = new double[4] { cotan,      0,                     0,  0 };
            proyected2[1] = new double[4] {      0, cotan,                     0,  0 };
            proyected2[2] = new double[4] {      0,      0,      f + n / (f - n), -1.0 };
            proyected2[3] = new double[4] {      0,      0,  2 * f * n / (f - n),  0 };

            var proyected3 = new double[4][];
            proyected3[0] = new double[4] { (h * 1.0 / w) * cotan, 0, 0, 0 };
            proyected3[1] = new double[4] { 0, cotan, 0, 0 };
            proyected3[2] = new double[4] { 0, 0, f / dz, -f * n / dz };
            proyected3[3] = new double[4] { 0, 0, 1, 0 };

            return new Matrix4x4(proyected3);
        }

        public static Matrix4x4 OrtogonalProyectionMatrix()
        {
            var proyected = new double[4][];
            proyected[0] = new double[4] { 1, 0, 0, 0 };
            proyected[1] = new double[4] { 0, 1, 0, 0 };
            proyected[2] = new double[4] { 0, 0, 0, 0 };
            proyected[3] = new double[4] { 0, 0, 0, 1 };

            return new Matrix4x4(proyected);
        }

        public static Matrix4x4 OrtographicProyectionMatrix()
        {
            double r = 5;
            double l = -5;

            double t = 5;
            double b = 5;

            double zf = 10.0;
            double zn = 1.0;

            var proyected = new double[4][];
            proyected[0] = new double[4] { (r - l) / 2, 0, 0, (l + r) / 2 };
            proyected[1] = new double[4] { 0, (t - b) / 2, 0, (t + b) / 2 };
            proyected[2] = new double[4] { 0, 0, -(zf - zn) / 2, -(zf + zn) / 2 };
            proyected[3] = new double[4] { 0, 0, 0, 1 };

            return new Matrix4x4(proyected);
        }

        public Matrix4x4 Multiply(Matrix4x4 mtx)
        {
            // m1 (nxm) (mxp) 
            var m1 = this.Matrix;
            var m2 = mtx.Matrix;

            var n = this.n;
            var p = mtx.m;

            var m = this.m == mtx.n ? this.m : throw (new Exception($"Invalid dimensions ({n}x{m1[0].Length}) * ({m2.Length}x{p})"));
            //Console.WriteLine($"({n}x{m1[0].Length}) * ({m2.Length}x{p})");

            var Multiplied = new double[n][];

            for (int i = 0; i < n; i++)
                Multiplied[i] = new double[p];

            for (int i = 0; i < p; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                {
                    for (int k = 0; k < m; k++)
                    {
                        sum += m1[j][k] * m2[k][i];
                    }
                    Multiplied[j][i] = sum;
                    sum = 0;
                }
            }

            return new Matrix4x4(Multiplied);
        }

        public Matrix4x4 Inverse() 
        {
            var A2323 = Matrix[2][2] * Matrix[3][3] - Matrix[2][3] * Matrix[3][2];
            var A1323 = Matrix[2][1] * Matrix[3][3] - Matrix[2][3] * Matrix[3][1];
            var A1223 = Matrix[2][1] * Matrix[3][2] - Matrix[2][2] * Matrix[3][1];
            var A0323 = Matrix[2][0] * Matrix[3][3] - Matrix[2][3] * Matrix[3][0];
            var A0223 = Matrix[2][0] * Matrix[3][2] - Matrix[2][2] * Matrix[3][0];
            var A0123 = Matrix[2][0] * Matrix[3][1] - Matrix[2][1] * Matrix[3][0];
            var A2313 = Matrix[1][2] * Matrix[3][3] - Matrix[1][3] * Matrix[3][2];
            var A1313 = Matrix[1][1] * Matrix[3][3] - Matrix[1][3] * Matrix[3][1];
            var A1213 = Matrix[1][1] * Matrix[3][2] - Matrix[1][2] * Matrix[3][1];
            var A2312 = Matrix[1][2] * Matrix[2][3] - Matrix[1][3] * Matrix[2][2];
            var A1312 = Matrix[1][1] * Matrix[2][3] - Matrix[1][3] * Matrix[2][1];
            var A1212 = Matrix[1][1] * Matrix[2][2] - Matrix[1][2] * Matrix[2][1];
            var A0313 = Matrix[1][0] * Matrix[3][3] - Matrix[1][3] * Matrix[3][0];
            var A0213 = Matrix[1][0] * Matrix[3][2] - Matrix[1][2] * Matrix[3][0];
            var A0312 = Matrix[1][0] * Matrix[2][3] - Matrix[1][3] * Matrix[2][0];
            var A0212 = Matrix[1][0] * Matrix[2][2] - Matrix[1][2] * Matrix[2][0];
            var A0113 = Matrix[1][0] * Matrix[3][1] - Matrix[1][1] * Matrix[3][0];
            var A0112 = Matrix[1][0] * Matrix[2][1] - Matrix[1][1] * Matrix[2][0];

            var det = Matrix[0][0] * (Matrix[1][1] * A2323 - Matrix[1][2] * A1323 + Matrix[1][3] * A1223)
                - Matrix[0][1] * (Matrix[1][0] * A2323 - Matrix[1][2] * A0323 + Matrix[1][3] * A0223)
                + Matrix[0][2] * (Matrix[1][0] * A1323 - Matrix[1][1] * A0323 + Matrix[1][3] * A0123)
                - Matrix[0][3] * (Matrix[1][0] * A1223 - Matrix[1][1] * A0223 + Matrix[1][2] * A0123);
            det = 1 / det;
            var returnMatrix = new double[4][];
            returnMatrix[0] = new double[4];
            returnMatrix[1] = new double[4];
            returnMatrix[2] = new double[4];
            returnMatrix[3] = new double[4];

            returnMatrix[0][0] = det * (Matrix[1][1] * A2323 - Matrix[1][2] * A1323 + Matrix[1][3] * A1223);
            returnMatrix[0][1] = det * -(Matrix[0][1] * A2323 - Matrix[0][2] * A1323 + Matrix[0][3] * A1223);
            returnMatrix[0][2] = det * (Matrix[0][1] * A2313 - Matrix[0][2] * A1313 + Matrix[0][3] * A1213);
            returnMatrix[0][3] = det * -(Matrix[0][1] * A2312 - Matrix[0][2] * A1312 + Matrix[0][3] * A1212);
            returnMatrix[1][0] = det * -(Matrix[1][0] * A2323 - Matrix[1][2] * A0323 + Matrix[1][3] * A0223);
            returnMatrix[1][1] = det * (Matrix[0][0] * A2323 - Matrix[0][2] * A0323 + Matrix[0][3] * A0223);
            returnMatrix[1][2] = det * -(Matrix[0][0] * A2313 - Matrix[0][2] * A0313 + Matrix[0][3] * A0213);
            returnMatrix[1][3] = det * (Matrix[0][0] * A2312 - Matrix[0][2] * A0312 + Matrix[0][3] * A0212);
            returnMatrix[2][0] = det * (Matrix[1][0] * A1323 - Matrix[1][1] * A0323 + Matrix[1][3] * A0123);
            returnMatrix[2][1] = det * -(Matrix[0][0] * A1323 - Matrix[0][1] * A0323 + Matrix[0][3] * A0123);
            returnMatrix[2][2] = det * (Matrix[0][0] * A1313 - Matrix[0][1] * A0313 + Matrix[0][3] * A0113);
            returnMatrix[2][3] = det * -(Matrix[0][0] * A1312 - Matrix[0][1] * A0312 + Matrix[0][3] * A0112);
            returnMatrix[3][0] = det * -(Matrix[1][0] * A1223 - Matrix[1][1] * A0223 + Matrix[1][2] * A0123);
            returnMatrix[3][1] = det * (Matrix[0][0] * A1223 - Matrix[0][1] * A0223 + Matrix[0][2] * A0123);
            returnMatrix[3][2] = det * -(Matrix[0][0] * A1213 - Matrix[0][1] * A0213 + Matrix[0][2] * A0113);
            returnMatrix[3][3] = det * (Matrix[0][0] * A1212 - Matrix[0][1] * A0212 + Matrix[0][2] * A0112);

            return new Matrix4x4(returnMatrix);
        }
    }
}
