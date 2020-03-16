using System.Diagnostics;

namespace RepairIconOverlay
{
    class ExternalProcessHandler
    {
        private readonly ConsoleDisplay _console;

        public ExternalProcessHandler(ConsoleDisplay console)
        {
            _console = console;
        }

        public int Run(string workingDirectory, string commandLine, string program, bool shellExecute = false)
        {
            var startInfo = !string.IsNullOrWhiteSpace(commandLine)
                                ? new ProcessStartInfo(program, commandLine)
                                : new ProcessStartInfo(program);
            if (!string.IsNullOrWhiteSpace(workingDirectory))
                startInfo.WorkingDirectory = workingDirectory;

            startInfo.UseShellExecute = shellExecute;
            if (!shellExecute)
            {
                startInfo.RedirectStandardError = true;
                startInfo.RedirectStandardOutput = true;
            }

            using (var process = new Process
            {
                StartInfo = startInfo
            })
            {
                if (!shellExecute)
                {
                    process.OutputDataReceived += Process_OutputDataReceived;
                    process.ErrorDataReceived += Process_ErrorDataReceived;
                }

                process.Start();

                if (!shellExecute)
                    process.BeginOutputReadLine();

                process.WaitForExit();
                var exitCode = process.ExitCode;

                process.Close();
                return exitCode;
            }
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
                _console.WriteError(e.Data);
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
                _console.WriteLine(e.Data);
        }
    }
}