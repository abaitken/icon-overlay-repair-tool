using System.Collections.Generic;
using RepairIconOverlay.Model;

namespace RepairIconOverlay
{
    interface IShellIconOverlayIdentifiers
    {
        void Delete(ShellIconOverlayIdentifier duplicate);
        List<ShellIconOverlayIdentifier> GetIdentifiers();
        void UpdateIdentifier(ShellIconOverlayIdentifier identifier, int newRank);
    }
}