using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestScene
{
    internal class Player
    {
        private Transform transform;
        private Vector3 velocity;
        private float speed;
        
        public Player(Vector3 position)
        {
            transform = new Transform();
            transform.position = position;
            velocity = Vector3.Zero;
            speed = 20f;
        }

        public void SetMoveDirection(Vector3 localDirection)
        {
            if (localDirection == Vector3.Zero)
            {
                velocity = Vector3.Zero;
            }
            else
            {
                localDirection.Normalize();
                velocity = transform.ToGlobal(localDirection) * speed;
            }
        }

        public Transform Transform() { return transform; }

        public Vector3 Velocity() { return velocity; }

        public void Update(float dt)
        {
            transform.position += dt * velocity;
        }

        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public bool CheckCollision(Model model)
        {
            return false;
        }
    }
}
