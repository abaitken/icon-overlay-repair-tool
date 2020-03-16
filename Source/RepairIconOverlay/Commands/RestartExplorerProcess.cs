using System;
using System.Diagnostics;

namespace RepairIconOverlay.Commands
{
    class RestartExplorerProcess : ICommand
    {
        public bool Execute(ConsoleDisplay console, string configurationFile)
        {
            console.WriteLine("Restarting explorer process..");

            foreach (var process in Process.GetProcessesByName("explorer.exe"))
                process.Kill();

            Process.Start("explorer.exe");

            return true;
        }
    }
}
