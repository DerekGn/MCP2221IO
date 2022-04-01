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

using System.IO;
using System.Text;

namespace MCP2221IO
{
    public class ChipSettings
    {
        /// <summary>
        /// Enables USB serial number usage during the USB enumeration of the CDC interface.
        /// </summary>
        public bool CdcSerialNumberEnable { get; private set; }

        public ChipSecurity ChipSecurity { get; private set; }

        /// <summary>
        /// The current clock out divider value
        /// </summary>
        public ClockOutDivider ClockDivider { get; private set; }

        /// <summary>
        /// DAC Reference voltage option
        /// </summary>
        public DacRefVoltage DacRefVoltage { get; private set; }

        /// <summary>
        /// DAC reference option
        /// </summary>
        public DacRefOption DacRefOption { get; private set; }

        /// <summary>
        /// Initial DAC Output Value
        /// </summary>
        public byte DacOutput { get; private set; }

        /// <summary>
        /// If set, the interrupt detection flag will be set when a
        /// negative edge occurs.
        /// </summary>
        public bool InterruptNegativeEdge { get; private set; }

        /// <summary>
        /// If set, the interrupt detection flag will be set when a
        /// positive edge occurs.
        /// </summary>
        public bool InterruptPositiveEdge { get; private set; }

        /// <summary>
        /// ADC Reference voltage option
        /// </summary>
        public AdcRefVoltage AdcRefVoltage { get; private set; }

        /// <summary>
        /// ADC reference option
        /// </summary>
        public AdcRefOption AdcRefOption { get; private set; }

        /// <summary>
        /// The Usb vendor id
        /// </summary>
        public ushort Vid { get; private set; }

        /// <summary>
        /// The Usb product id
        /// </summary>
        public ushort Pid { get; private set; }

        /// <summary>
        /// USB Self-Powered Attribute
        /// </summary>
        public UsbSelfPowered SelfPowered { get; private set; }

        /// <summary>
        /// USB Remote Wake-Up Capability
        /// </summary>
        public UsbRemoteWake RemoteWake { get; private set; }

        /// <summary>
        /// The requested mA value during the USB enumeration
        /// </summary>
        public int PowerRequestMa { get; private set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"CdcSerialNumberEnable: {CdcSerialNumberEnable}\r\n");
            stringBuilder.Append($"ChipSecurity: {ChipSecurity}\r\n");
            stringBuilder.Append($"ClockDivider: {ClockDivider}\r\n");
            stringBuilder.Append($"DacRefVoltage: {DacRefVoltage}\r\n");
            stringBuilder.Append($"DacRefOption: {DacRefOption}\r\n");
            stringBuilder.Append($"DacOutput: 0x{DacOutput:X}\r\n");
            stringBuilder.Append($"InterruptNegativeEdge: {InterruptNegativeEdge}\r\n");
            stringBuilder.Append($"InterruptPositiveEdge: {InterruptPositiveEdge}\r\n");
            stringBuilder.Append($"AdcRefVoltage: {AdcRefVoltage}\r\n");
            stringBuilder.Append($"AdcRefOption: {AdcRefOption}\r\n");
            stringBuilder.Append($"Vid: 0x{Vid:X}\r\n");
            stringBuilder.Append($"Pid: 0x{Pid:X}\r\n");
            stringBuilder.Append($"SelfPowered: {SelfPowered}\r\n");
            stringBuilder.Append($"RemoteWake: {RemoteWake}\r\n");
            stringBuilder.Append($"PowerRequestMa: 0x{PowerRequestMa:X}\r\n");

            return stringBuilder.ToString();
        }

        internal void Deserialise(Stream stream)
        {
            stream.ReadByte();

            int temp = stream.ReadByte();

            CdcSerialNumberEnable = (temp & 0x80) == 0x80;
            ChipSecurity = (ChipSecurity)(temp & 0b11);
            ClockDivider = (ClockOutDivider) (stream.ReadByte() & 0x0F);

            temp = stream.ReadByte();

            DacRefVoltage = (DacRefVoltage)((temp & 0xC0) >> 6);
            DacRefOption = (DacRefOption)((temp & 0x10) >> 4);
            DacOutput = (byte)(temp & 0x0F);

            temp = stream.ReadByte();

            InterruptNegativeEdge = (temp & 0x40) == 0x40;
            InterruptPositiveEdge = (temp & 0x20) == 0x20;
            AdcRefVoltage = (AdcRefVoltage)((temp & 0x18) >> 3);
            AdcRefOption = (AdcRefOption)((temp & 0x4) >> 2);

            Vid = (byte)stream.ReadByte();
            Vid += (ushort)(stream.ReadByte() << 8);

            Pid = (byte)stream.ReadByte();
            Pid += (ushort)(stream.ReadByte() << 8);

            temp = stream.ReadByte();

            SelfPowered = (UsbSelfPowered)((temp & 0x40) >> 6);
            RemoteWake = (UsbRemoteWake)((temp & 0x20) >> 5);
            PowerRequestMa = stream.ReadByte() * 2;
        }
    }
}