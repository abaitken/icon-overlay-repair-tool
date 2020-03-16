using RepairIconOverlay.Display;
using RepairIconOverlay.Model;
using System.Linq;

namespace RepairIconOverlay.Commands
{
    class CreateNewConfigurationFile : ICommand
    {
        public bool Execute(ConsoleDisplay console, string configurationFile)
        {
            var identfiers = new ShellIconOverlayIdentifiers();
            var keySets = (from identifier in identfiers.GetIdentifiers()
                          group identifier by identifier.Rank into g
                           select new KeySet
                           {
                               Rank = g.Key,
                               Keys = (from item in g
                                       select item.Name).ToList()
                           }).ToList();

            var configuration = new Configuration
            {
                Sets = keySets
            };
            var serializer = new ConfigurationSerializer();
            serializer.Write(configurationFile, configuration);
            return true;
        }
    }
}
