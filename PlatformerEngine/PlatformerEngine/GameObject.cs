using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    /// <summary>
    /// a (non programming) object that can be put inside the game world
    /// </summary>
    public abstract class GameObject
    {
        /// <summary>
        /// collision precision
        /// </summary>
        public static float CollisionPrecision = 1;
        /// <summary>
        /// the position of the object
        /// </summary>
        public Vector2 Position;
        /// <summary>
        /// the sprite of the object
        /// </summary>
        public SpriteData Sprite;
        /// <summary>
        /// the room with the object
        /// </summary>
        public Room Room;
        /// <summary>
        /// velocity of the object
        /// </summary>
        public Vector2 Velocity;
        /// <summary>
        /// if this is currently touching some other block
        /// </summary>
        public bool Touching;
        /// <summary>
        /// if we dont remove this if we are switching rooms
        /// </summary>
        public bool Persistent;
        /// <summary>
        /// initializes part of the game object
        /// </summary>
        /// <param name="room">the room with the object</param>
        /// <param name="position">the position of the object</param>
        /// <param name="velocity">the velocity of the object</param>
        public GameObject(Room room, Vector2 position)
        {
            Room = room;
            Position = position;
            Persistent = false;
            Touching = false;
            Sprite = new SpriteData();
        }
        /// <summary>
        /// updates this game object
        /// </summary>
        public virtual void Update()
        {
            Touching = false;
            if (Velocity.X != 0 || Velocity.Y != 0)
            {
                for (int i = 0; i < Room.GameObjectList.Count; i++)
                {
                    GameObject obj = Room.GameObjectList[i];
                    if (obj.GetType().IsEquivalentTo(PEngine.GetTypeFromName("obj_block")))
                    {
                        //feel like there should be a better way to do collisions but idk that way
                        Rectangle targetRect = obj.GetHitbox();
                        targetRect.Location += obj.Position.ToPoint();
                        Rectangle fromRect = PlatformerMath.AddVectorToRect(GetHitbox(), Position, PlatformerMath.VectorCeil(new Vector2(0, Velocity.Y)));
                        while (PlatformerMath.RectangleInRectangle(fromRect, targetRect))
                        {
                            Touching = true;
                            if (Math.Abs(Velocity.Y) < CollisionPrecision)
                            {
                                Velocity.Y = 0;
                                break;
                            }
                            else
                            {
                                Velocity.Y -= Math.Sign(Velocity.Y) * CollisionPrecision;
                                fromRect = PlatformerMath.AddVectorToRect(GetHitbox(), Position, PlatformerMath.VectorCeil(new Vector2(0, Velocity.Y)));
                            }
                        }
                        fromRect = PlatformerMath.AddVectorToRect(GetHitbox(), Position, PlatformerMath.VectorCeil(new Vector2(Velocity.X, 0)));
                        while (PlatformerMath.RectangleInRectangle(fromRect, targetRect))
                        {
                            Touching = true;
                            if (Math.Abs(Velocity.X) < CollisionPrecision)
                            {
                                Velocity.X = 0;
                                break;
                            }
                            else
                            {
                                Velocity.X -= Math.Sign(Velocity.X) * CollisionPrecision;
                                fromRect = PlatformerMath.AddVectorToRect(GetHitbox(), Position, PlatformerMath.VectorCeil(new Vector2(Velocity.X, 0)));
                            }
                        }
                        fromRect = PlatformerMath.AddVectorToRect(GetHitbox(), Position, PlatformerMath.VectorCeil(Velocity));
                        while (PlatformerMath.RectangleInRectangle(fromRect, targetRect))
                        {
                            Touching = true;
                            if (Math.Abs(Velocity.X) < CollisionPrecision)
                            {
                                Velocity.X = 0;
                                break;
                            }
                            else
                            {
                                Velocity.X -= Math.Sign(Velocity.X) * CollisionPrecision;
                            }
                            if (Math.Abs(Velocity.Y) < CollisionPrecision)
                            {
                                Velocity.Y = 0;
                                break;
                            }
                            else
                            {
                                Velocity.Y -= Math.Sign(Velocity.Y) * CollisionPrecision;
                            }
                            fromRect = PlatformerMath.AddVectorToRect(GetHitbox(), Position, PlatformerMath.VectorCeil(Velocity));
                        }
                    }
                }
            }
            Position += Velocity;
            Sprite?.Update();
        }
        /// <summary>
        /// draws this game object
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to draw to</param>
        /// <param name="viewPosition">the offset to match the view position</param>
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 viewPosition)
        {
            Sprite?.Draw(spriteBatch, Position - viewPosition);
        }
        /// <summary>
        /// loads assets for this object
        /// </summary>
        /// <param name="assets">the asset manager to load from</param>
        public virtual void Load(AssetManager assets)
        {

        }
        /// <summary>
        /// gets the hitbox made up of position and sprite size
        /// </summary>
        /// <returns>the hitbox</returns>
        public Rectangle GetHitbox()
        {
            return new Rectangle((Sprite.Offset - Sprite.Origin).ToPoint(), Sprite.Size.ToPoint());
        }
    }
}
