using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine.UserInterface
{
    /// <summary>
    /// a box that can have text inputted into it
    /// </summary>
    public class TextInputElement : ButtonElement, IInputable
    {
        public string Text
        {
            get
            {
                if(TextElement != null)
                {
                    return TextElement.Text;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                if (TextElement != null)
                {
                    TextElement.Text = value;
                }
            }
        }
        public char[] ValidKeys { get; set; }
        /// <summary>
        /// creates a new text input element
        /// </summary>
        /// <param name="uiManager">a reference to the ui manager</param>
        /// <param name="position">the position</param>
        /// <param name="size">the size</param>
        /// <param name="layer">the draw layer</param>
        /// <param name="name">the name</param>
        public TextInputElement(UIManager uiManager, Vector2 position, Vector2 size, float layer, string name) : base(uiManager, position, size, layer, name, "")
        {
            ValidKeys = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ./\\-_".ToCharArray();
            Click = () =>
            {
                UIManager.CurrentInput = this;
            };
        }
    }
}
