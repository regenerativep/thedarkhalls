using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine.UserInterface
{
    /// <summary>
    /// base class for ui elements
    /// </summary>
    public abstract class UIElement
    {
        /// <summary>
        /// the position of the element
        /// </summary>
        public Vector2 Position;
        /// <summary>
        /// the size of the element
        /// </summary>
        public Vector2 Size;
        /// <summary>
        /// the draw layer of the element
        /// </summary>
        public float Layer;
        /// <summary>
        /// a reference to the parent ui manager
        /// </summary>
        public UIManager UIManager;
        /// <summary>
        /// the name of the ui element
        /// </summary>
        public string Name;
        /// <summary>
        /// constructs a ui element
        /// </summary>
        /// <param name="uiManager">a reference to the ui manager</param>
        /// <param name="position">position of the element</param>
        /// <param name="size">size of the element</param>
        /// <param name="layer">draw layer of the element</param>
        /// <param name="name">name of the element (no duplicates)</param>
        public UIElement(UIManager uiManager, Vector2 position, Vector2 size, float layer, string name)
        {
            UIManager = uiManager;
            Position = position;
            Size = size;
            Layer = layer;
            Name = name;
            UIManager.Elements.Add(name, this);
            ConsoleManager.WriteLine("loaded ui element " + Name);
        }
        /// <summary>
        /// call when this ui element has been clicked on
        /// </summary>
        /// <param name="mouseState">the current mouse state</param>
        /// <param name="offset">offset to fix mouse input positions</param>
        public virtual void MousePressed(MouseState mouseState, Vector2 offset) { }
        /// <summary>
        /// call when this ui element has had a mouse release a button on top of it
        /// </summary>
        /// <param name="mouseState">the current mouse state</param>
        /// <param name="offset">offset to fix mouse input positions</param>
        public virtual void MouseReleased(MouseState mouseState, Vector2 offset) { }
        /// <summary>
        /// updates this ui element
        /// </summary>
        public virtual void Update() { }
        /// <summary>
        /// draws this ui element
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to draw to</param>
        /// <param name="offset">offset for drawing to the right place</param>
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 offset) { }
        /// <summary>
        /// call when this ui element has a mouse scrolling on top of it
        /// </summary>
        /// <param name="mouseState">the mouse state</param>
        /// <param name="amount">the amount scrolled</param>
        public virtual void Scroll(MouseState mouseState, float amount) { }
        /// <summary>
        /// remove this ui element's reference from the ui manager
        /// </summary>
        /// <param name="hardDestroy">remove all sub elements too</param>
        public virtual void Destroy(bool hardDestroy = false)
        {
            UIManager.DestroyUIElement(this);
        }
    }
}
