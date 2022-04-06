using MCP2221IO.Settings;
using System;
using System.IO;

namespace MCP2221IO.Commands
{
    internal class WriteChipSettingsCommand : WriteFlashDataCommand
    {
        public WriteChipSettingsCommand(ChipSettings chipSettings) : base(WriteFlashSubCode.WriteChipSettings)
        {
            ChipSettings = chipSettings;
        }

        public ChipSettings ChipSettings { get; }

        public override void Serialise(Stream stream)
        {
            base.Serialise(stream);

            int update = ChipSettings.CdcSerialNumberEnable ? 0x80 : 0x0;
            update |= (int)ChipSettings.ChipSecurity;

            stream.WriteByte((byte)update);

            throw new NotImplementedException();
        }
    }
}
