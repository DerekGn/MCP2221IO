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
using System.Linq;

namespace MCP2221IO.Commands
{
    /// <summary>
    /// Write I2C data to a device
    /// </summary>
    internal class I2cWriteDataCommand : BaseCommand
    {
        public I2cWriteDataCommand(CommandCodes commandCode, I2cAddress address, IList<byte> data) : base(commandCode)
        {
            Address = address;
            Data = data;
        }

        /// <summary>
        /// The I2C device address
        /// </summary>
        public I2cAddress Address { get; }
        /// <summary>
        /// The data to write to the device
        /// </summary>
        public IList<byte> Data { get; }

        public override void Serialize(Stream stream)
        {
            base.Serialize(stream);
            stream.WriteUShort((ushort)Data.Count);
            stream.Write(Address.WriteAddress.ToArray());
            stream.Write(Data.ToArray(), 0, Data.Count);
        }
    }
}
