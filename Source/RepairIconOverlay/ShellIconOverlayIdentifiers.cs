using Microsoft.Win32;
using RepairIconOverlay.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepairIconOverlay
{
    class ShellIconOverlayIdentifiers : IShellIconOverlayIdentifiers
    {
        public const string KeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\ShellIconOverlayIdentifiers";
        public List<ShellIconOverlayIdentifier> GetIdentifiers()
        {
            using (var shellIconOverlayIdentifiers = Registry.LocalMachine.OpenSubKey(KeyPath))
            {
                var names = shellIconOverlayIdentifiers.GetSubKeyNames();
                var identifiers = from name in names
                                  select CreateIdentifer(name, shellIconOverlayIdentifiers);
                return identifiers.ToList();
            }
        }

        private ShellIconOverlayIdentifier CreateIdentifer(string name, RegistryKey shellIconOverlayIdentifiers)
        {
            using (var identifierKey = shellIconOverlayIdentifiers.OpenSubKey(name))
            {
                if (identifierKey.SubKeyCount != 0)
                    throw new InvalidOperationException($"The key '{name}' contains sub keys when it should not");

                if (identifierKey.ValueCount != 1)
                    throw new InvalidOperationException($"The key '{name}' contains more than 1 value when it should only contain the default");

                var defaultValue = identifierKey.GetValue(null) as string;
                return new ShellIconOverlayIdentifier(name, defaultValue);
            }
        }

        public void Delete(ShellIconOverlayIdentifier duplicate)
        {
            using (var shellIconOverlayIdentifiers = Registry.LocalMachine.OpenSubKey(KeyPath, true))
            {
                shellIconOverlayIdentifiers.DeleteSubKey(duplicate.OriginalName);
            }
        }

        public void UpdateIdentifier(ShellIconOverlayIdentifier identifier, int newRank)
        {
            throw new NotImplementedException();
        }
    }
}
