using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlpool.Core.Render;

namespace Whirlpool.Core.Scene
{
    public class Transform
    {
        public string name;
        public Mesh mesh;
        public Vector3 position;
        public Vector3 scale;
        public Quaternion rotation;
        public Quaternion localRotation;
        public Texture texture;
        public Material material;

        public EventHandler onRender;
        public EventHandler onUpdate;

        public Transform(string name, Mesh mesh, Vector3 position, Vector3 scale, Quaternion rotation, Quaternion localRotation, Texture texture, Material material)
        {
            this.name = name;
            this.mesh = mesh;
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
            this.localRotation = localRotation;
            this.texture = texture;
            this.material = material;
        }

        public virtual void Render()
        {
            Render3D.DrawMesh(mesh, position, scale, rotation, localRotation, texture, material);
            onRender?.Invoke(this, null);
        }

        public virtual void Update()
        {
            onUpdate?.Invoke(this, null);
        }
    }
}
