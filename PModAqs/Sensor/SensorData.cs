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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PModAqs.Sensor
{
    internal class SensorData
    {
        public SensorData(IList<byte> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Count < 8)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Must contain 6 bytes");
            }

            Co2 = (ushort)(data[0] << 8);
            Co2 += data[1];

            TVOC = (ushort)(data[2] << 8);
            TVOC += data[3];

            Status = (Status)data[4];
            Error = (Error)data[5];

            RawData = new RawData(data.Skip(6).Take(2).ToList());
        }

        public ushort Co2 { get; }

        public ushort TVOC { get; }

        public Status Status { get; }

        public Error Error { get; }

        public RawData RawData { get; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{nameof(Co2)}:\t\t[0x{Co2:X4}] {Co2}");
            stringBuilder.AppendLine($"{nameof(TVOC)}:\t\t[0x{TVOC:X4}] {TVOC}");
            stringBuilder.AppendLine($"{nameof(Status)}:\t\t[0x{(byte)Status:X4}] {Status}");
            stringBuilder.AppendLine($"{nameof(Error)}:\t\t[0x{(byte)Error:X4}] {Error}");
            stringBuilder.AppendLine($"{nameof(RawData)} =>");
            stringBuilder.Append($"{RawData}");

            return stringBuilder.ToString();
        }
    }
}
