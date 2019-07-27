using PlatformerEngine;
using System;

namespace PlatformerEditor
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new PlatformerEditor())
            {
                Action endCb = ConsoleManager.Start();
                game.Run();
                endCb();
            }
        }
    }
}
