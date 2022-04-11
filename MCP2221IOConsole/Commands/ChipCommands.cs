
using McMaster.Extensions.CommandLineUtils;
using System;

namespace MCP2221IOConsole.Commands
{
    [Command("chip", Description = "Access device Chip settings")]
    [Subcommand(typeof(ReadChipSettingsCommand))]
    internal class ChipCommand : BaseCommand
    {
        public ChipCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
