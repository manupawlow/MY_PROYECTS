using GraphicsEngine.Meshes;
using System;
using System.Collections.Generic;

namespace GraphicsEngine.Meshes
{
    public abstract class Mesh
    {
        protected Mesh()
        {
            WorldState = new WorldState();
        }

        public List<Triangle> Triangles { get; set; }
        public WorldState WorldState { get; set; }

        public abstract void Init();
        public abstract void Update(int elapsedTime);

        public Matrix4x4 GetTranslationMatrix() => Matrix4x4.TranslateMatrix(WorldState.Traslation.X, WorldState.Traslation.Y, WorldState.Traslation.Z);
        public Matrix4x4 GetScaleMatrix() => Matrix4x4.ScaleMatrix(WorldState.Scale.X, WorldState.Scale.Y, WorldState.Scale.Z);
        public Matrix4x4 GetRotationMatrix() => Matrix4x4.PitchRotationMatrix(WorldState.Rotation.X) * Matrix4x4.YawRotationMatrix(WorldState.Rotation.Y) * Matrix4x4.RollRotationMatrix(WorldState.Rotation.Z);
    }

    public class MeshInstance : Mesh
    {
        public MeshInstance(List<Triangle> triangles) : base()
        {
            Triangles = triangles;
        }

        public override void Init() { }

        public override void Update(int elapsedTime) { }
    }

    public class WorldState
    {
        public Vector3 Traslation { get; set; }
        public Vector3 Scale { get; set; }
        public Vector3 Rotation { get; set; }

        public WorldState()
        {
            Traslation = new Vector3(0, 0, 0);
            Scale = new Vector3(1, 1, 1);
            Rotation = new Vector3(0, 0, 0);
        }
    }
}