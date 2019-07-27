using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlatformerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatformerTestGame.GameObjects;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace PlatformerTestGame.Items
{
    public class MagicItem : Item
    {
        public Texture2D[] MagicBallTexture;
        public SoundEffect[] MagicSounds;
        public MagicItem(Room room) : base()
        {
            Damage = 1;
            MaxCooldown = 12;
            room.Engine.Assets.RequestTexture("itm_magic_book", (tex) =>
            {
                Sprite.Change(tex);
                Sprite.Size = new Vector2(48, 48);
            });
            room.Engine.Assets.RequestFramedTexture("itm_magic_ball", (frames) =>
            {
                MagicBallTexture = frames;
            });
            MagicSounds = new SoundEffect[2];
            room.Engine.Assets.RequestSound("snd_magic_1", (sound) =>
            {
                MagicSounds[0] = sound;
            });
            room.Engine.Assets.RequestSound("snd_magic_2", (sound) =>
            {
                MagicSounds[1] = sound;
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
            ProjectileObject proj = new ProjectileObject(player.Room, player.Position, dir, 6, Damage, "obj_enemy", "obj_boss");
            proj.TimeToLive = 150;
            proj.Sprite.Change(MagicBallTexture);
            proj.Sprite.Size = new Vector2(32, 32);
            proj.Sprite.Offset = -proj.Sprite.Size / 2;
            proj.Sprite.Speed = 0.3f;
            player.Room.GameObjectList.Add(proj);
            player.Room.Sounds.PlaySound(MagicSounds[PlatformerMath.Choose(0, 1)], 1);
        }
    }
}
