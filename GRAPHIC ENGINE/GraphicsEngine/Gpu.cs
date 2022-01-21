using GraphicsEngine.Graphics;
using GraphicsEngine.Meshes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEngine
{
    public class Gpu
    {
        public const bool DRAW_MESH = true;
        public const bool DRAW_LINES = !true;
        public const bool DRAW_LIGHT = !true;
        public const bool DRAW_NORMAL = !true;

        public IScreenManager ScreenManager { get; set; }

        public Camera Camera { get; set; }
        public List<Light> Lights { get; set; }

        public List<Triangle> Triangles { get; set; }

        public Gpu(Camera camera, IScreenManager screenManager)
        {
            ScreenManager = screenManager;

            Camera = camera;

            Lights = new List<Light>();

            Triangles = new List<Triangle>();
        }

        public void Init()
        {
            ScreenManager.Init();
        }

        public void RenderSceen()
        {
            Console.Clear();

            ScreenManager.ClearScreen();

            var worldToViewmatrix = Camera.LookAtRH();

            var viewToProyectionMatrix = Matrix4x4.ProyectionMatrix(Math.PI * 60 / 180.0, 10, 100, ScreenManager.Width(), ScreenManager.Height());
            //var viewToProyectionMatrix = Matrix4x4.OrtographicProyectionMatrix();
            //var viewToProyectionMatrix = Matrix4x4.Identity;

            foreach (var t in Triangles)
            {
                //var v1 = t.V1.Position.Copy();
                //var v2 = t.V2.Position.Copy();
                //var v3 = t.V3.Position.Copy();

                //v1 = worldToViewmatrix * v1;
                //v2 = worldToViewmatrix * v2;
                //v3 = worldToViewmatrix * v3;

                var vViewed1 = new Vertex(worldToViewmatrix * t.V1.Position, t.V1.Color);
                var vViewed2 = new Vertex(worldToViewmatrix * t.V2.Position, t.V2.Color);
                var vViewed3 = new Vertex(worldToViewmatrix * t.V3.Position, t.V3.Color);

                var viewedTri = new Triangle(vViewed1, vViewed2, vViewed3);

                var clipped = new Triangle[2];

                var nClippedTirangles = TirangleClipAgainstPlane(new Vector3(0, 0, 5), new Vector3(0, 0, -1), viewedTri, ref clipped[0], ref clipped[1]);

                for(int n = 0; n < nClippedTirangles; n++)
                {
                    var vProj1 = AdjustToView(viewToProyectionMatrix * clipped[n].V1.Position);
                    var vProj2 = AdjustToView(viewToProyectionMatrix * clipped[n].V2.Position);
                    var vProj3 = AdjustToView(viewToProyectionMatrix * clipped[n].V3.Position);
                    
                    if (DRAW_MESH)
                    {
                        var color = EngineColor.Black;

                        foreach (var light in Lights)
                        {
                            var dotProduct = Vector3.Dot(t.Normal, (light.Position).Normalize());

                            color += t.Color * (1 - dotProduct);

                            if (DRAW_LIGHT)
                            {
                                var center = AdjustToView(viewToProyectionMatrix * worldToViewmatrix * t.Center);

                                var light_pos = AdjustToView(viewToProyectionMatrix * worldToViewmatrix * light.Position);

                                var normalColor = EngineColor.Yellow;
                                ScreenManager.DrawLine((int)center.X, (int)center.Y, (int)light_pos.X, (int)light_pos.Y, normalColor);
                            }

                            //PHONG SHADING
                            //var Ka = 1;
                            //var Kd = 1;
                            //var Ks = 1;
                            //var shininess = 1;

                            //var nDotL = Vector3.Dot(t.Normal, light.Position + t.Center);
                            //var rDotV = 1;

                            //color += Ka * light.AmbientColor;
                            //color += Kd * light.DiffuseColor * nDotL;
                            //color += Ks * light.SpecularColor * Math.Pow(rDotV, shininess);
                        }

                        ScreenManager.FillTriangle((int)vProj1.X, (int)vProj1.Y, (int)vProj2.X, (int)vProj2.Y, (int)vProj3.X, (int)vProj3.Y, color);
                    }

                    if (DRAW_LINES)
                    {
                        var boxColor = EngineColor.Yellow;
                        ScreenManager.DrawLine((int)vProj2.X, (int)vProj2.Y, (int)vProj3.X, (int)vProj3.Y, boxColor);
                        ScreenManager.DrawLine((int)vProj1.X, (int)vProj1.Y, (int)vProj2.X, (int)vProj2.Y, boxColor);
                        ScreenManager.DrawLine((int)vProj3.X, (int)vProj3.Y, (int)vProj1.X, (int)vProj1.Y, boxColor);
                    }

                    if (DRAW_NORMAL)
                    {
                        var center = AdjustToView(viewToProyectionMatrix * worldToViewmatrix * t.Center);
                        var normal = AdjustToView(viewToProyectionMatrix * worldToViewmatrix * (t.Center + t.Normal).Normalize());

                        var normalColor = EngineColor.Yellow;
                        ScreenManager.DrawLine((int)center.X, (int)center.Y, (int)normal.X, (int)normal.Y, normalColor);
                    }
                }
            }

            ScreenManager.Render();

            Triangles.Clear();
        }

        private int TirangleClipAgainstPlane(Vector3 plane_p, Vector3 plane_n, Triangle in_tri, ref Triangle out_tri1, ref Triangle out_tri2)
        {
            plane_n = plane_n.Normalize();

            double dist(Vector3 p) => plane_n.X * p.X + plane_n.Y * p.Y + plane_n.Z * p.Z - Vector3.Dot(plane_n, plane_p);

            var inside_points = new Vector3[3]; int nInsidePointCount = 0;
            var outside_points = new Vector3[3]; int nOutsidePointCount = 0;

            var d0 = dist(in_tri.V1.Position);
            var d1 = dist(in_tri.V2.Position);
            var d2 = dist(in_tri.V3.Position);

            if (d0 >= 0)
                inside_points[nInsidePointCount++] = in_tri.V1.Position.Copy();
            else
                outside_points[nOutsidePointCount++] = in_tri.V1.Position.Copy();
            if (d1 >= 0)
                inside_points[nInsidePointCount++] = in_tri.V2.Position.Copy();
            else
                outside_points[nOutsidePointCount++] = in_tri.V2.Position.Copy();
            if (d2 >= 0)
                inside_points[nInsidePointCount++] = in_tri.V3.Position.Copy();
            else
                outside_points[nOutsidePointCount++] = in_tri.V3.Position.Copy();

            if(nInsidePointCount == 0) { return 0; }

            if (nInsidePointCount == 3) { out_tri1 = in_tri; return 1; }

            if(nInsidePointCount == 1 && nOutsidePointCount == 2)
            {
                var v1 = new Vertex(inside_points[0], in_tri.Color);

                var v2 = new Vertex(Vector3.VectorIntersectPlane(plane_p, plane_n, inside_points[0], outside_points[0]), in_tri.Color);
                var v3 = new Vertex(Vector3.VectorIntersectPlane(plane_p, plane_n, inside_points[0], outside_points[1]), in_tri.Color);

                out_tri1 = new Triangle(v1, v2, v3);

                return 1;
            }

            if (nInsidePointCount == 2 && nOutsidePointCount == 1)
            {
                var v11 = new Vertex(inside_points[0], in_tri.Color);
                var v12 = new Vertex(inside_points[1], in_tri.Color);

                var v13 = new Vertex(Vector3.VectorIntersectPlane(plane_p, plane_n, inside_points[0], outside_points[0]), in_tri.Color);

                out_tri1 = new Triangle(v11, v12, v13);

                var v21 = new Vertex(inside_points[0], in_tri.Color);
                var v22 = new Vertex(out_tri1.V2.Position, in_tri.Color);

                var v23 = new Vertex(Vector3.VectorIntersectPlane(plane_p, plane_n, inside_points[1], outside_points[0]), in_tri.Color);

                out_tri2 = new Triangle(v21, v22, v23);

                return 2;
            }

            return 3;
        }



        private Vector3 AdjustToView(Vector3 vector)
        {
            var w = ScreenManager.Width();
            var h = ScreenManager.Height();

            var v = vector.Copy();

            v.X /= v.W;
            v.Y /= v.W;
            v.Z /= v.W;

            v.X += 1.0;
            v.Y += 1.0;
            v.X *= 0.5 * w;
            v.Y *= 0.5 * h;

            return v;
        }
    }
}