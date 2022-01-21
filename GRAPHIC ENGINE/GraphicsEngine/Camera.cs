using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEngine
{
    public class Camera
    {
        public Vector3 Position { get; set; }
        public Vector3 LookAt { get; set; }
        public Vector3 UP { get; set; }

        public Camera(Vector3 pos, Vector3 lAt, Vector3 up)
        {
            Position = pos;
            LookAt = lAt;
            UP = up;
        }

        public Camera(Vector3 pos)
        {
            Position = pos;
            LookAt = -Vector3.VersorK;
            UP = Vector3.VersorJ;
        }

        public Matrix4x4 LookAtRH()
        {
            var eye = Position;
            var target = LookAt;
            var up = UP;

            var zaxis = (eye - target).Normalize();    // The "forward" vector.
            var xaxis = (Vector3.Cross(up, zaxis)).Normalize();// The "right" vector.
            var yaxis = Vector3.Cross(zaxis, xaxis);     // The "up" vector.

            // Create a 4x4 orientation matrix from the right, up, and forward vectors
            // This is transposed which is equivalent to performing an inverse 
            // if the matrix is orthonormalized (in this case, it is).

            var m = new double[4][];
            m[0] = new double[4] { xaxis.X, xaxis.Y, xaxis.Z, 0 };
            m[1] = new double[4] { yaxis.X, yaxis.Y, yaxis.Z, 0 };
            m[2] = new double[4] { zaxis.X, zaxis.Y, zaxis.Z, 0 };
            m[3] = new double[4] { 0, 0, 0, 1 };

            var orientation = new Matrix4x4(m);

            // Create a 4x4 translation matrix.
            // The eye position is negated which is equivalent
            // to the inverse of the translation matrix. 
            // T(v)^-1 == T(-v)
            var translation = Matrix4x4.TranslateMatrix(-eye.X, -eye.Y, -eye.Z);

            // Combine the orientation and translation to compute 
            // the final view matrix. Note that the order of 
            // multiplication is reversed because the matrices
            // are already inverted.
            return (orientation * translation);
        }
    }
}
