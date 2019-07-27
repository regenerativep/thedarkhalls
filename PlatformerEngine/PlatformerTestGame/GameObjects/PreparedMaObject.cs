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
    public class PreparedMaObject : GameObject
    {
        public static int MaxTimer = 640;
        public static float TextOpacityChange = 0.01f;
        public SpriteFont Font;
        public Light Light;
        public int Timer;
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
        public PreparedMaObject(Room room, Vector2 position) : base(room, position)
        {
            Vector2 viewSize = new Vector2(Room.Engine.Game.GraphicsDevice.Viewport.Width, Room.Engine.Game.GraphicsDevice.Viewport.Height);
            Light = new Light(viewSize / 2);
            Font = null;
            Text = PlatformerMath.Choose("Ready or not, here I come.", "I'm coming.", "Are you ready?", "Are you prepared?", "I hope you're prepared to die.");
            TextOpacity = 0;
            Timer = MaxTimer;
            Persistent = true;
            Room.Engine.Assets.RequestFont("fnt_main", (font) =>
            {
                Font = font;
                Text = Text;
            });
            Light = new Light(viewSize / 2);
            Room.Engine.Assets.RequestTexture("lgt_circular", (tex) =>
            {
                Light.Sprite.Change(tex);
                Light.Sprite.Size = new Vector2(384, 256);
                Light.Sprite.Offset = -Light.Sprite.Size / 2;
                Light.Color = new Color(255, 102, 102);
            });
            Room.LightList.Add(Light);
        }
        public override void Update()
        {
            if (Timer > 0)
            {
                Timer--;
            }
            else
            {
                Room.LightList.Remove(Light);
                Room.GameObjectList.Remove(this);
                return;
            }
            if (TextOpacity < 1)
            {
                TextOpacity += TextOpacityChange;
            }
            if (TextOpacity > 1)
            {
                TextOpacity = 1;
            }
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 viewPosition)
        {
            if (!Room.LightList.Contains(Light))
            {
                Room.LightList.Add(Light);
            }
            Vector2 viewSize = new Vector2(spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height);
            if (Font != null)
            {
                spriteBatch.DrawString(Font, Text, (viewSize - TextSize) / 2, Color.Red * TextOpacity);
            }
            base.Draw(spriteBatch, viewPosition);
        }
    }
}
