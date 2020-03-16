using System;
using System.Collections.Generic;
using System.Linq;
using RepairIconOverlay.Display;
using RepairIconOverlay.Model;

namespace RepairIconOverlay.Commands
{
    class RepairIconOverlayRegistryKeys : ICommand
    {
        private readonly IShellIconOverlayIdentifiers _identifiers;

        class ExecuteOnce : ICommand
        {
            private readonly ICommand _inner;
            private bool? lastResult;

            public ExecuteOnce(ICommand inner)
            {
                _inner = inner;
            }

            public bool Execute(ConsoleDisplay console, string configurationFile)
            {
                if (!lastResult.HasValue)
                    lastResult = _inner.Execute(console, configurationFile);

                return lastResult.Value;
            }
        }

        public RepairIconOverlayRegistryKeys(IShellIconOverlayIdentifiers shellIconOverlayIdentifiers)
        {
            _identifiers = shellIconOverlayIdentifiers;
        }

        public bool Execute(ConsoleDisplay console, string configurationFile)
        {
            console.WriteLine("Repairing shell icon overlays...");
            var serializer = new ConfigurationSerializer();
            var configuration = serializer.Read(configurationFile);

            if(configuration.Sets == null || configuration.Sets.Count == 0)
            {
                console.WriteError($"No sets defined");
                return false;
            }

            var backupTask = new ExecuteOnce(new BackupRegistryKeys());
            var duplicates = GatherDuplicates().ToList();
            if (duplicates.Any())
            {
                if (!backupTask.Execute(console, configurationFile))
                    return false;

                console.WriteLine($"Deleting {duplicates.Count} duplicate identifiers");
                foreach (var duplicate in duplicates)
                    _identifiers.Delete(duplicate);
            }


            var changes = GatherChanges(console, configuration).ToList();
            if (!changes.Any())
            {
                console.WriteLine($"No identifiers need updating");
                return true;
            }
            
            if (!backupTask.Execute(console, configurationFile))
                return false;
            
            console.WriteLine($"Updating {changes.Count} identifiers");
            foreach (var change in changes)
                _identifiers.UpdateIdentifier(change.Item1, change.Item2);

            return new RestartExplorerProcess().Execute(console, configurationFile);
        }

        internal IEnumerable<Tuple<ShellIconOverlayIdentifier, int>> GatherChanges(ConsoleDisplay console, Configuration configuration)
        {
            var flatKeys = (from set in configuration.Sets
                           from key in set.Keys
                           select new
                           {
                               set.Rank,
                               Key = key
                           }).ToList();
            var current = _identifiers.GetIdentifiers();

            var configurationKeys = (from item in flatKeys
                                     select item.Key).ToList();
            var currentKeys = (from item in current
                               select item.Name).ToList();

            var additionalConfigurationKeys = configurationKeys.Except(currentKeys).ToList();
            if (additionalConfigurationKeys.Any())
                console.WriteWarning($"There are {additionalConfigurationKeys.Count} additional keys defined in the configuration (compared with the registry). Repair may not produce expected results.");
            
            var additionalCurrentKeys = currentKeys.Except(configurationKeys).ToList();
            if (additionalCurrentKeys.Any())
                console.WriteWarning($"There are {additionalCurrentKeys.Count} additional identifiers in the registry (compared with the configuration). Repair may not produce expected results.");

            var join = from item in flatKeys
                       join identifier in current on item.Key equals identifier.Name
                       select new
                       {
                           Configuration = item,
                           Current = identifier
                       };

            var changes = from item in @join
                          where item.Configuration.Rank != item.Current.Rank
                          select new Tuple<ShellIconOverlayIdentifier, int>(item.Current, item.Configuration.Rank);

            return changes;
        }

        internal IEnumerable<ShellIconOverlayIdentifier> GatherDuplicates()
        {
            var current = _identifiers.GetIdentifiers();
            var nameGroups = from item in current
                             group item by item.Name into g
                             select new
                             {
                                 Duplicates = g.ToList()
                             };

            var duplicatesOnly = from item in nameGroups
                                 where item.Duplicates.Count != 1
                                 from duplicate in item.Duplicates.Skip(1)
                                 select duplicate;

            return duplicatesOnly;
        }
    }
}
