using Microsoft.Win32;
using RepairIconOverlay.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepairIconOverlay.Commands
{
    class CreateNewConfigurationFile : ICommand
    {
        public void Execute(ConsoleDisplay console, string configurationFile)
        {
            var configuration = new Configuration
            {
                Sets = GetKeySets()
            };
            var serializer = new ConfigurationSerializer();
            serializer.Write(configurationFile, configuration);
        }

        private List<KeySet> GetKeySets()
        {
            using (var ShellIconOverlayIdentifiers = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\ShellIconOverlayIdentifiers"))
            {
                var names = ShellIconOverlayIdentifiers.GetSubKeyNames();
                var rankedNames = from name in names
                                  select new
                                  {
                                      name = name.TrimStart(),
                                      rank = CalculateRank(name)
                                  };

                var sets = from name in rankedNames
                           group name by name.rank into g
                           select new KeySet
                           {
                               Rank = g.Key,
                               Keys = (from item in g
                                       select item.name).ToList()
                           };
                return sets.ToList();
            }
        }

        private int CalculateRank(string name)
        {
            for (int i = 0; i < name.Length; i++)
            {
                var c = name[i];
                if (c != ' ')
                    return i;
            }
            throw new ArgumentException();
        }
    }
}
