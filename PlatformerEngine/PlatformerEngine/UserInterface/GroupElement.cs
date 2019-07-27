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
    /// an element that can group multiple elements together
    /// </summary>
    public class GroupElement : UIElement
    {
        /// <summary>
        /// the list of grouped elements
        /// </summary>
        public List<UIElement> Elements;
        /// <summary>
        /// offsets drawing and mouse input
        /// </summary>
        public Vector2 SoftOffset;
        /// <summary>
        /// creates a new group element
        /// </summary>
        /// <param name="uiManager">the ui manager</param>
        /// <param name="position">the position</param>
        /// <param name="size">the size</param>
        /// <param name="layer">the draw layer</param>
        /// <param name="name">the element name</param>
        public GroupElement(UIManager uiManager, Vector2 position, Vector2 size, float layer, string name) : base(uiManager, position, size, layer, name)
        {
            Elements = new List<UIElement>();
            SoftOffset = new Vector2(0, 0);
        }
        public override void Update()
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                Elements[i].Update();
            }
            base.Update();
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            for(int i = 0; i < Elements.Count; i++)
            {
                Elements[i].Draw(spriteBatch, Position + offset + SoftOffset);
            }
            base.Draw(spriteBatch, offset);
        }
        public override void MousePressed(MouseState mouseState, Vector2 offset)
        {
            Vector2 newOffset = Position + SoftOffset + offset;
            Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y) - newOffset;
            for (int i = Elements.Count - 1; i >= 0; i--)
            {
                UIElement elem = Elements[i];
                if (PlatformerMath.PointInRectangle(new Rectangle(elem.Position.ToPoint(), elem.Size.ToPoint()), mousePos))
                {
                    elem.MousePressed(mouseState, newOffset);
                    break;
                }
            }
            base.MousePressed(mouseState, offset);
        }
        public override void MouseReleased(MouseState mouseState, Vector2 offset)
        {
            Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y) - Position - SoftOffset;
            for (int i = Elements.Count - 1; i >= 0; i--)
            {
                UIElement elem = Elements[i];
                if (PlatformerMath.PointInRectangle(new Rectangle(elem.Position.ToPoint(), elem.Size.ToPoint()), mousePos))
                {
                    elem.MouseReleased(mouseState, offset);
                    break;
                }
            }
            base.MouseReleased(mouseState, offset);
        }
        public override void Scroll(MouseState mouseState, float amount)
        {
            Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y) - Position;
            for (int i = Elements.Count - 1; i >= 0; i--)
            {
                UIElement elem = Elements[i];
                if (PlatformerMath.PointInRectangle(new Rectangle(elem.Position.ToPoint(), elem.Size.ToPoint()), mousePos))
                {
                    elem.Scroll(mouseState, amount);
                }
            }
            base.Scroll(mouseState, amount);
        }
        /// <summary>
        /// destroys all children from this group
        /// </summary>
        /// <param name="andTheirChildren">includes the children of the children</param>
        /// <param name="hardDestroy">destroy the children but with more force</param>
        public void RemoveAllChildren(bool andTheirChildren = false, bool hardDestroy = false)
        {
            for(int i = Elements.Count - 1; i >= 0; i--)
            {
                UIElement elem = Elements[i];
                if (andTheirChildren)
                {
                    try
                    {
                        GroupElement group = (GroupElement)elem;
                        group.RemoveAllChildren(true, hardDestroy);
                    }
                    catch (InvalidCastException)
                    {
                        //
                    }
                }
                elem.Destroy(hardDestroy);
                Elements.RemoveAt(i);
            }
        }
        public override void Destroy(bool hardDestroy = false)
        {
            if(hardDestroy)
            {
                RemoveAllChildren(true, true);
            }
            base.Destroy(hardDestroy);
        }
    }
}
