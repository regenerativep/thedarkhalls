using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    /// <summary>
    /// performs and contains information for transitions
    /// </summary>
    public interface ITransition
    {
        /// <summary>
        /// start performing a transition
        /// </summary>
        /// <param name="isOpening">false if ending a room, true if opening a room</param>
        /// <param name="completionCallback">called when transition completes</param>
        /// <returns>should return this transition</returns>
        ITransition Perform(bool isOpening, Action completionCallback = null);
        /// <summary>
        /// updates the transition
        /// </summary>
        void Update();
        /// <summary>
        /// draws the current state of the transition
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to draw to</param>
        void Draw(SpriteBatch spriteBatch);
        /// <summary>
        /// if the transition is currrently active
        /// </summary>
        bool IsActive { get; }
    }
}
