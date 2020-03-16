using RepairIconOverlay.Commands;
using System;
using System.Collections.Generic;
using System.IO;

namespace RepairIconOverlay
{
    internal class App
    {
        private ConsoleDisplay _console;

        public App(ConsoleDisplay consoleDisplay)
        {
            _console = consoleDisplay;
        }

        internal int Run(string[] args)
        {
            DisplayAppInfo();

            if (!new ShellIconOverlayIdentifiers().CheckKeyAccess(out var errorMessage))
            {
                _console.WriteError(errorMessage);
                _console.WriteLine("Consider running with elevated permissions.");
                _console.WriteLine();
                return ExitCodes.Error;
            }

            var commandLineParser = new CommandLineParser(args);

            if (commandLineParser.Count == 0 || commandLineParser.ContainsCommand('h', '?'))
            {
                DisplayHelpText();
                return ExitCodes.Error;
            }

            var configurationFileSpecified = commandLineParser.GetCommandParameter('c', out var configurationFile);
            if (!configurationFileSpecified || string.IsNullOrWhiteSpace(configurationFile))
            {
                _console.WriteError("Configuration file must be specified!");
                _console.WriteLine();
                return ExitCodes.Error;
            }

            var commands = CreateCommands(commandLineParser);

            try
            {
                foreach (var command in commands)
                    if (!command.Execute(_console, configurationFile))
                    {
                        _console.WriteLine();
                        return ExitCodes.Error;
                    }
            }
            catch (Exception ex)
            {
                _console.WriteError("An exception has been thrown:");
                _console.WriteError($@"{ex.Message}
{ex.StackTrace}");
            }
            _console.WriteLine();
            return ExitCodes.OK;
        }

        private IEnumerable<ICommand> CreateCommands(CommandLineParser commandLineParser)
        {
            if (commandLineParser.ContainsCommand('n'))
            {
                yield return new CreateNewConfigurationFile();
                yield break;
            }

            yield return new ValidateConfiguration();
            yield return new RepairIconOverlayRegistryKeys(new ShellIconOverlayIdentifiers());
        }

        private void DisplayHelpText()
        {
            var commands = new[]
            {
                new { Commands = "h,?", Text = "This help" },
                new { Commands = "c", Text = "Configuration file" },
                new { Commands = "n", Text = "Create new configuration file based on current registry keys" },
                //new { Commands = "u", Text = "Update configuration file based on current registry keys" } // TODO : Implement update command
            };

            const int maxSpacing = 6;
            foreach (var item in commands)
            {
                var spacing = new string(' ', maxSpacing - item.Commands.Length);
                _console.WriteLine($"{item.Commands}{spacing}{item.Text}");
            }

            _console.WriteLine();

            var exitCodes = new[]
            {
                new { ExitCode = ExitCodes.OK, Text = "OK" },
                new { ExitCode = ExitCodes.Error, Text = "Error" },
            };

            foreach (var item in exitCodes)
            {
                var spacing = new string(' ', maxSpacing - item.ExitCode.ToString().Length);
                _console.WriteLine($"{item.ExitCode}{spacing}{item.Text}");
            }

            _console.WriteLine();
        }

        private void DisplayAppInfo()
        {
            var info = new AppInfo();
            _console.WriteLine($"{info.Title} {info.Version}");
            _console.WriteLine($"{info.Company} {info.Copyright}");
            _console.WriteLine(info.Description);
            _console.WriteLine();
        }
    }
}