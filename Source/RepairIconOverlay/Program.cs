using RepairIconOverlay.Display;
using System;
using System.Diagnostics;

namespace RepairIconOverlay
{
    class Program
    {
        static int Main(string[] args)
        {
            var exitCode = new App(new ConsoleDisplay()).Run(args);
#if DEBUG
            if (Debugger.IsAttached)
            {
                Console.WriteLine("DEBUG: Press any key to exit");
                Console.ReadKey();
            }
#endif

            return exitCode;
        }
    }
}
