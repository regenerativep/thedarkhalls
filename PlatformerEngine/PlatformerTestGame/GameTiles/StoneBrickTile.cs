using Microsoft.Xna.Framework;
using PlatformerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerTestGame.GameObjects
{
    public class StoneBrickTile : GameTile
    {
        public StoneBrickTile(Room room, Vector2 position) : base(room, position)
        {
            Sprite.Size = new Vector2(64, 64);
        }
        public override void Load(AssetManager assets)
        {
            bool left = Room.CheckTileAt(Position - new Vector2(Sprite.Size.X, 0));
            bool right = Room.CheckTileAt(Position + new Vector2(Sprite.Size.X, 0));
            bool bottom = Room.CheckTileAt(Position + new Vector2(0, Sprite.Size.Y));
            string spriteNameAddition = "";
            if (!bottom)
            {
                spriteNameAddition += "bottom";
            }
            if (!left)
            {
                spriteNameAddition += "left";
            }
            if (!right)
            {
                spriteNameAddition += "right";
            }
            if (spriteNameAddition.Length == 0)
            {
                spriteNameAddition = "none";
            }
            assets.RequestTexture("tle_stonebrick_" + spriteNameAddition, (tex) =>
            {
                Sprite.Change(tex);
            });
        }
    }
}
