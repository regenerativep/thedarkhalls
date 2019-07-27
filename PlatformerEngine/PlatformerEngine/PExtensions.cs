using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    /// <summary>
    /// extensions for drawing from a spritebatch
    /// </summary>
    public static class PExtensions
    {
        /// <summary>
        /// draws a line to the spritebatch
        /// </summary>
        /// <param name="sb">this spritebatch</param>
        /// <param name="a">beginning point of the line</param>
        /// <param name="b">ending point of the line</param>
        /// <param name="color">color of the line</param>
        public static void DrawLine(this SpriteBatch sb, Vector2 a, Vector2 b, Color color)
        {
            DrawLine(sb, a, b, color, new LayerData(0));
        }
        /// <summary>
        /// a texture with a single white pixel
        /// </summary>
        private static Texture2D singlePixel = null;
        /// <summary>
        /// draws a line to the spritebatch
        /// </summary>
        /// <param name="sb">this spritebatch</param>
        /// <param name="a">beginning point of the line</param>
        /// <param name="b">ending point of the line</param>
        /// <param name="color">color of the line</param>
        /// <param name="depth">layer to draw the line at</param>
        public static void DrawLine(this SpriteBatch sb, Vector2 a, Vector2 b, Color color, LayerData layer)
        {
            if(singlePixel == null)
            {
                singlePixel = new Texture2D(sb.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                singlePixel.SetData(new Color[] { Color.White });
            }
            Vector2 dist = b - a;
            float angle = (float)Math.Atan2(dist.Y, dist.X);
            sb.Draw(singlePixel, a, null, color, angle, Vector2.Zero, new Vector2(dist.Length(), 1), SpriteEffects.None, layer.ActualLayer);
        }
        /// <summary>
        /// draws a rectangle
        /// </summary>
        /// <param name="sb">the spritebatch to draw to</param>
        /// <param name="a">top left</param>
        /// <param name="b">bottom right</param>
        /// <param name="color">color of rectangle</param>
        /// <param name="layer">layer to draw at</param>
        public static void DrawRectangle(this SpriteBatch sb, Vector2 a, Vector2 b, Color color, LayerData layer)
        {
            DrawRectangle(sb, a, b, color, layer.ActualLayer);
        }
        /// <summary>
        /// draws a rectangle
        /// </summary>
        /// <param name="sb">the spritebatch to draw to</param>
        /// <param name="a">top left</param>
        /// <param name="b">bottom right</param>
        /// <param name="color">color of rectangle</param>
        /// <param name="layer">layer to draw at</param>
        public static void DrawRectangle(this SpriteBatch sb, Vector2 a, Vector2 b, Color color, float layer)
        {
            if (singlePixel == null)
            {
                singlePixel = new Texture2D(sb.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                singlePixel.SetData(new Color[] { Color.White });
            }
            sb.Draw(singlePixel, a, null, color, 0f, Vector2.Zero, b - a, SpriteEffects.None, layer);
        }
        /// <summary>
        /// draws a rectangle
        /// </summary>
        /// <param name="sb">the spritebatch to draw to</param>
        /// <param name="a">top left</param>
        /// <param name="b">bottom right</param>
        /// <param name="color">color of rectangle</param>
        /// <param name="outlineColor">outline color of rectangle</param>
        /// <param name="layer">layer to draw at</param>
        /// <param name="thickness">the thickness of the outline</param>
        public static void DrawOutlinedRectangle(this SpriteBatch sb, Vector2 a, Vector2 b, Color color, Color outlineColor, float layer, int thickness = 1)
        {
            DrawRectangle(sb, a, b, outlineColor, layer);
            Vector2 thicknessVector = new Vector2(thickness);
            DrawRectangle(sb, a + thicknessVector, b - thicknessVector, color, layer + 0.001f);
        }
        /// <summary>
        /// draws an X
        /// </summary>
        /// <param name="sb">this spritebatch</param>
        /// <param name="pos">the center location of the X</param>
        /// <param name="size">width of the X</param>
        public static void DrawX(this SpriteBatch sb, Vector2 pos, float size, LayerData layer)
        {
            Vector2 sizeVec = new Vector2(size / 2);
            DrawLine(sb, pos - sizeVec, pos + sizeVec, Color.Black, layer);
            sizeVec.Y = -sizeVec.Y;
            DrawLine(sb, pos - sizeVec, pos + sizeVec, Color.Black, layer);
        }
        public static bool LeftPressed(this MouseState ms)
        {
            return ms.LeftButton == ButtonState.Pressed;
        }
        public static bool RightPressed(this MouseState ms)
        {
            return ms.RightButton == ButtonState.Pressed;
        }
        public static bool MiddlePressed(this MouseState ms)
        {
            return ms.MiddleButton == ButtonState.Pressed;
        }
    }
}
