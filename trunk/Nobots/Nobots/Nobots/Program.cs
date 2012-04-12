using System;

namespace Nobots
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            using (MainGame game = new MainGame())
            {
                game.Run();
            }
        }
    }
#endif
}

