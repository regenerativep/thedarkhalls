using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlatformerEngine;
using PlatformerTestGame.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerTestGame.Items
{
    public abstract class Item
    {
        public float Damage;
        public SpriteData Sprite;
        public int MaxCooldown;
        public int Cooldown;
        public Item()
        {
            Damage = 0;
            MaxCooldown = 0;
            Cooldown = 0;
            Sprite = new SpriteData();
        }
        public abstract void DrawItem(SpriteBatch spriteBatch, Vector2 offset);
        public abstract void Use(PlayerObject player);
        public void DoUse(PlayerObject player)
        {
            if(Cooldown == 0)
            {
                Use(player);
                Cooldown = MaxCooldown;
            }
        }
        public void Update()
        {
            if(Cooldown > 0)
            {
                Cooldown--;
            }
        }
    }
}
