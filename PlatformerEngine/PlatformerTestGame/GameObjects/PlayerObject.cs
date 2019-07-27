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
using Microsoft.Xna.Framework.Audio;

namespace PlatformerTestGame.GameObjects
{
    public class PlayerObject : GameObject, IDamagable
    {
        public static float Gravity = 0.6f;
        public static Vector2 MaxVelocity = new Vector2(8, 16);
        public static float GroundFriction = 0.4f;
        public static int MaxJumpsLeft = 1;
        public static int MaxJumpCooldown = 15;

        public Item Item;
        public InputManager Input;
        public KeyInputTrigger Left, Right, Jump;
        public float GroundSpeed, AirSpeed, JumpSpeed;
        public float MinDirectionChangeSpeed, MinRunAnimationSpeed;
        public float MaxHealth;
        public float Health;
        public bool Grounded;
        public int JumpsLeft;
        public int JumpCooldown;
        public Texture2D IdleImage;
        public Texture2D[] RunImage;
        public MouseState MouseState, PrevMouseState;
        public SpriteFont Font;
        public SoundEffect JumpSound, AirJumpSound;
        public Light Light;
        public PlayerObject(Room room, Vector2 position) : base(room, position)
        {
            JumpsLeft = 0;
            Persistent = true;
            Input = new InputManager();
            GroundSpeed = 1f;
            AirSpeed = 0.3f;
            JumpSpeed = -17f;
            Grounded = false;
            MaxHealth = 128;
            Health = MaxHealth;
            Velocity = new Vector2(0, 0);
            MinDirectionChangeSpeed = 1f;
            MinRunAnimationSpeed = 2f;
            IdleImage = null;
            RunImage = null;
            Font = null;
            Left = new KeyInputTrigger(Keys.A);
            Right = new KeyInputTrigger(Keys.D);
            Jump = new KeyInputTrigger(Keys.Space);
            Input.KeyTriggerList.AddRange(new KeyInputTrigger[] { Left, Right, Jump });
            Item = null;
            JumpCooldown = 0;
            Light = new Light(Position);
            Room.LightList.Add(Light);
            ConsoleManager.Commands.Add(new KillPlayerCommand());
        }
        class KillPlayerCommand : ICommand
        {
            public int ArgumentCount { get { return 0; } }

            public string Name { get { return "Kill player"; } }

            public string CallCommand { get { return "killplayer"; } }

            public void Invoke(params string[] args)
            {
                PlayerObject player = (PlayerObject)Program.Game.Engine.CurrentRoom.FindObject("obj_player");
                if(player != null)
                {
                    player.Kill();
                }
            }
        }
        public void Kill()
        {
            BossObject bossObject = (BossObject)Room.FindObject("obj_boss");
            if (Room.GameObjectList.Contains(bossObject))
            {
                Room.Engine.LoadRoom("Levels\\lose.json", new FadeTransition());
            }
            Room.LightList.Remove(Light);
            Room.GameObjectList.Remove(this);
            if(bossObject != null && Room.GameObjectList.Contains(bossObject))
            {
                bossObject.Kill();
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
            assets.RequestTexture("obj_player_idle", (tex) =>
            {
                IdleImage = tex;
                Sprite.Change(IdleImage);
            });
            assets.RequestFramedTexture("obj_player_run", (frames) =>
            {
                RunImage = frames;
            });
            assets.RequestFont("fnt_main", (font) =>
            {
                Font = font;
            });
            assets.RequestSound("msc_main", (snd) =>
            {
                Room.Sounds.PlayMusic(snd, 0.4f);
            });
            assets.RequestSound("snd_jump_ground", (snd) =>
            {
                JumpSound = snd;
            });
            assets.RequestSound("snd_jump_air", (snd) =>
            {
                AirJumpSound = snd;
            });
            assets.RequestTexture("lgt_circular", (tex) =>
            {
                Light.Sprite.Change(tex);
                Light.Sprite.Size = new Vector2(1024, 1024);
                Light.Sprite.Offset = -Light.Sprite.Size / 2;
            });
            assets.RequestTexture("bg_stoneBrick", (tex) =>
            {
                Room.Background.Change(tex);
                Room.Background.Size = new Vector2(Room.Width, Room.Height);
            });
            Sprite.Size = new Vector2(48, 64);
            Sprite.Offset = -(new Vector2(Sprite.Size.X / 2, Sprite.Size.Y / 2 + (Sprite.Size.Y - Sprite.Size.X) / 2));
            Sprite.Speed = 0.2f;
            Room.GameObjectList.Add(new ItemObject(Room, new Vector2(192, 640), new SwordItem(Room)));
            Room.GameObjectList.Add(new ItemObject(Room, new Vector2(768, 640), new MagicItem(Room)));
        }
        public override void Update()
        {
            MouseState = Mouse.GetState();
            float moveSpeed = AirSpeed;
            if(Room.FindCollision(new Rectangle((Position + Sprite.Offset + new Vector2(0, 1)).ToPoint(), Sprite.Size.ToPoint()), "obj_block") != null)
            {
                Grounded = true;
                moveSpeed = GroundSpeed;
                if(Math.Abs(Velocity.X) > GroundFriction)
                {
                    Velocity.X -= Math.Sign(Velocity.X) * GroundFriction;
                }
                else
                {
                    Velocity.X = 0;
                }
                JumpsLeft = MaxJumpsLeft;
            }
            else
            {
                Grounded = false;
            }
            Input.Update();
            if(Light != null)
            {
                Light.Position = Position;
            }
            if (Jump.Pressed && JumpsLeft > 0 && JumpCooldown == 0)
            {
                if(Grounded)
                {
                    Room.Sounds.PlaySound(JumpSound);
                }
                else
                {
                    Room.Sounds.PlaySound(AirJumpSound);
                }
                Velocity.Y = JumpSpeed;
                JumpsLeft--;
                JumpCooldown = MaxJumpCooldown;
                if (JumpsLeft == 0)
                {
                    if (Left.Pressed && Velocity.X > -2)
                    {
                        Velocity.X = -2;
                    }
                    if (Right.Pressed && Velocity.X < 2)
                    {
                        Velocity.X = 2;
                    }
                }
                Grounded = false;
            }
            if (Left.Pressed && Velocity.X > -MaxVelocity.X)
            {
                Velocity.X -= moveSpeed;
            }
            if (Right.Pressed && Velocity.X < MaxVelocity.X)
            {
                Velocity.X += moveSpeed;
            }
            if (Velocity.X > MinDirectionChangeSpeed)
            {
                Sprite.SpriteEffect = SpriteEffects.None;
            }
            else if (Velocity.X < -MinDirectionChangeSpeed)
            {
                Sprite.SpriteEffect = SpriteEffects.FlipHorizontally;
            }
            if(Math.Abs(Velocity.X) >= MinRunAnimationSpeed)
            {
                Sprite.Change(RunImage);
            }
            else
            {
                Sprite.Change(IdleImage);
            }
            Item?.Update();
            if (MouseState.LeftPressed())
            {
                Item?.DoUse(this);
            }
            PlatformerGame game = (PlatformerGame)Room.Engine.Game;
            if (Position.X > Room.Width && Velocity.X > 0)
            {
                Position.X -= Room.Width;
                game.GoToRoom(game.CurrentRoomX + 1, game.CurrentRoomY);
                JumpsLeft = MaxJumpsLeft;
            }
            else if (Position.X < 0 && Velocity.X < 0)
            {
                Position.X += Room.Width;
                game.GoToRoom(game.CurrentRoomX - 1, game.CurrentRoomY);
                JumpsLeft = MaxJumpsLeft;
            }
            else if (Position.Y > Room.Height && Velocity.Y > 0)
            {
                Position.Y -= Room.Height;
                game.GoToRoom(game.CurrentRoomX, game.CurrentRoomY + 1);
                JumpsLeft = MaxJumpsLeft;
            }
            else if (Position.Y < 0 && Velocity.Y < 0)
            {
                Position.Y += Room.Height;
                game.GoToRoom(game.CurrentRoomX, game.CurrentRoomY - 1);
                JumpsLeft = MaxJumpsLeft;
            }
            Rectangle hitbox = GetHitbox();
            hitbox.Location += Position.ToPoint();
            GameObject itemObject = Room.FindCollision(hitbox, "obj_item");
            if (itemObject != null)
            {
                ItemObject itemContainer = (ItemObject)itemObject;
                Item = itemContainer.Item;
                UpgradeObject.Item = Item;
                Room.GameObjectList.Remove(itemObject);
            }
            else
            {
                itemObject = Room.FindCollision(hitbox, "obj_upgrade");
                if (itemObject != null)
                {
                    UpgradeObject upgradeObject = (UpgradeObject)itemObject;
                    upgradeObject.Upgrade();
                }
            }
            if(JumpCooldown > 0)
            {
                JumpCooldown--;
            }
            Velocity.Y += Gravity;
            base.Update();
            PrevMouseState = MouseState;
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 viewPosition)
        {
            Vector2 drawSize = new Vector2(144, 48);
            Vector2 drawFrom = Position - (drawSize / 2) - new Vector2(0, 64) - viewPosition;
            spriteBatch.DrawOutlinedRectangle(drawFrom, drawFrom + drawSize, Color.White, Color.Black, 0.99f);
            spriteBatch.DrawString(Font, "health: " + Health + " / " + MaxHealth, drawFrom + new Vector2(4, 4), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);
            string damageText = "power: ";
            if (Item != null)
            {
                damageText += Item.Damage.ToString();
            }
            else
            {
                damageText += "no item";
            }
            spriteBatch.DrawString(Font, damageText, drawFrom + new Vector2(4, 24), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);
            base.Draw(spriteBatch, viewPosition);
        }
    }
}
