namespace MCP2221IO.Commands
{
    internal class ReadChipSettingsCommand : ReadFlashDataCommand
    {
        public ReadChipSettingsCommand() : base(ReadFlashSubCode.ReadChipSettings)
        {
        }
    }
}
