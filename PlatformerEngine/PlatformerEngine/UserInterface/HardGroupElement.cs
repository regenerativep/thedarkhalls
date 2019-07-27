using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine.UserInterface
{
    /// <summary>
    /// a hard version of the group element, with its own texture that it draws to
    /// </summary>
    public class HardGroupElement : GroupElement
    {
        /// <summary>
        /// the separate graphics to draw to
        /// </summary>
        public RenderTarget2D Graphics;
        /// <summary>
        /// creates a new hard group element
        /// </summary>
        /// <param name="uiManager">the ui manager</param>
        /// <param name="position">the position</param>
        /// <param name="size">the size</param>
        /// <param name="layer">the draw layer</param>
        /// <param name="name">the element name</param>
        public HardGroupElement(UIManager uiManager, Vector2 position, Vector2 size, float layer, string name) : base(uiManager, position, size, layer, name)
        {
            Graphics = new RenderTarget2D(UIManager.Game.GraphicsDevice, (int)Size.X, (int)Size.Y);
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(Graphics);
            spriteBatch.Begin(UIManager.SortMode);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);
            for(int i = 0; i < Elements.Count; i++)
            {
                Elements[i].Draw(spriteBatch, SoftOffset);
            }
            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(UIManager.SortMode);
            spriteBatch.Draw(Graphics, Position + offset, Color.White);
        }
    }
}
