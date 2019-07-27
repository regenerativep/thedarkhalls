using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine.Physics
{
    /// <summary>
    /// represents a link between a physics object and a game object
    /// </summary>
    public class GameObjectLink
    {
        /// <summary>
        /// the linked game object
        /// </summary>
        public GameObject GameObject;
        /// <summary>
        /// the linked physics object
        /// </summary>
        public PhysicsBasedObject PhysicsObject;
        /// <summary>
        /// creates a new link between a game object and a physics object
        /// </summary>
        /// <param name="gameObject">the game object to link</param>
        /// <param name="physicsObject">the physics object to link</param>
        public GameObjectLink(GameObject gameObject, PhysicsBasedObject physicsObject)
        {
            GameObject = gameObject;
            PhysicsObject = physicsObject;
        }
        /// <summary>
        /// updates the game object to match the physics object
        /// </summary>
        public void Update()
        {
            Vector2 newPosition = new Vector2(PhysicsObject.Position.X, PhysicsObject.Position.Y);
            if(GameObject.Sprite != null)
            {
                //newPosition += GameObject.Sprite.Offset;
            }
            GameObject.Position = newPosition;
        }
    }
}
