using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairIconOverlay.Commands
{
    interface ICommand
    {
        void Execute(ConsoleDisplay console, string configurationFile);
    }
}
