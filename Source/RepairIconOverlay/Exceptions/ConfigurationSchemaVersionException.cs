using System;

namespace RepairIconOverlay.Exceptions
{
    public class ConfigurationSchemaVersionException : Exception
    {
        public ConfigurationSchemaVersionException()
            : base("Configuration file schema version was an unexpected value")
        {

        }
    }
}
