using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    /// <summary>
    /// contains general math things for the engine
    /// </summary>
    public static class PlatformerMath
    {
        public static Random RandNumGen = new Random();
        public static T Choose<T>(params T[] arr)
        {
            int ind = RandNumGen.Next(arr.Length);
            return arr[ind];
        }
        /// <summary>
        /// checks if the given rectangles intersect
        /// got from https://stackoverflow.com/questions/306316/determine-if-two-rectangles-overlap-each-other
        /// </summary>
        /// <param name="a">a rectangle</param>
        /// <param name="b">a rectangle</param>
        /// <returns>if the given rectangle intersects the other given rectangle</returns>
        public static bool RectangleInRectangle(Rectangle a, Rectangle b)
        {
            return a.X < b.X + b.Width && a.X + a.Width > b.X && a.Y < b.Y + b.Height && a.Y + a.Height > b.Y; //flipped comparisons for y since original code is for cartesian coords
        }
        /// <summary>
        /// adds (a) vector(s) to the position of a rectangle
        /// </summary>
        /// <param name="rect">the rectangle</param>
        /// <param name="vecs">the vectors to add</param>
        /// <returns>the new rectangle with the added vectors</returns>
        public static Rectangle AddVectorToRect(Rectangle rect, params Vector2[] vecs)
        {
            Rectangle newRect = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
            foreach (Vector2 vec in vecs)
            {
                newRect.X += (int)vec.X;
                newRect.Y += (int)vec.Y;
            }
            return newRect;
        }
        /// <summary>
        /// checks if a point is in a rectangle
        /// </summary>
        /// <param name="rect">the rectangle</param>
        /// <param name="p">the point</param>
        /// <returns></returns>
        public static bool PointInRectangle(Rectangle rect, Vector2 p)
        {
            return p.X > rect.X && p.X <= rect.X + rect.Width && p.Y > rect.Y && p.Y < rect.Y + rect.Height;
        }
        /// <summary>
        /// ceils a vector
        /// </summary>
        /// <param name="val">the vector</param>
        /// <returns>a ceiled version of the given vector</returns>
        public static Vector2 VectorCeil(Vector2 val)
        {
            return new Vector2((float)Math.Ceiling(Math.Abs(val.X)) * Math.Sign(val.X), (float)Math.Ceiling(Math.Abs(val.Y)) * Math.Sign(val.Y));
        }
    }
}
