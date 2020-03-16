using System;
using System.Diagnostics;

namespace RepairIconOverlay
{
    class Program
    {
        static void Main(string[] args)
        {
            new App(new ConsoleDisplay()).Run(args);
#if DEBUG
            if (Debugger.IsAttached)
            {
                Console.WriteLine("DEBUG: Press any key to exit");
                Console.ReadKey();
            }
#endif
        }
    }
}
