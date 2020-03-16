using RepairIconOverlay.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RepairIconOverlay
{
    class ConfigurationSerializer
    {
        public void Write(string filename, Configuration obj)
        {
            var serializer = new XmlSerializer(typeof(Configuration));
            using (var writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, obj);
            }
        }
    }
}
