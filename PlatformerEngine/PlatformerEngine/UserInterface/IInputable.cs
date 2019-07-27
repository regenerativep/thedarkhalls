using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine.UserInterface
{
    /// <summary>
    /// interface for anything that can have text inputted into it
    /// </summary>
    public interface IInputable
    {
        /// <summary>
        /// the current state of the text of this inputtable object
        /// </summary>
        string Text { get; set; }
        /// <summary>
        /// the valid keys that can be put into this inputtable object
        /// </summary>
        char[] ValidKeys { get; }
    }
}
