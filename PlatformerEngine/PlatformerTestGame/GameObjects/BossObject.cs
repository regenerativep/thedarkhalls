using PlatformerEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PlatformerEngine.Physics;
using Microsoft.Xna.Framework.Graphics;
using PlatformerTestGame.Items;

namespace PlatformerTestGame.GameObjects
{
    public class BossObject : GameObject, IDamagable
    {
        public static float Gravity = 0.6f;
        public static Vector2 MaxVelocity = new Vector2(3, 3);
        public static float GroundFriction = 0.4f;
        public static float MaxHealth = 768;

        public Boolean IsAggro;
        public float GroundSpeed, AirSpeed, JumpSpeed;
        public float MinDirectionChangeSpeed, MinRunAnimationSpeed;
        public float Health;
        public bool Grounded;
        public Texture2D IdleImage;
        public Texture2D[] RunImage;
        public Light Light;
        public SpriteFont Font;
        public BossObject(Room room, Vector2 position) : base(room, position)
        {
            GroundSpeed = 1f;
            AirSpeed = 0.3f;
            Health = MaxHealth;
            Velocity = new Vector2(0, 0);
            MinDirectionChangeSpeed = 1f;
            MinRunAnimationSpeed = 2f;
            IdleImage = null;
            RunImage = null;
            IsAggro = false;
            Persistent = true;
            Light = new Light(Position);
            Font = null;
            ConsoleManager.Commands.Add(new KillBossCommand());
        }
        class KillBossCommand : ICommand
        {
            public int ArgumentCount { get { return 0; } }

            public string Name { get { return "Kill boss"; } }

            public string CallCommand { get { return "killboss"; } }

            public void Invoke(params string[] args)
            {
                BossObject boss = (BossObject)Program.Game.Engine.CurrentRoom.FindObject("obj_boss");
                if (boss != null)
                {
                    boss.Kill();
                }
            }
        }
        public void Kill()
        {
            PlayerObject playerObject = (PlayerObject)Room.FindObject("obj_player");
            if (Room.GameObjectList.Contains(playerObject))
            {
                Room.Engine.LoadRoom("Levels\\win.json", new FadeTransition());
            }
            Room.LightList.Remove(Light);
            Room.GameObjectList.Remove(this);
            if (playerObject != null && Room.GameObjectList.Contains(playerObject))
            {
                playerObject.Kill();
            }
        }
        public void Damage(float amount)
        {
            Health -= amount;
            if(Health <= 0)
            {
                Kill();
            }
        }
        public override void Load(AssetManager assets)
        {
            assets.RequestFramedTexture("obj_boss_fly", (frames) =>
            {
                RunImage = frames;
                IdleImage = frames[0];
                Sprite.Change(RunImage);
            });
            assets.RequestFont("fnt_main", (font) =>
            {
                Font = font;
            });
            assets.RequestTexture("lgt_circular", (tex) =>
            {
                Light.Sprite.Change(tex);
                Light.Sprite.Size = new Vector2(512, 512);
                Light.Sprite.Offset = -Light.Sprite.Size / 2;
                Light.Color = Color.Red;
            });
            Sprite.Size = new Vector2(64, 96);
            Sprite.Offset = -(new Vector2(Sprite.Size.X / 2, Sprite.Size.Y / 2 + (Sprite.Size.Y - Sprite.Size.X) / 2));
            Sprite.Speed = 0.2f;
        }
        public override void Update()
        {
            Room = Room.Engine.CurrentRoom;
            if (!Room.LightList.Contains(Light))
            {
                Room.LightList.Add(Light);
            }
            Light.Position = Position;
            //AI CODE HERE
            PlayerObject Player = (PlayerObject)Room.FindObject("obj_player");
            if (IsAggro)
            {
                if (Player != null)
                {
                    float TargetX = Player.Position.X + PlatformerMath.RandNumGen.Next(32);
                    float TargetY = Player.Position.Y + PlatformerMath.RandNumGen.Next(32);
                    if (Position.X > TargetX)
                    {
                        Velocity.X = -(MaxVelocity.X);
                    }
                    else
                    {
                        Velocity.X = MaxVelocity.X;
                    }
                    if (Position.Y > TargetY)
                    {
                        Velocity.Y = -(MaxVelocity.Y);
                    }
                    else
                    {
                        Velocity.Y = MaxVelocity.Y;
                    }
                    if (Room.FindCollision(new Rectangle((Position + Sprite.Offset).ToPoint(), Sprite.Size.ToPoint()), "obj_player") != null)
                    {
                        Player.Damage(2);
                    }
                }
                else
                {
                    Velocity.X = 0;
                    Velocity.Y = 0;
                }

                if (Velocity.X > MinDirectionChangeSpeed)
                {
                    Sprite.SpriteEffect = SpriteEffects.None;
                }
                else if (Velocity.X < -MinDirectionChangeSpeed)
                {
                    Sprite.SpriteEffect = SpriteEffects.FlipHorizontally;
                }
                Sprite.Change(RunImage);
            }
            else
            {
                if (Player != null)
                {
                    Position = Player.Position + new Vector2(-2048, -2048);
                }
            }
            Position += Velocity;
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 viewPosition)
        {

            Vector2 drawSize = new Vector2(144, 24);
            Vector2 drawFrom = Position - (drawSize / 2) - new Vector2(0, 64) - viewPosition;
            spriteBatch.DrawOutlinedRectangle(drawFrom, drawFrom + drawSize, Color.White, Color.Black, 0.99f);
            spriteBatch.DrawString(Font, "health: " + Health + " / " + MaxHealth, drawFrom + new Vector2(4, 4), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);
            base.Draw(spriteBatch, viewPosition);
        }
    }
}
