/*
* MIT License
*
* Copyright (c) 2022 Derek Goslin https://github.com/DerekGn
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

using MCP2221IO.Settings;
using System;
using System.IO;

namespace MCP2221IO.Commands
{
    /// <summary>
    /// 
    /// </summary>
    internal class WriteSramSettingsCommand : BaseCommand
    {
        public WriteSramSettingsCommand(SramSettings sramSettings, bool clearInterrupts) : base(CommandCodes.SetSram)
        {
            SramSettings = sramSettings ?? throw new ArgumentNullException(nameof(sramSettings));
            ClearInterrupts = clearInterrupts;
        }

        public SramSettings SramSettings { get; }

        public bool ClearInterrupts { get; }

        public override void Serialize(Stream stream)
        {
            base.Serialize(stream);

            int update = 0x80;
            update |= (int)SramSettings.ClockDutyCycle << 3;
            update |= (int)SramSettings.ClockDivider & 0b111;

            stream.WriteByte((byte)update);

            update = 0x80;
            update |= (int)SramSettings.DacRefVoltage << 3;
            update |= (int)SramSettings.DacRefOption;

            stream.WriteByte((byte)update);

            update = 0x80;
            update |= SramSettings.DacOutput & 0x0F;

            stream.WriteByte((byte)update);

            update = 0x80;
            update |= ((int)SramSettings.AdcRefVoltage & 0x06) << 2;
            update |= (int)SramSettings.AdcRefOption;

            stream.WriteByte((byte)update);

            update = 0x80;
            update |= SramSettings.InterruptPositiveEdge ? 0x03 : 0x00;
            update |= SramSettings.InterruptNegativeEdge ? 0x02 : 0x00;
            update |= ClearInterrupts ? 0x01 : 0x00;

            stream.WriteByte((byte)update);

            stream.WriteByte(0x80);

            SramSettings.Gp0Settings.Serialize(stream);
            SramSettings.Gp1Settings.Serialize(stream);
            SramSettings.Gp2Settings.Serialize(stream);
            SramSettings.Gp3Settings.Serialize(stream);
        }
    }
}
