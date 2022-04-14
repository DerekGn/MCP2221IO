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
using MCP2221IO.Gp;
using System.IO;
using System.Text;

namespace MCP2221IO.Settings
{
    public class SramSettings : BaseSettings
    {
        /// <summary>
        /// Enables USB serial number usage during the USB enumeration of the CDC interface.
        /// </summary>
        public bool CdcSerialNumberEnable { get; protected set; }

        /// <summary>
        /// The chip security settings
        /// </summary>
        public SramChipSecurity ChipSecurity { get; set; }

        /// <summary>
        /// The Usb vendor id
        /// </summary>
        public ushort Vid { get; protected set; }

        /// <summary>
        /// The Usb product id
        /// </summary>
        public ushort Pid { get; protected set; }

        /// <summary>
        /// USB Self-Powered Attribute
        /// </summary>
        public UsbSelfPowered SelfPowered { get; protected set; }

        /// <summary>
        /// USB Remote Wake-Up Capability
        /// </summary>
        public UsbRemoteWake RemoteWake { get; protected set; }

        /// <summary>
        /// The current password expressed as an 8 byte hex number
        /// </summary>
        public Password Password { get; protected set; }

        /// <summary>
        /// The GP0 settings
        /// </summary>
        public GpSetting<Gp0Designation> Gp0Settings { get; internal set; }

        /// <summary>
        /// The GP1 settings
        /// </summary>
        public GpSetting<Gp1Designation> Gp1Settings { get; internal set; }

        /// <summary>
        /// The GP2 settings
        /// </summary>
        public GpSetting<Gp2Designation> Gp2Settings { get; internal set; }

        /// <summary>
        /// The GP3 settings
        /// </summary>
        public GpSetting<Gp3Designation> Gp3Settings { get; internal set; }

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
            stringBuilder.AppendLine($"{nameof(Password)}: [{Password}]");
            stringBuilder.AppendLine($"{nameof(Gp0Settings)}: {Gp0Settings}");
            stringBuilder.AppendLine($"{nameof(Gp1Settings)}: {Gp1Settings}");
            stringBuilder.AppendLine($"{nameof(Gp2Settings)}: {Gp2Settings}");
            stringBuilder.AppendLine($"{nameof(Gp3Settings)}: {Gp3Settings}");

            return stringBuilder.ToString();
        }

        internal virtual void Deserialise(Stream stream)
        {
            int temp = stream.ReadByte();

            CdcSerialNumberEnable = (temp & 0x80) == 0x80;
            ChipSecurity = (SramChipSecurity)(temp & 0b11);
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
            PowerRequestMa = stream.ReadByte() * 2;

            byte[] buffer = new byte[8];

            stream.Read(buffer);

            Password = new Password(buffer);

            Gp0Settings = new GpSetting<Gp0Designation>();
            Gp1Settings = new GpSetting<Gp1Designation>();
            Gp2Settings = new GpSetting<Gp2Designation>();
            Gp3Settings = new GpSetting<Gp3Designation>();

            Gp0Settings.Deserialize(stream);
            Gp1Settings.Deserialize(stream);
            Gp2Settings.Deserialize(stream);
            Gp3Settings.Deserialize(stream);
        }
    }
}
