using System;

namespace RepairIconOverlay.Display
{
    internal class ConsoleDisplay
    {
        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }

        public void WriteWarning(string value)
        {
            using (new ColorScope(ConsoleColor.Yellow))
                Console.WriteLine($"WARNING: {value}");
        }

        public void WriteError(string value)
        {
            using (new ColorScope(ConsoleColor.Red))
                Console.WriteLine($"ERROR: {value}");
        }

        class ColorScope : IDisposable
        {
            private readonly ConsoleColor _previousColor;
            public ColorScope(ConsoleColor newColor)
            {
                _previousColor = Console.ForegroundColor;
                Console.ForegroundColor = newColor;
            }

            private bool _disposed = false;

            public void Dispose()
            {
                if (!_disposed)
                {
                    Console.ForegroundColor = _previousColor;

                    _disposed = true;
                }
            }
        }
    }
}