using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine.Physics
{
    public class MovingObject : PhysicsBasedObject
    {
        public Vector2 Velocity;
        public float AngularVelocity;
        public Vector2 SpeedLimit;
        public bool TouchingSurface;
        public float Angle
        {
            get
            {
                return angle;
            }
            set
            {
                angle = value;
                angleMatrix.X = (float)Math.Cos(angle);
                angleMatrix.Y = (float)Math.Sin(angle);
            }
        }
        private float angle;
        public float Mass;
        private Vector2 angleMatrix;
        public MovingObject(Vector2[] vertices, Vector2 position, float mass = 1) : base(vertices, position)
        {
            Mass = mass;
            Velocity = new Vector2(0, 0);
            AngularVelocity = 0;
            angleMatrix = new Vector2(0, 0);
            angle = 0;
            SpeedLimit = new Vector2(-1);
            TouchingSurface = false;
        }
        public override Vector2 GetVertex(int num)
        {
            Vector2 actual = vertices[num];
            return new Vector2(actual.X * angleMatrix.X - actual.Y * angleMatrix.Y, actual.X * angleMatrix.Y + actual.Y * angleMatrix.X);
        }
        public override void Update()
        {
            int velSign;
            if (SpeedLimit.X >= 0)
            {
                velSign = Math.Sign(Velocity.X);
                if (Velocity.X * velSign > SpeedLimit.X)
                {
                    Velocity.X = SpeedLimit.X * velSign;
                }
            }
            if (SpeedLimit.Y >= 0)
            {
                velSign = Math.Sign(Velocity.Y);
                if (Velocity.Y * velSign > SpeedLimit.Y)
                {
                    Velocity.Y = SpeedLimit.Y * velSign;
                }
            }
            Position += Velocity;
            Angle += AngularVelocity;
        }
    }
}
