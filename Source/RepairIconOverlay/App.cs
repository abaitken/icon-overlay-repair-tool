using RepairIconOverlay.Commands;
using RepairIconOverlay.Display;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                new { Commands = "-h", Text = "Displays this help" },
                new { Commands = "-c filepath", Text = "Specifies the configuration file" },
                new { Commands = "-n", Text = "Create new configuration file based on current registry keys" },
                //new { Commands = "u", Text = "Update configuration file based on current registry keys" } // TODO : Implement update command
            };

            _console.WriteLine("Options:");

            new ConsoleTable(_console).Write(commands.Select(i => i.Commands).ToArray(),
                                             commands.Select(i => i.Text).ToArray(),
                                             "   ", 3);

            _console.WriteLine();

            var exitCodes = new[]
            {
                new { ExitCode = ExitCodes.OK, Text = "OK" },
                new { ExitCode = ExitCodes.Error, Text = "Error" },
            };

            _console.WriteLine("Exit codes:");


            new ConsoleTable(_console).Write(exitCodes.Select(i => i.ExitCode.ToString()).ToArray(),
                                             exitCodes.Select(i => i.Text).ToArray(),
                                             "   ", 3);

            _console.WriteLine();
        }

        private void DisplayAppInfo()
        {
            var info = new AppInfo();
            _console.WriteLine();
            _console.WriteLine($"{info.Title} v{info.Version}");
            _console.WriteLine($"{info.Company} - {info.Copyright}");
            _console.WriteLine();
            _console.WriteLine(info.Description);
            _console.WriteLine();
        }
    }
}