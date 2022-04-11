
using CommandLine;

namespace MCP2221IOConsole.Commands
{
    [Verb("chip", HelpText = "Access device Chip settings")]
    internal class ChipCommands : BaseCommand
    {
        [Option('r', "read", SetName = "Read", Required = false, HelpText = "Read the chip settings")]
        public bool Read { get; set; }

        [Option('w', "write", SetName = "Write", Required = false, HelpText = "Write the chip settings")]
        public bool Write { get; set; }
    }
}
