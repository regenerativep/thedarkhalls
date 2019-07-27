using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerEngine.UserInterface
{
    /// <summary>
    /// a ui element for drawing text
    /// </summary>
    public class TextElement : UIElement
    {
        /// <summary>
        /// the color to draw the text as
        /// </summary>
        public Color TextColor;
        /// <summary>
        /// the text to draw
        /// </summary>
        public string Text;
        /// <summary>
        /// the font to use to draw the text
        /// </summary>
        public SpriteFont Font;
        /// <summary>
        /// creates a new text element
        /// </summary>
        /// <param name="uiManager">the ui manager</param>
        /// <param name="position">the top left position of the text</param>
        /// <param name="size">size of the element</param>
        /// <param name="layer">the draw layer</param>
        /// <param name="name">the element name</param>
        /// <param name="color">the text color</param>
        /// <param name="text">the text to draw</param>
        /// <param name="fontName">the asset name of the font to use</param>
        public TextElement(UIManager uiManager, Vector2 position, Vector2 size, float layer, string name, Color color, string text = "", string fontName = "main") : base(uiManager, position, size, layer, name)
        {
            TextColor = color;
            Text = text;
            UIManager.Assets.RequestFont(fontName, (font) =>
            {
                Font = font;
            });
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (Font != null)
            {
                spriteBatch.DrawString(Font, Text, Position + offset, TextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, Layer);
            }
            base.Draw(spriteBatch, offset);
        }
    }
}
