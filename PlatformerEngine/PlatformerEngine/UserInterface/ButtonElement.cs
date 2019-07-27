using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using PlatformerEngine;
using Microsoft.Xna.Framework.Input;

namespace PlatformerEngine.UserInterface
{
    /// <summary>
    /// an element with text that can be clicked on
    /// </summary>
    public class ButtonElement : UIElement
    {
        /// <summary>
        /// the text to draw
        /// </summary>
        public TextElement TextElement;
        /// <summary>
        /// the padding from the top left to draw the text from
        /// </summary>
        public Vector2 TextPadding;
        /// <summary>
        /// called when this is clicked on
        /// </summary>
        public Action Click;
        /// <summary>
        /// creates a new button element
        /// </summary>
        /// <param name="uiManager">the ui manager</param>
        /// <param name="position">the position</param>
        /// <param name="size">the size</param>
        /// <param name="layer">the draw layer</param>
        /// <param name="name">element name</param>
        /// <param name="text">the text shown by this button</param>
        public ButtonElement(UIManager uiManager, Vector2 position, Vector2 size, float layer, string name, string text) : base(uiManager, position, size, layer, name)
        {
            TextPadding = new Vector2(2, 2);
            TextElement = new TextElement(UIManager, TextPadding, Size - TextPadding, Layer + 0.01f, name + "_text", Color.Black, text);
            Click = null;
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.DrawOutlinedRectangle(Position + offset, Position + Size + offset, Color.White, Color.Black, Layer);
            TextElement.Draw(spriteBatch, Position + offset);
            base.Draw(spriteBatch, offset);
        }
        public override void MousePressed(MouseState mouseState, Vector2 offset)
        {
            Click?.Invoke();
            base.MousePressed(mouseState, offset);
        }
        public override void Destroy(bool hardDestroy = false)
        {
            TextElement.Destroy(hardDestroy);
            base.Destroy();
        }
    }
}
