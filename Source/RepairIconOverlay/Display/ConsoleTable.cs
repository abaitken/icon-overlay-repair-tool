using System;
using System.Linq;

namespace RepairIconOverlay.Display
{
    class ConsoleTable
    {
        private readonly ConsoleDisplay _console;

        public ConsoleTable(ConsoleDisplay console)
        {
            _console = console;
        }

        public void Write(string[] col1, string[] col2, string prefix, int minSpacing = 1)
        {
            if (col1.Length != col2.Length)
                throw new ArgumentOutOfRangeException();

            var widestValues = col1.Max(i => i.Length);

            for (int i = 0; i < col1.Length; i++)
            {
                var cell1 = col1[i];
                var cell2 = col2[i];

                var spacing = new string(' ', widestValues - cell1.Length + minSpacing);
                _console.WriteLine($"{prefix}{cell1}{spacing}{cell2}");
            }
        }
    }
}