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
    /// a list element with the items going horizontally
    /// </summary>
    public class HorizontalListElement : HardGroupElement
    {
        /// <summary>
        /// the list of items
        /// </summary>
        public List<UIElement> Items;
        /// <summary>
        /// creates a new horizontal list element
        /// </summary>
        /// <param name="uiManager">the ui manager</param>
        /// <param name="position">the position</param>
        /// <param name="size">the size</param>
        /// <param name="layer">the draw layer</param>
        /// <param name="name">the element name</param>
        public HorizontalListElement(UIManager uiManager, Vector2 position, Vector2 size, float layer, string name) : base(uiManager, position, size, layer, name)
        {
            Items = new List<UIElement>();
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
            int nextX = 0;
            for(int i = 0; i < Items.Count; i++)
            {
                UIElement item = Items[i];
                item.Position.X = nextX;
                nextX += (int)Math.Ceiling(item.Size.X);
                Elements.Add(item);
            }
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            spriteBatch.DrawOutlinedRectangle(Position + offset, Position + Size + offset, Color.White, Color.Black, Layer - 0.01f);
            base.Draw(spriteBatch, offset);
        }
    }
}
