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
    public class UpgradeObject : GameObject
    {
        public static Item Item = null;
        private Room originalRoom;
        public UpgradeObject(Room room, Vector2 position) : base(room, position)
        {
            originalRoom = Room;
            Sprite.Size = new Vector2(48, 48);
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 viewPosition)
        {
            Item?.DrawItem(spriteBatch, viewPosition + Position);
        }
        public void Upgrade()
        {
            if (Item != null)
            {
                Item.Damage += 1;
            }
            Room.GameObjectList.Remove(this);
            originalRoom.GameObjectList.Remove(this);
        }
    }
}
