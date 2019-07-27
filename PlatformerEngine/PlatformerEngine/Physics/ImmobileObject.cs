using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine.Physics
{
    public class ImmobileObject : PhysicsBasedObject
    {
        public float Friction;
        public ImmobileObject(Vector2[] vertices, Vector2 position) : base(vertices, position)
        {
            Friction = 0.1f;
        }
    }
}
