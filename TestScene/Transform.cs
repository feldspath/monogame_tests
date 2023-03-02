using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestScene
{
    internal class Transform
    {
        public Vector3 position;
        public Quaternion rotation;
        public float scale;

        public Transform()
        {
            position = Vector3.Zero;
            rotation = Quaternion.Identity;
            scale = 1f;
        }

        public Matrix Frame()
        {
            Matrix s = Matrix.Identity * scale;
            Matrix t = Matrix.CreateTranslation(position);
            Matrix r = Matrix.CreateFromQuaternion(rotation);
            return s * r * t;
        }

        public Vector3 Up()
        {
            return Vector3.Transform(Vector3.Up, Frame());
        }

        public Vector3 Right()
        {
            return Vector3.Transform(Vector3.Right, Frame());
        }

        public Vector3 Front()
        {
            return Vector3.Transform(Vector3.Forward, Frame());
        }

        public Vector3 ToLocal(Vector3 v)
        {
            return Vector3.TransformNormal(v, Matrix.Invert(Frame()));
        }

        public Vector3 ToGlobal(Vector3 v)
        {
            return Vector3.TransformNormal(v, Frame());
        }
    }
}
