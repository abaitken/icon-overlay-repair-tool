using Microsoft.Win32;

namespace RepairIconOverlay
{
    static class RegistryKeyExtensions
    {
        public static void RenameSubKey(this RegistryKey parent, string source, string destination)
        {
            using (var sourceKey = parent.OpenSubKey(source))
            {
                using (var destinationKey = parent.CreateSubKey(destination, true))
                {
                    CopyTree(sourceKey, destinationKey);
                }
            }

            parent.DeleteSubKeyTree(source);
        }

        private static void CopyTree(RegistryKey sourceKey, RegistryKey destinationKey)
        {
            foreach (var valueName in sourceKey.GetValueNames())
            {
                var value = sourceKey.GetValue(valueName);
                var kind = sourceKey.GetValueKind(valueName);
                destinationKey.SetValue(valueName, value, kind);
            }

            foreach (var subkey in sourceKey.GetSubKeyNames())
            {
                using (var sourceSubkey = sourceKey.OpenSubKey(subkey))
                {
                    using (var destinationSubKey = destinationKey.CreateSubKey(subkey, true))
                    {
                        CopyTree(sourceSubkey, destinationSubKey);
                    }
                }
            }
        }
    }
}
