using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairIconOverlay.Model
{
    class ShellIconOverlayIdentifier
    {
        public ShellIconOverlayIdentifier(string originalName, string identifier)
        {
            if (string.IsNullOrWhiteSpace(originalName))
                throw new ArgumentException("Expected a value", nameof(originalName));
            if (string.IsNullOrWhiteSpace(identifier))
                throw new ArgumentException($"Expected a value", nameof(identifier));

            OriginalName = originalName;
            Identifier = identifier;
            Rank = CalculateRank(originalName);
            Name = originalName.TrimStart();
        }

        public string OriginalName { get; }
        public string Identifier { get; }
        public int Rank { get; }
        public string Name { get; }

        private static int CalculateRank(string name)
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
