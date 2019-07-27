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
    public class GameOverObject : GameObject
    {
        public static float TextOpacityChange = 0.001f;
        public SpriteFont Font;
        public Light Light;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                if (Font != null)
                {
                    TextSize = Font.MeasureString(text);
                }
            }
        }
        private string text;
        public Vector2 TextSize;
        public float TextOpacity;
        public GameOverObject(Room room, Vector2 position) : base(room, position)
        {
            Font = null;
            Text = PlatformerMath.Choose("I don't think you were prepared.", "Back to the hole where you belong.", "Are you even trying?", "Time's up.", "Even SCP-999 would stand a better chance than you.");
            TextOpacity = 0;
        }
        public override void Load(AssetManager assets)
        {
            assets.RequestFont("fnt_main", (font) =>
            {
                Font = font;
                Text = Text;
            });
            Vector2 viewSize = new Vector2(Room.Engine.Game.GraphicsDevice.Viewport.Width, Room.Engine.Game.GraphicsDevice.Viewport.Height);
            Light = new Light(viewSize / 2);
            assets.RequestTexture("lgt_circular", (tex) =>
            {
                Light.Sprite.Change(tex);
                Light.Sprite.Size = new Vector2(384, 256);
                Light.Sprite.Offset = -Light.Sprite.Size / 2; 
            });
            Room.LightList.Add(Light);
            base.Load(assets);
        }
        public override void Update()
        {
            if(TextOpacity < 1)
            {
                TextOpacity += TextOpacityChange;
            }
            if(TextOpacity > 1)
            {
                TextOpacity = 1;
            }
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 viewPosition)
        {
            if(!Room.LightList.Contains(Light))
            {
                Room.LightList.Add(Light);
            }
            Vector2 viewSize = new Vector2(spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height);
            spriteBatch.DrawRectangle(new Vector2(0, 0), viewSize, Color.Black, 0f);
            if (Font != null)
            {
                spriteBatch.DrawString(Font, Text, (viewSize - TextSize) / 2, Color.Red * TextOpacity);
            }
            base.Draw(spriteBatch, viewPosition);
        }
    }
}
