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
    internal class WriteGpSettingsCommand : WriteFlashDataCommand
    {
        public WriteGpSettingsCommand(GpSettings gpSettings) : base(WriteFlashSubCode.WriteGpSettings)
        {
            GpSettings = gpSettings ?? throw new ArgumentNullException(nameof(gpSettings));
        }

        public GpSettings GpSettings { get; }

        public override void Serialise(Stream stream)
        {
            base.Serialise(stream);

            WritePort(stream, GpSettings.Gp0PowerUpSetting);
            WritePort(stream, GpSettings.Gp1PowerUpSetting);
            WritePort(stream, GpSettings.Gp2PowerUpSetting);
            WritePort(stream, GpSettings.Gp3PowerUpSetting);
        }

        private void WritePort<T>(Stream stream, GpSetting<T> port) where T : System.Enum
        {
            int update = (port.OutputValue ? 0x40 : 0) << 4;
            update |= port.IsInput ? 0x8 : 0x00;
            update |= (int)(object)port.Designation & 0b111;

            stream.WriteByte((byte)update);
        }
    }
}
