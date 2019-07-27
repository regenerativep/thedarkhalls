using PlatformerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerTestGame.GameObjects
{
    public class ProjectileObject : GameObject
    {
        public string[] Targets;
        public float Damage;
        public int TimeToLive;
        public ProjectileObject(Room room, Vector2 position, float dir, float speed, float damage, params string[] targets) : base(room, position)
        {
            TimeToLive = 0;
            Targets = targets;
            Damage = damage;
            Velocity = new Vector2((float)Math.Cos(dir), (float)Math.Sin(dir)) * speed;
        }
        public override void Update()
        {
            base.Update();
            Rectangle hitbox = GetHitbox();
            hitbox.Location += Position.ToPoint();

            GameObject hitObject = null;
            for (int i = 0; i < Targets.Length; i++)
            {
                hitObject = Room.FindCollision(hitbox, Targets[i]);
                if (hitObject != null) break;
            }
            if(hitObject != null)
            {
                try
                {
                    IDamagable target = (IDamagable)hitObject;
                    target.Damage(Damage);
                }
                catch(InvalidCastException)
                {
                    //..
                }
                Room.GameObjectList.Remove(this);
                return;
            }
            if (Touching || Position.X < 0 || Position.Y < 0 || Position.X > Room.Width || Position.Y > Room.Height || TimeToLive <= 0)
            {
                Room.GameObjectList.Remove(this);
            }
            TimeToLive--;
        }
    }
}
