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

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MCP2221IO
{
    public class DeviceStatus
    {

        public I2CCancelTransferState CancelTransferState { get; private set; }

        public I2CSpeedStatus SpeedStatus { get; private set; }

        /// <summary>
        /// The divider value given the command
        /// </summary>
        public byte I2CLastDivisor { get; private set; }

        /// <summary>
        /// Internal I2C state machine state
        /// </summary>
        public byte I2CStateMachineState { get; private set; }

        /// <summary>
        /// The requested I2C transfer length
        /// </summary>
        public ushort I2CTransferLength { get; private set; }

        /// <summary>
        /// The already transferred (through I2C) number of bytes
        /// </summary>
        public ushort I2CTransferredLength { get; private set; }

        /// <summary>
        /// Internal I2C data buffer counter
        /// </summary>
        public byte I2CBufferCounter { get; private set; }

        /// <summary>
        /// Current I2C communication speed divider value
        /// </summary>
        public byte I2CClockDivisor { get; private set; }

        /// <summary>
        /// Current I2C timeout value
        /// </summary>
        public byte I2CTimeout { get; private set; }

        /// <summary>
        /// The I2C address being used
        /// </summary>
        public ushort I2CAddress { get; private set; }

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
        public byte I2CReadPending { get; private set; }

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

            stringBuilder.AppendLine($"CancelTransferState: {CancelTransferState}");
            stringBuilder.AppendLine($"SpeedStatus: {SpeedStatus}");
            stringBuilder.AppendLine($"I2CLastDivisor: 0x{I2CLastDivisor:X}");
            stringBuilder.AppendLine($"I2CStateMachineState: 0x{I2CStateMachineState:X}");
            stringBuilder.AppendLine($"I2CTransferLength: 0x{I2CTransferLength:X}");
            stringBuilder.AppendLine($"I2CTransferredLength: 0x{I2CTransferredLength:X}");
            stringBuilder.AppendLine($"I2CBufferCounter: 0x{I2CBufferCounter:X}");
            stringBuilder.AppendLine($"I2CClockDivisor: 0x{I2CClockDivisor:X}");
            stringBuilder.AppendLine($"I2CTimeout: 0x{I2CTimeout:X}");
            stringBuilder.AppendLine($"I2CAddress: 0x{I2CAddress:X}");
            stringBuilder.AppendLine($"SclLineState: {SclLineState}");
            stringBuilder.AppendLine($"SdaLineState: {SdaLineState}");
            stringBuilder.AppendLine($"EdgeDetectionState: {EdgeDetectionState}");
            stringBuilder.AppendLine($"I2CReadPending: 0x{I2CReadPending:X}");
            stringBuilder.AppendLine($"HardwareRevision: {HardwareRevision}");
            stringBuilder.AppendLine($"FirmwareRevision: {FirmwareRevision}");

            stringBuilder.AppendLine($"ADC:\t[0x{Adc[0]:X}]\t[0x{Adc[1]:X}]\t[0x{Adc[2]:X}]");

            return stringBuilder.ToString();
        }

        internal void Deserialise(Stream stream)
        {
            CancelTransferState = (I2CCancelTransferState)stream.ReadByte();
            SpeedStatus = (I2CSpeedStatus)stream.ReadByte();
            I2CLastDivisor = (byte)stream.ReadByte();

            stream.Position += 3;

            I2CStateMachineState = (byte)stream.ReadByte();
            I2CTransferLength = (byte)stream.ReadByte();
            I2CTransferLength += (ushort)(stream.ReadByte() << 8);
            I2CTransferredLength = (byte)stream.ReadByte();
            I2CTransferredLength += (ushort)(stream.ReadByte() << 8);
            I2CBufferCounter = (byte)stream.ReadByte();
            I2CClockDivisor = (byte)stream.ReadByte();
            I2CTimeout = (byte)stream.ReadByte();
            I2CAddress = (byte)stream.ReadByte();
            I2CAddress += (ushort)(stream.ReadByte() << 8);

            stream.Position += 4;

            SclLineState = stream.ReadByte() == 1;
            SdaLineState = stream.ReadByte() == 1;
            EdgeDetectionState = stream.ReadByte() == 1;
            I2CReadPending = (byte)stream.ReadByte();

            stream.Position += 19;

            HardwareRevision = string.Empty;
            HardwareRevision += (char)stream.ReadByte();
            HardwareRevision += (char)stream.ReadByte();

            FirmwareRevision = string.Empty;
            FirmwareRevision += (char)stream.ReadByte();
            FirmwareRevision += (char)stream.ReadByte();

            List<ushort> adc = new List<ushort>();

            for (int i = 0; i < 3; i++)
            {
                ushort value = (byte)stream.ReadByte();
                value += (ushort)(stream.ReadByte() << 8);

                adc.Add(value);
            }

            Adc = adc.AsReadOnly();
        }
    }
}