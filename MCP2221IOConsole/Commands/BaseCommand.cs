using CommandLine;

namespace MCP2221IOConsole.Commands
{
    class BaseCommand
    {
        [Option('v', "vid", Required = false, HelpText = "The VID of the MCP2221", Default = 0x04D8)]
        public int Vid { get; set; }
        [Option('p', "pid", Required = false, HelpText = "The PID of the MCP2221", Default = 0x00DD)]
        public int Pid { get; set; }
        [Option('s', "serial", Required = false, HelpText = "The serial number of the MCP2221 instance to use")]
        public string SerialNumber { get; set; }
    }
}
