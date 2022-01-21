using System;
using System.Collections.Generic;
using EngineBasics.Meshes;
using EngineBasics.ScreenManagers;
using GraphicsEngine;
using GraphicsEngine.Graphics;
using GraphicsEngine.Meshes;

namespace TestEngine
{
    class TestGraphicsEngine : BaseGraphicsEngine
    {
        IScreenManager ScreenManager { get; set; }

        //MESHES
        Box box;
        Sphere sphere;
        TriangleMesh tri;
        MeshInstance instance;

        public override void Init()
        {
            Meshes = new List<Mesh>();

            Camera = new Camera(Vector3.Origin);

            ScreenManager = new ConsoleScreen(0, 0);
            ScreenManager = new FormScreen(700, 500);

            GPU = new Gpu(Camera, ScreenManager);
            GPU.Init();

            var light = new Light(new Vector3(10, 20, 10), EngineColor.Yellow);
            var light2 = new Light(new Vector3(-10, -20, -10), EngineColor.Yellow);
            GPU.Lights.Add(light);
            //GPU.Lights.Add(light2);

            box = new Box(1, 1, 1, EngineColor.Red);
            sphere = new Sphere(3, 6, EngineColor.Pink);
            tri = new TriangleMesh(1, 1, EngineColor.White);
            instance = MeshGenerator.MeshFromTxt2("diamond_mesh.txt");

            Meshes.Add(sphere);
            /*
            Meshes.Add(box);
            Meshes.Add(instance);
            Meshes.Add(tri);
            */
        }

        public override void Update()
        {

            BoxUpdate(box);
            SphereUpdate(sphere);
            InstanceUpdate(instance);

            foreach (var mesh in Meshes)
            {
                mesh.Update(elapsedTime);

                var translationMatrix = mesh.GetTranslationMatrix();
                var scaleMatrix = mesh.GetScaleMatrix();
                var rotationMatrix = mesh.GetRotationMatrix();

                var externalTransform = Matrix4x4.Identity;

                var modelToWorldMatrix = translationMatrix * rotationMatrix * scaleMatrix * externalTransform;

                var worldTriangles = new List<Triangle>();

                foreach(var t in mesh.Triangles)
                {
                    var v1 = new Vertex(modelToWorldMatrix * t.V1.Position, t.V1.Color);
                    var v2 = new Vertex(modelToWorldMatrix * t.V2.Position, t.V2.Color);
                    var v3 = new Vertex(modelToWorldMatrix * t.V3.Position, t.V3.Color);

                    var newT = new Triangle(v1, v2, v3);

                    var dotProduct = Vector3.Dot(newT.Normal, newT.Center - Camera.Position);

                    //if (newT.Normal.Z > 0)
                    if (dotProduct < 0)
                        worldTriangles.Add(newT);
                }

                //SEND TRIANGLES IN WORLD SPACE TO GPU
                GPU.Triangles.AddRange(worldTriangles);
            }

            GAME_OVER = ScreenManager.IsClosed();
        }

        public override void Render()
        {
            GPU.RenderSceen();
        }

        private void BoxUpdate(Box box)
        {
            var scale = 10;
            box.WorldState.Scale = new Vector3(scale, scale, scale);

            var translationX = 10;
            var translationY = 0;
            var translationZ = -50 + 1 * elapsedTime;
            box.WorldState.Traslation = new Vector3(translationX, translationY, translationZ);

            var rotationX = 0 * Math.PI / 64;//90 * Math.Sin(elapsedTime * 0.05);
            var rotationY = 1 * Math.PI / 32; //Math.Sin(elapsedTime * 0.05 * Math.PI / 16);
            var rotationZ = 0 * Math.PI / 64;
            box.WorldState.Rotation += new Vector3(rotationX, rotationY, rotationZ);
        }

        private void SphereUpdate(Sphere sphere)
        {
            var scale = 5;
            sphere.WorldState.Scale = new Vector3(scale, scale, scale);

            var translationX = -20;
            var translationY = 0;
            var translationZ = -60;
            sphere.WorldState.Traslation = new Vector3(translationX, translationY, translationZ);

            var rotationX = 0;
            var rotationY = 1 * Math.PI / 64;
            var rotationZ = 0;
            sphere.WorldState.Rotation += new Vector3(rotationX, rotationY, rotationZ);
        }

        private void InstanceUpdate(MeshInstance instance)
        {
            var scale = 15;
            instance.WorldState.Scale = new Vector3(scale, scale, scale);

            var translationX = 0;
            var translationY = 0;
            var translationZ = -60;
            instance.WorldState.Traslation = new Vector3(translationX, translationY, translationZ);

            var rotationX = 0;
            var rotationY = 1 * Math.PI / 64;
            var rotationZ = 0;
            instance.WorldState.Rotation += new Vector3(rotationX, rotationY, rotationZ);
        }
    }
}
