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
    internal class VersionInfo
    {
        public VersionInfo(IList<byte> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if(data.Count < 6)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Must contain 6 bytes");
            }

            HardwareId = data[0];
            HardwareVersion = data[1];
            FirmwareBootVersion = new VersionNumber(data.Skip(2).Take(2).ToList());
            FirmwareAppVersion = new VersionNumber(data.Skip(4).Take(2).ToList());
        }

        public byte HardwareId { get; private set; }

        public byte HardwareVersion { get; private set; }

        public VersionNumber FirmwareBootVersion { get; private set; }

        public VersionNumber FirmwareAppVersion { get; private set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{nameof(HardwareId)}: 0x{HardwareId:X2}");
            stringBuilder.AppendLine($"{nameof(HardwareVersion)}: 0x{HardwareVersion:X2}");
            stringBuilder.AppendLine($"{nameof(FirmwareBootVersion)}: {FirmwareBootVersion}");
            stringBuilder.AppendLine($"{nameof(FirmwareAppVersion)}: {FirmwareAppVersion}");

            return stringBuilder.ToString();
        }
    }
}
