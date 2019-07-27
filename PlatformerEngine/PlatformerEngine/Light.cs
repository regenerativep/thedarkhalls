using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlatformerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    public class Light
    {
        public Vector2 Position;
        public SpriteData Sprite;
        public Color Color;
        public Light(Vector2 pos)
        {
            Position = pos;
            Sprite = new SpriteData();
            Color = Color.White;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 viewPosition)
        {
            Sprite.Draw(spriteBatch, Position - viewPosition, Color);
        }
    }
}
