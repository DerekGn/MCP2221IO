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
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MCP2221IO
{
    public class DeviceStatus
    {
        /// <summary>
        /// The I2C Transfer state
        /// </summary>
        public I2cCancelTransferState CancelTransferState { get; private set; }

        /// <summary>
        /// The I2C bus speed
        /// </summary>
        public I2cSpeedStatus SpeedStatus { get; private set; }

        /// <summary>
        /// The divider value given the command
        /// </summary>
        public byte I2cLastDivisor { get; private set; }

        /// <summary>
        /// Internal I2C state machine state
        /// </summary>
        public byte I2cStateMachineState { get; private set; }

        /// <summary>
        /// The requested I2C transfer length
        /// </summary>
        public ushort I2cTransferLength { get; private set; }

        /// <summary>
        /// The already transferred (through I2C) number of bytes
        /// </summary>
        public ushort I2cTransferredLength { get; private set; }

        /// <summary>
        /// Internal I2C data buffer counter
        /// </summary>
        public byte I2cBufferCounter { get; private set; }

        /// <summary>
        /// Current I2C communication speed divider value
        /// </summary>
        public byte I2cClockDivisor { get; private set; }

        /// <summary>
        /// Current I2C communication speed
        /// </summary>
        public int I2cSpeed { get; private set; }

        /// <summary>
        /// Current I2C timeout value
        /// </summary>
        public byte I2cTimeout { get; private set; }

        /// <summary>
        /// The I2C address being used
        /// </summary>
        public ushort I2cAddress { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool AckStatus { get; private set; }

        /// <summary>
        /// SCL line value – as read from the pin
        /// </summary>
        public bool SclLineState { get; private set; }

        /// <summary>
        /// SDA line value – as read from the pin
        /// </summary>
        public bool SdaLineState { get; private set; }

        /// <summary>
        /// Interrupt edge detector state
        /// </summary>
        public bool EdgeDetectionState { get; private set; }

        /// <summary>
        /// I2C Read pending value
        /// </summary>
        public byte I2cReadPending { get; private set; }

        /// <summary>
        /// The Hardware revision
        /// </summary>
        public string HardwareRevision { get; private set; }

        /// <summary>
        /// The Firmware revision
        /// </summary>
        public string FirmwareRevision { get; private set; }

        /// <summary>
        /// ADC values
        /// </summary>
        public IReadOnlyList<ushort> Adc { get; private set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{nameof(CancelTransferState)}: {CancelTransferState}");
            stringBuilder.AppendLine($"{nameof(SpeedStatus)}: {SpeedStatus}");
            stringBuilder.AppendLine($"{nameof(I2cLastDivisor)}: 0x{I2cLastDivisor:X}");
            stringBuilder.AppendLine($"{nameof(I2cStateMachineState)}: 0x{I2cStateMachineState:X}");
            stringBuilder.AppendLine($"{nameof(I2cTransferLength)}: 0x{I2cTransferLength:X}");
            stringBuilder.AppendLine($"{nameof(I2cTransferredLength)}: 0x{I2cTransferredLength:X}");
            stringBuilder.AppendLine($"{nameof(I2cBufferCounter)}: 0x{I2cBufferCounter:X}");
            stringBuilder.AppendLine($"{nameof(I2cClockDivisor)}: 0x{I2cClockDivisor:X}");
            stringBuilder.AppendLine($"{nameof(I2cSpeed)}: 0x{I2cSpeed:X}");
            stringBuilder.AppendLine($"{nameof(I2cTimeout)}: 0x{I2cTimeout:X}");
            stringBuilder.AppendLine($"{nameof(I2cAddress)}: 0x{I2cAddress:X}");
            stringBuilder.AppendLine($"{nameof(AckStatus)}: {AckStatus}");
            stringBuilder.AppendLine($"{nameof(SclLineState)}: {SclLineState}");
            stringBuilder.AppendLine($"{nameof(SdaLineState)}: {SdaLineState}");
            stringBuilder.AppendLine($"{nameof(EdgeDetectionState)}: {EdgeDetectionState}");
            stringBuilder.AppendLine($"{nameof(I2cReadPending)}: 0x{I2cReadPending:X}");
            stringBuilder.AppendLine($"{nameof(HardwareRevision)}: {HardwareRevision}");
            stringBuilder.AppendLine($"{nameof(FirmwareRevision)}: {FirmwareRevision}");

            stringBuilder.AppendLine($"{nameof(Adc)}:\r\n[0x{Adc[0]:X4}]\r\n[0x{Adc[1]:X4}]\r\n[0x{Adc[2]:X4}]");

            return stringBuilder.ToString();
        }

        internal void Deserialize(Stream stream)
        {
            CancelTransferState = (I2cCancelTransferState)stream.ReadByte();
            SpeedStatus = (I2cSpeedStatus)stream.ReadByte();
            I2cLastDivisor = (byte)stream.ReadByte();

            stream.Seek(9, SeekOrigin.Begin);

            I2cStateMachineState = (byte)stream.ReadByte();
            I2cTransferLength = stream.ReadUShort();
            I2cTransferredLength = stream.ReadUShort();
            I2cBufferCounter = (byte)stream.ReadByte();
            I2cClockDivisor = (byte)stream.ReadByte();
            I2cTimeout = (byte)stream.ReadByte();
            I2cAddress = stream.ReadUShort();

            stream.Seek(21, SeekOrigin.Begin);

            AckStatus = (stream.ReadByte() & 0x40) == 0x40;
            SclLineState = stream.ReadByte() == 1;
            SdaLineState = stream.ReadByte() == 1;
            EdgeDetectionState = stream.ReadByte() == 1;
            I2cReadPending = (byte)stream.ReadByte();

            stream.Seek(47, SeekOrigin.Begin);

            HardwareRevision = string.Empty;
            HardwareRevision += (char)stream.ReadByte();
            HardwareRevision += '.';
            HardwareRevision += (char)stream.ReadByte();

            FirmwareRevision = string.Empty;
            FirmwareRevision += (char)stream.ReadByte();
            FirmwareRevision += '.';
            FirmwareRevision += (char)stream.ReadByte();

            List<ushort> adc = new List<ushort>();

            for (int i = 0; i < 3; i++)
            {
                adc.Add(stream.ReadUShort());
            }

            Adc = adc.AsReadOnly();

            I2cSpeed = 12000000 / (I2cClockDivisor + 2);
        }
    }
}