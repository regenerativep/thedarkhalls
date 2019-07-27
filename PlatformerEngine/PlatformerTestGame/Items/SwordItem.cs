using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformerEngine;
using PlatformerTestGame.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerTestGame.Items
{
    public class SwordItem : Item
    {
        public Texture2D SwipeTexture;
        public SoundEffect[] SwipeSounds;
        public SwordItem(Room room) : base()
        {
            Damage = 2;
            MaxCooldown = 16;
            room.Engine.Assets.RequestTexture("itm_sword", (tex) =>
            {
                Sprite.Change(tex);
                Sprite.Size = new Vector2(48, 48);
            });
            room.Engine.Assets.RequestTexture("itm_sword_swipe", (tex) =>
            {
                SwipeTexture = tex;
            });
            SwipeSounds = new SoundEffect[3];
            room.Engine.Assets.RequestSound("snd_sword_1", (sound) =>
            {
                SwipeSounds[0] = sound;
            });
            room.Engine.Assets.RequestSound("snd_sword_2", (sound) =>
            {
                SwipeSounds[1] = sound;
            });
            room.Engine.Assets.RequestSound("snd_sword_3", (sound) =>
            {
                SwipeSounds[2] = sound;
            });
        }
        public override void DrawItem(SpriteBatch spriteBatch, Vector2 offset)
        {
            Sprite.Draw(spriteBatch, offset);
        }
        public override void Use(PlayerObject player)
        {
            MouseState mouseState = Mouse.GetState();
            float dir = (float)Math.Atan2(mouseState.Y - (player.Position.Y + player.Room.ViewPosition.Y), mouseState.X - (player.Position.X + player.Room.ViewPosition.X));
            ProjectileObject proj = new ProjectileObject(player.Room, player.Position, dir, 8, Damage, "obj_enemy", "obj_boss");
            proj.TimeToLive = 10;
            proj.Sprite.Change(SwipeTexture);
            proj.Sprite.Angle = dir;
            proj.Sprite.Size = new Vector2(16, 32);
            proj.Sprite.Origin = proj.Sprite.Size / 2;
            player.Room.GameObjectList.Add(proj);
            player.Room.Sounds.PlaySound(SwipeSounds[PlatformerMath.Choose(0, 1, 2)], 1);
        }
    }
}
