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
    /// contains information on a sprite and how it is drawn
    /// </summary>
    public class SpriteData
    {
        /// <summary>
        /// the current frame that we are on
        /// </summary>
        public int FrameIndex
        {
            get
            {
                return (int)currentFrame;
            }
            set
            {
                currentFrame = value;
            }
        }
        private float currentFrame;
        /// <summary>
        /// speed by which we change frames
        /// </summary>
        public float Speed;
        /// <summary>
        /// angle of the drawn sprite
        /// </summary>
        public float Angle;
        /// <summary>
        /// offset of the sprite
        /// </summary>
        public Vector2 Offset;
        /// <summary>
        /// origin of the sprite
        /// </summary>
        public Vector2 Origin;
        /// <summary>
        /// size of the sprite
        /// </summary>
        public Vector2 Size;
        /// <summary>
        /// rectangle on the image to draw from
        /// </summary>
        public Rectangle? Source;
        /// <summary>
        /// the layer to draw to
        /// </summary>
        public LayerData LayerData;
        /// <summary>
        /// the frames of the sprite
        /// </summary>
        public Texture2D[] Frames;
        /// <summary>
        /// the sprite effect to apply to the sprite
        /// </summary>
        public SpriteEffects SpriteEffect;
        /// <summary>
        /// creates a new sprite data instance
        /// </summary>
        public SpriteData()
        {
            FrameIndex = 0;
            Speed = 1;
            LayerData = new LayerData(0);
            Offset = new Vector2(0, 0);
            Origin = new Vector2(0, 0);
            Size = new Vector2(0, 0);
            SpriteEffect = SpriteEffects.None;
            Source = null;
        }
        /// <summary>
        /// creates a new sprite data instance with the given frames
        /// </summary>
        /// <param name="frames">frames of the sprite</param>
        public SpriteData(Texture2D[] frames) : base()
        {
            Frames = frames;
        }
        /// <summary>
        /// updates the sprite data
        /// </summary>
        public void Update()
        {
            if (Frames == null) return;
            currentFrame += Speed;
            while (currentFrame < 0) currentFrame += Frames.Length;
            while (currentFrame >= Frames.Length) currentFrame -= Frames.Length;
        }
        /// <summary>
        /// draws the sprite with the sprite data
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to draw to</param>
        /// <param name="position">the position to draw at</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Draw(spriteBatch, position, Color.White);
        }
        /// <summary>
        /// draws the sprite with the sprite data
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to draw to</param>
        /// <param name="position">the position to draw at</param>
        /// <param name="color">the color to use when drawing</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color) //TODO: figure out why we can't just use a variable in the object for color
        {
            if (Frames != null)
            {
                Rectangle drawRect = new Rectangle((position + Offset).ToPoint(), Size.ToPoint());
                spriteBatch.Draw(Frames[FrameIndex], drawRect, Source, color, Angle, Origin, SpriteEffect, LayerData.ActualLayer);
            }
        }
        /// <summary>
        /// changes the sprite to the given texture
        /// </summary>
        /// <param name="newSprite">the texture</param>
        public void Change(Texture2D newSprite)
        {
            if(Frames != null && Frames.Length > 0 && Frames[0] == newSprite)
            {
                return;
            }
            Frames = new Texture2D[] { newSprite };
            FrameIndex = 0;
        }
        /// <summary>
        /// changes the sprite to the given framed texture
        /// </summary>
        /// <param name="newSprite">the framed texture</param>
        public void Change(Texture2D[] newSprite)
        {
            if (Frames != newSprite)
            {
                Frames = newSprite;
                FrameIndex = 0;
            }
        }
    }
}
