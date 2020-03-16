using RepairIconOverlay.Display;
using RepairIconOverlay.Exceptions;
using System.IO;

namespace RepairIconOverlay.Commands
{
    class ValidateConfiguration : ICommand
    {
        public bool Execute(ConsoleDisplay console, string configurationFile)
        {
            if (!File.Exists(configurationFile))
            {
                console.WriteError("Configuration file not found!");
                return false;
            }

            try
            {
                var serializer = new ConfigurationSerializer();
                var configuration = serializer.Read(configurationFile);
            }
            catch (ConfigurationSchemaVersionException)
            {
                console.WriteError("Configuration file is for a different version and is unsupported in this version.");
                return false;
            }
            return true;
        }
    }
}
