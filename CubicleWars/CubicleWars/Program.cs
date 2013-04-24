using System;

namespace CubicleWars
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (var game = new Startup())
            {
                game.Run();
            }
        }
    }
#endif
}

