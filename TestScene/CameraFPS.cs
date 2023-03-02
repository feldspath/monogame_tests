using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestScene
{
    internal class CameraFPS
    {
        private Transform transform;
        private float yaw;
        private float pitch;

        private Matrix viewMatrix;
        private Matrix projectionMatrix;
        private Matrix frameMatrix;


        public CameraFPS(Vector3 position, float aspectRatio)
        {
            transform = new Transform();
            transform.position = position;
            yaw = 0;
            pitch = 0;
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), aspectRatio, 1f, 1000f);

            ComputeViewMatrix();
        }

        private void ComputeViewMatrix()
        {
            transform.rotation = Quaternion.CreateFromYawPitchRoll(yaw, pitch, 0f);

            frameMatrix = transform.Frame();
            viewMatrix = Matrix.Invert(frameMatrix);
        }

        public Matrix GetProjectionMatrix()
        {
            return projectionMatrix;
        }

        public Matrix GetViewMatrix()
        {
            return viewMatrix;
        }

        public void Move(Vector3 direction)
        {
            transform.position += transform.ToGlobal(direction);
            ComputeViewMatrix();
        }


        public void SetPosition(Vector3 newPosition)
        {
            transform.position = newPosition;
            ComputeViewMatrix();
        }


        public Transform Transform() { return transform; }

        public void Rotate(float dYaw, float dPitch)
        {
            yaw += dYaw;
            pitch += dPitch;

            // Clamp pitch
            if (pitch > 89f)
            {
                pitch = 89f;
            }
            else if (pitch < -89f)
            {
                pitch = -89f;
            }

            ComputeViewMatrix();
        }
    }
}
