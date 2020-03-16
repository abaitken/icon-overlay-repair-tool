using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RepairIconOverlay.Model
{

    [XmlRoot("Configuration")]
    public class ConfigurationHeader
    {
        [XmlAttribute]
        public int Version { get; set; }
    }

    [XmlRoot("Configuration")]
    public class Configuration
    {
        public const int SchemaVersion = 1;

        public Configuration()
        {
            Version = SchemaVersion;
        }

        [XmlAttribute]
        public int Version { get; set; }

        public List<KeySet> Sets { get; set; }
    }

    public class KeySet
    {
        public int Rank { get; set; }
        public List<string> Keys { get; set; }
    }
}
