using RepairIconOverlay.Model;
using System;
using System.IO;
using System.Xml.Serialization;

namespace RepairIconOverlay
{
    class ConfigurationSerializer
    {
        private readonly XmlSerializer _serializer;

        public ConfigurationSerializer()
        {
            _serializer = new XmlSerializer(typeof(Configuration));
        }

        public void Write(string filename, Configuration obj)
        {
            using (var writer = new StreamWriter(filename))
            {
                _serializer.Serialize(writer, obj);
            }
        }

        public Configuration Read(string filename)
        {
            var headerSerializer = new XmlSerializer(typeof(ConfigurationHeader));

            using (var reader = new StreamReader(filename))
            {
                var header = (ConfigurationHeader)headerSerializer.Deserialize(reader);

                if (header.Version != Configuration.SchemaVersion)
                    throw new InvalidOperationException("Configuration file schema version was an unexpected value");
            }

            using (var reader = new StreamReader(filename))
            {
                return (Configuration)_serializer.Deserialize(reader);
            }
        }
    }
}
