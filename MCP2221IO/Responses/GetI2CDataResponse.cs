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

using MCP2221IO.Exceptions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MCP2221IO.Responses
{
    /// <summary>
    /// Get I2C data from the device
    /// </summary>
    internal class GetI2CDataResponse : BaseResponse
    {
        public GetI2CDataResponse() : base(Commands.CommandCodes.GetI2CData)
        {
        }

        public IList<byte> Data { get; private set; }

        public override void Deserialize(Stream stream)
        {
            base.Deserialize(stream);

            int temp = stream.ReadByte();

            if (temp == 0x7F)
            {
                throw new CommandExecutionFailedException("I2C data read failed");
            }
            else
            {
                byte[] buffer = new byte[temp];

                stream.Read(buffer);

                Data = buffer.ToList();
            }
        }
    }
}
