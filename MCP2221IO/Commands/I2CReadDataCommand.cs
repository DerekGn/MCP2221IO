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
using System.Linq;

namespace MCP2221IO.Commands
{
    /// <summary>
    /// Read I2C data command
    /// </summary>
    internal class I2cReadDataCommand : BaseCommand
    {
        public I2cReadDataCommand(CommandCodes commandCode, I2cAddress address, ushort length) : base(commandCode)
        {
            Address = address;
            Length = length;
        }

        /// <summary>
        /// The address of the I2C device to read
        /// </summary>
        public I2cAddress Address { get; }
        /// <summary>
        /// The length of data to read
        /// </summary>
        public ushort Length { get; }

        public override void Serialize(Stream stream)
        {
            base.Serialize(stream);

            stream.WriteUShort(Length);
            stream.Write(Address.ReadAddress.ToArray());
        }
    }
}
