namespace MCP2221IO.Commands
{
    internal class ReadGpioPortsCommand : ReadFlashDataCommand
    {
        public ReadGpioPortsCommand() : base(ReadFlashSubCode.ReadGpSettings)
        {

        }
    }
}
