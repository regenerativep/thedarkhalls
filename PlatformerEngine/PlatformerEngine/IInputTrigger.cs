using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    public interface IInputTrigger
    {
        /// <summary>
        /// the key
        /// </summary>
        Keys Key { get; set; }
        /// <summary>
        /// if the key is pressed
        /// </summary>
        bool Pressed { get; set; }
        /// <summary>
        /// what to call when key is pressed
        /// </summary>
        Action<bool> Callback { get; set; }
        /// <summary>
        /// check for any updates in the input
        /// </summary>
        /// <param name="keyState">the keyboard state to check from</param>
        void Update(KeyboardState keyboardState);
    }
}
