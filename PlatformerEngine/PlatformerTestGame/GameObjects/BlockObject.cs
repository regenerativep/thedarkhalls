using PlatformerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using PlatformerEngine.Physics;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerTestGame.GameObjects
{
    public class BlockObject : GameObject
    {
        public BlockObject(Room room, Vector2 position) : base(room, position)
        {
            Vector2 size = new Vector2(64, 64);
            Sprite.Size = size;
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 viewPosition)
        {
            Rectangle hitbox = GetHitbox();
            hitbox.Location += Position.ToPoint();
            spriteBatch.DrawRectangle(hitbox.Location.ToVector2(), (hitbox.Location + hitbox.Size).ToVector2(), Color.White, 0);
            base.Draw(spriteBatch, viewPosition);
        }
    }
}
