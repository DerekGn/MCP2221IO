
using McMaster.Extensions.CommandLineUtils;
using System;

namespace MCP2221IOConsole.Commands
{
    [Command(Description = "Reset the device")]
    internal class ResetCommand : BaseCommand
    {
        public ResetCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
