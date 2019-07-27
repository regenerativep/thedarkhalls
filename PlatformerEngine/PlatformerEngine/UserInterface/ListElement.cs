using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using PlatformerEngine;

namespace PlatformerEngine.UserInterface
{
    /// <summary>
    /// an element that has a list of items
    /// </summary>
    public class ListElement : HardGroupElement
    {
        /// <summary>
        /// the list of items
        /// </summary>
        public List<UIElement> Items;
        /// <summary>
        /// the maximum scroll distance from 0
        /// </summary>
        public float MaxScroll;
        /// <summary>
        /// creates a new list element
        /// </summary>
        /// <param name="uiManager">the ui manager</param>
        /// <param name="position">the position</param>
        /// <param name="size">the size</param>
        /// <param name="layer">the draw layer</param>
        /// <param name="name">the element name</param>
        public ListElement(UIManager uiManager, Vector2 position, Vector2 size, float layer, string name) : base(uiManager, position, size, layer, name)
        {
            Items = new List<UIElement>();
            MaxScroll = 0;
        }
        /// <summary>
        /// adds an item to the list
        /// </summary>
        /// <param name="item">the ui element</param>
        /// <param name="ind">an index if we are inserting somewhere within the list</param>
        public void AddItem(UIElement item, int ind = -1)
        {
            if(ind < 0)
            {
                Items.Add(item);
            }
            else
            {
                Items.Insert(ind, item);
            }
            UpdateList();
        }
        /// <summary>
        /// removes an item from the list
        /// </summary>
        /// <param name="item">the item to remove</param>
        public void RemoveItem(UIElement item)
        {
            for(int i = Items.Count - 1; i >= 0; i--)
            {
                if(Items[i] == item)
                {
                    Items.RemoveAt(i);
                    break;
                }
            }
            UpdateList();
        }
        /// <summary>
        /// removes an item from the list at the given index
        /// </summary>
        /// <param name="ind">the index to remove from</param>
        public void RemoveItem(int ind)
        {
            Items.RemoveAt(ind);
            UpdateList();
        }
        /// <summary>
        /// updates the list on GroupElement if any changes were made
        /// </summary>
        public void UpdateList()
        {
            Elements.Clear();
            int nextY = 0;
            for(int i = 0; i < Items.Count; i++)
            {
                UIElement item = Items[i];
                item.Position.Y = nextY;
                nextY += (int)Math.Ceiling(item.Size.Y);
                Elements.Add(item);
            }
            MaxScroll = Math.Max(nextY - Size.Y, 0);
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.DrawOutlinedRectangle(Position + offset, Position + Size + offset, Color.White, Color.Black, Layer);
            base.Draw(spriteBatch, offset);
        }
        public override void Scroll(MouseState mouseState, float amount)
        {
            SoftOffset.Y += amount;
            if (SoftOffset.Y < -MaxScroll) SoftOffset.Y = -MaxScroll;
            if (SoftOffset.Y > 0) SoftOffset.Y = 0;
            base.Scroll(mouseState, amount);
        }
    }
}
