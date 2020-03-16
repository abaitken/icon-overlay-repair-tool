using System;
using System.Linq;

namespace RepairIconOverlay
{
    class CommandLineParser
    {
        private readonly string[] _args;
        private char[] _commandChars = new[] { '-', '/' };

        public CommandLineParser(string[] args)
        {
            _args = args;
        }

        /// <summary>
        /// Resolves a command and its parameter
        /// </summary>
        /// <param name="commandText">Command to find</param>
        /// <param name="commandParameter">Command parameter or null if no subsequent valid argument was specified</param>
        /// <returns>True if the command exists otherwise false</returns>
        public bool GetCommandParameter(char commandText, out string commandParameter)
        {
            return GetCommandWithParameter(commandText.ToString(), out commandParameter);
        }

        /// <summary>
        /// Resolves a command and its parameter
        /// </summary>
        /// <param name="commandText">Command to find</param>
        /// <param name="commandParameter">Command parameter or null if no subsequent valid argument was specified</param>
        /// <returns>True if the command exists otherwise false</returns>
        public bool GetCommandWithParameter(string commandText, out string commandParameter)
        {
            if(!MatchArg(commandText, out var index))
            {
                commandParameter = null;
                return false;
            }

            if(index + 1 >= Count)
            {
                commandParameter = null;
                return true;
            }

            var nextArg = this[index + 1];
            if(_commandChars.Any(i => nextArg.StartsWith(i.ToString())))
            {
                commandParameter = null;
                return true;
            }

            commandParameter = nextArg;
            return true;
        }

        public bool ContainsCommand(params char[] commandText)
        {
            return commandText.Any(i => ContainsCommand(i));
        }

        public bool ContainsCommand(params string[] commandText)
        {
            return commandText.Any(i => ContainsCommand(i));
        }

        public bool ContainsCommand(char commandText)
        {
            return ContainsCommand(commandText.ToString());
        }

        public bool ContainsCommand(string commandText)
        {
            return MatchArg(commandText, out _);
        }

        public bool MatchArg(string commandText, out int index)
        {
            for (int i = 0; i < _args.Length; i++)
            {
                var arg = _args[i];
                if(MatchArg(arg, commandText))
                {
                    index = i;
                    return true;
                }
            }

            index = -1;
            return false;
        }

        private bool MatchArg(string arg, string commandText)
        {
            // Assume arg.Length includes a command, 
            // so if arg is equal to or less then the expected command, 
            // it is not valid
            if (arg.Length <= commandText.Length)
                return false;

            if (!_commandChars.Any(i => arg.StartsWith(i.ToString())))
                return false;

            var argWithoutCommand = arg.Substring(1);

            return argWithoutCommand.Equals(commandText, StringComparison.CurrentCultureIgnoreCase);
        }

        public int Count => _args.Length;

        public string this[int index] => _args[index];
    }
}