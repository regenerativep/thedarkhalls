using PlatformerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using PlatformerTestGame.Items;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerTestGame.GameObjects
{
    public class ItemObject : GameObject
    {
        public Item Item;
        public ItemObject(Room room, Vector2 position, Item item) : base(room, position)
        {
            Item = item;
            Sprite.Size = new Vector2(48, 48);
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 viewPosition)
        {
            Item.DrawItem(spriteBatch, viewPosition + Position);
        }
    }
}
