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

        internal void Run(string[] args)
        {
            DisplayAppInfo();
            // TODO : Check for elevated privilages

            var commandLineParser = new CommandLineParser(args);
            
            if(commandLineParser.Count == 0 || commandLineParser.ContainsCommand('h', '?'))
            {
                DisplayHelpText();
                return;
            }

            var configurationFileSpecified = commandLineParser.GetCommandParameter('c', out var configurationFile);
            if (!configurationFileSpecified || string.IsNullOrWhiteSpace(configurationFile))
            {
                _console.WriteError("Configuration file must be specified!");
                return;
            }

            var commands = CreateCommands(commandLineParser, configurationFile);

            try
            {
                foreach (var command in commands)
                    if (!command.Execute(_console, configurationFile))
                        break;
            }
            catch (Exception ex)
            {
                _console.WriteError("An exception has been thrown:");
                _console.WriteError($@"{ex.Message}
{ex.StackTrace}");
            }
        }

        private IEnumerable<ICommand> CreateCommands(CommandLineParser commandLineParser, string configurationFile)
        {
            if(commandLineParser.ContainsCommand('n'))
            {
                yield return new CreateNewConfigurationFile();
                yield break;
            }

            if (!File.Exists(configurationFile))
            {
                _console.WriteError("Configuration file not found!");
                yield break;
            }

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
        }

        private void DisplayAppInfo()
        {
            var info = new AppInfo();
            _console.WriteLine($"{info.Title} {info.Version}");
            _console.WriteLine(info.Description);
            _console.WriteLine();
        }
    }
}