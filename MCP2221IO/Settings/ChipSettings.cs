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

using MCP2221IO.Extensions;
using System.IO;
using System.Text;

namespace MCP2221IO.Settings
{
    /// <summary>
    /// The chip settings for the device from flash memory
    /// </summary>
    public class ChipSettings : BaseSettings
    {
        /// <summary>
        /// Enables USB serial number usage during the USB enumeration of the CDC interface.
        /// </summary>
        public bool CdcSerialNumberEnable { get; set; }

        /// <summary>
        /// The chip security settings
        /// </summary>
        public ChipSecurity ChipSecurity { get; set; }

        /// <summary>
        /// The Usb vendor id
        /// </summary>
        public ushort Vid { get; set; }

        /// <summary>
        /// The Usb product id
        /// </summary>
        public ushort Pid { get; set; }

        /// <summary>
        /// USB Self-Powered Attribute
        /// </summary>
        public UsbSelfPowered SelfPowered { get; set; }

        /// <summary>
        /// USB Remote Wake-Up Capability
        /// </summary>
        public UsbRemoteWake RemoteWake { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{nameof(CdcSerialNumberEnable)}: {CdcSerialNumberEnable}");
            stringBuilder.AppendLine($"{nameof(ChipSecurity)}: {ChipSecurity}");
            stringBuilder.AppendLine($"{nameof(ClockDutyCycle)}: {ClockDutyCycle}");
            stringBuilder.AppendLine($"{nameof(ClockDivider)}: {ClockDivider}");
            stringBuilder.AppendLine($"{nameof(DacRefVoltage)}: {DacRefVoltage}");
            stringBuilder.AppendLine($"{nameof(DacRefOption)}: {DacRefOption}");
            stringBuilder.AppendLine($"{nameof(DacOutput)}: 0x{DacOutput:X}");
            stringBuilder.AppendLine($"{nameof(InterruptNegativeEdge)}: {InterruptNegativeEdge}");
            stringBuilder.AppendLine($"{nameof(InterruptPositiveEdge)}: {InterruptPositiveEdge}");
            stringBuilder.AppendLine($"{nameof(AdcRefVoltage)}: {AdcRefVoltage}");
            stringBuilder.AppendLine($"{nameof(AdcRefOption)}: {AdcRefOption}");
            stringBuilder.AppendLine($"{nameof(Vid)}: 0x{Vid:X}");
            stringBuilder.AppendLine($"{nameof(Pid)}: 0x{Pid:X}");
            stringBuilder.AppendLine($"{nameof(SelfPowered)}: {SelfPowered}");
            stringBuilder.AppendLine($"{nameof(RemoteWake)}: {RemoteWake}");
            stringBuilder.AppendLine($"{nameof(PowerRequestMa)}: 0x{PowerRequestMa:X}");

            return stringBuilder.ToString();
        }

        internal void Deserialize(Stream stream)
        {
            stream.ReadByte();
            stream.ReadByte();

            int temp = stream.ReadByte();

            CdcSerialNumberEnable = (temp & 0x80) == 0x80;
            ChipSecurity = (ChipSecurity)(temp & 0b11);
            temp = stream.ReadByte();
            ClockDutyCycle = (ClockDutyCycle)((temp & 0x18) >> 3);
            ClockDivider = (ClockOutDivider)(temp & 0x07);

            temp = stream.ReadByte();

            DacRefVoltage = (DacRefVoltage)((temp & 0xC0) >> 6);
            DacRefOption = (DacRefOption)((temp & 0x10) >> 4);
            DacOutput = (byte)(temp & 0x0F);

            temp = stream.ReadByte();

            InterruptNegativeEdge = (temp & 0x40) == 0x40;
            InterruptPositiveEdge = (temp & 0x20) == 0x20;
            AdcRefVoltage = (AdcRefVoltage)((temp & 0x18) >> 3);
            AdcRefOption = (AdcRefOption)((temp & 0x4) >> 2);

            Vid = stream.ReadUShort();
            Pid = stream.ReadUShort();

            temp = stream.ReadByte();

            SelfPowered = (UsbSelfPowered)((temp & 0x40) >> 6);
            RemoteWake = (UsbRemoteWake)((temp & 0x20) >> 5);
            _powerRequestMa = stream.ReadByte();
        }

        internal void Serialize(Stream stream)
        {
            int update = CdcSerialNumberEnable ? 0x80 : 0x0;
            update |= (int)ChipSecurity;

            stream.WriteByte((byte)update);

            update |= (int)ClockDivider;
            update |= ((int)ClockDutyCycle << 3);
            stream.WriteByte((byte)update);

            update |= DacOutput;
            update |= ((int)DacRefOption << 4);
            update |= ((int)DacRefVoltage << 5);

            stream.WriteByte((byte)update);

            update |= InterruptNegativeEdge ? 1 : 0 << 6;
            update |= InterruptPositiveEdge ? 1 : 0 << 5;
            update |= (int)AdcRefVoltage << 3;
            update |= (int)AdcRefOption << 2;

            stream.WriteByte((byte)update);

            stream.WriteUShort(Vid);
            stream.WriteUShort(Pid);

            update |= (int)SelfPowered << 6;
            update |= (int)RemoteWake << 5;

            stream.WriteByte((byte)update);
            stream.WriteByte((byte)_powerRequestMa);
        }
    }
}