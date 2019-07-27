using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformerEngine;
using PlatformerEngine.UserInterface;

namespace PlatformerEditor
{
    public class LevelElement : UIElement
    {
        public Vector2 SoftOffset;
        public Vector2 Snap;
        public Vector2 LevelSize;
        public Vector2 Gravity;
        public bool PanIsPressed;
        public WorldItem CurrentWorldItem;
        public RenderTarget2D Graphics;
        public LevelElement(UIManager uiManager, Vector2 position, Vector2 size, float layer, string name) : base(uiManager, position, size, layer, name)
        {
            SoftOffset = new Vector2(0, 0);
            Snap = new Vector2(32, 32);
            LevelSize = new Vector2(512, 512);
            Gravity = new Vector2(0, 0);
            PanIsPressed = false;
            CurrentWorldItem = null;
            Graphics = new RenderTarget2D(UIManager.Game.GraphicsDevice, (int)Size.X, (int)Size.Y);
        }
        public override void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            PlatformerEditor actualGame = (PlatformerEditor)UIManager.Game;
            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(Graphics);
            //draw grid
            spriteBatch.Begin(UIManager.SortMode);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);
            //Vector2 actualOffset = offset + SoftOffset;// + Position;
            Vector2 actualOffset = SoftOffset;
            for (int i = (int)actualOffset.X; i <= actualOffset.X + LevelSize.X; i += (int)Snap.X)
            {
                spriteBatch.DrawLine(new Vector2(i, actualOffset.Y), new Vector2(i, actualOffset.Y + LevelSize.Y), Color.Black);
            }
            for (int i = (int)actualOffset.Y; i <= actualOffset.Y + LevelSize.Y; i += (int)Snap.Y)
            {
                spriteBatch.DrawLine(new Vector2(actualOffset.X, i), new Vector2(actualOffset.X + LevelSize.X, i), Color.Black);
            }
            spriteBatch.End();
            //draw layers
            List<int> layerKeys = new List<int>(actualGame.WorldLayers.Keys); //TODO: this is probably slow. make it not
            layerKeys.Sort((a, b) =>
            {
                return Math.Sign(b - a);
            });
            for(int i = 0; i < layerKeys.Count; i++)
            {
                WorldLayer worldLayer = actualGame.WorldLayers[layerKeys[i]];
                if (!worldLayer.IsVisible) continue;
                spriteBatch.Begin(UIManager.SortMode);
                for(int j = 0; j < worldLayer.WorldItems.Count; j++)
                {
                    WorldItem item = worldLayer.WorldItems[j];
                    item.Draw(spriteBatch, actualOffset);
                }
                spriteBatch.End();
            }
            //draw mouse item
            if (actualGame.CurrentWorldItemType == null && CurrentWorldItem != null)
            {
                CurrentWorldItem = null;
            }
            else if ((actualGame.CurrentWorldItemType != null && CurrentWorldItem == null) || (actualGame.CurrentWorldItemType != null && CurrentWorldItem.ItemType != actualGame.CurrentWorldItemType))
            {
                CurrentWorldItem = new WorldItem(UIManager, actualGame.CurrentWorldItemType, SnapPosition(GetMousePosition(offset)), 0.6f);
            }
            else if (CurrentWorldItem != null)
            {
                CurrentWorldItem.Position = SnapPosition(GetMousePosition(offset));
            }
            spriteBatch.Begin(UIManager.SortMode);
            CurrentWorldItem?.Draw(spriteBatch, actualOffset);
            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(UIManager.SortMode);
            spriteBatch.Draw(Graphics, Position + offset, Color.White);
        }
        public override void Update()
        {
            if (PanIsPressed)
            {
                Vector2 mouseDifference = (UIManager.MouseState.Position - UIManager.PreviousMouseState.Position).ToVector2();
                SoftOffset += mouseDifference;
            }
            base.Update();
        }
        public override void MousePressed(MouseState mouseState, Vector2 offset)
        {
            PlatformerEditor actualGame = (PlatformerEditor)UIManager.Game;
            if (mouseState.MiddlePressed())
            {
                PanIsPressed = true;
            }
            else if(mouseState.LeftPressed())
            {
                Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y) - offset - Position - SoftOffset;
                if (actualGame.CurrentWorldLayer != null)
                {
                    WorldItem item = new WorldItem(UIManager, actualGame.CurrentWorldItemType, SnapPosition(mousePos), actualGame.CurrentWorldLayer.DrawLayer);
                    actualGame.CurrentWorldLayer.WorldItems.Add(item);
                }
            }
            else if(mouseState.RightPressed())
            {
                if(actualGame.CurrentWorldLayer != null)
                {
                    Vector2 mousePos = GetMousePosition(offset);
                    for (int i = 0; i < actualGame.CurrentWorldLayer.WorldItems.Count; i++)
                    {
                        WorldItem item = actualGame.CurrentWorldLayer.WorldItems[i];
                        if (PlatformerMath.PointInRectangle(new Rectangle(item.Position.ToPoint(), item.Size.ToPoint()), mousePos))
                        {
                            actualGame.CurrentWorldLayer.WorldItems.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            base.MousePressed(mouseState, offset);
        }
        public Vector2 GetMousePosition(Vector2? offset = null)
        {
            return new Vector2(UIManager.MouseState.X, UIManager.MouseState.Y) - (offset ?? Vector2.Zero) - Position - SoftOffset;
        }
        public Vector2 SnapPosition(Vector2 position)
        {
            Vector2 newVec = (position / Snap).ToPoint().ToVector2() * Snap;
            if (position.X < 0)
            {
                newVec.X -= Snap.X;
            }
            if (position.Y < 0)
            {
                newVec.Y -= Snap.Y;
            }
            return newVec;
        }
        public override void MouseReleased(MouseState mouseState, Vector2 offset)
        {
            if (!mouseState.MiddlePressed())
            {
                PanIsPressed = false;
            }
            base.MouseReleased(mouseState, offset);
        }
    }
}
