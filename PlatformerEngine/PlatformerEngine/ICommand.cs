using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerEngine
{
    /// <summary>
    /// interface for creating a command
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// the number of arguments to input into this command
        /// </summary>
        int ArgumentCount { get; }
        /// <summary>
        /// name of the command internally
        /// </summary>
        string Name { get; }
        /// <summary>
        /// what the commmand can be triggered by
        /// </summary>
        string CallCommand { get; }
        /// <summary>
        /// calls the command (don't know if invoke is the best name, i just know anonymous functions also use it)
        /// </summary>
        /// <param name="args">the arguments to put into the command</param>
        void Invoke(params string[] args);
    }
}
