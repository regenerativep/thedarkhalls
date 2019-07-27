using System;
using PlatformerEngine;

namespace PlatformerTestGame
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        public static PlatformerGame Game;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (Game = new PlatformerGame())
            {
                //Action endCb = ConsoleManager.Start();
                Game.Run();
                //endCb();
            }
        }
    }
#endif
}
