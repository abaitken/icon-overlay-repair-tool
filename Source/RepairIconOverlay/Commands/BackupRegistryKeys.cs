using System;
using System.Diagnostics;
using System.IO;

namespace RepairIconOverlay.Commands
{
    class BackupRegistryKeys : ICommand
    {
        public bool Execute(ConsoleDisplay console, string configurationFile)
        {
            var externalProcess = new ExternalProcessHandler(console);

            var datestring = DateTime.Now.ToString("o").Replace(":", ".");
            var backupFile = $"{datestring}.reg";
            console.WriteLine($"Creating registry backup: {backupFile}");

            var registryKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\ShellIconOverlayIdentifiers";
            var commandLineArguments = $@"export {registryKey} {backupFile}";

            var fullPath = Path.GetFullPath(configurationFile);
            var workingDirectory = Path.GetDirectoryName(fullPath);
            var result = externalProcess.Run(workingDirectory, commandLineArguments, "reg");

            var success = (result == 0);
            if (!success)
                console.WriteError("Backup process failed");

            return success;
        }
    }

}
