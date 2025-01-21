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
using System;
using System.IO;

namespace MCP2221IO.Commands
{
    /// <summary>
    /// A base command
    /// </summary>
    internal abstract class BaseCommand : ICommand
    {
        protected BaseCommand(CommandCodes commandCode)
        {
            CommandCode = commandCode;
        }

        // <inheritdoc/>
        public CommandCodes CommandCode { get; }

        // <inheritdoc/>
        public virtual void Serialize(Stream stream)
        {
            ArgumentNullException.ThrowIfNull(stream);

            if (stream.Length != 64)
            {
                throw new InvalidStreamLengthException($"Unexpected stream length Expected: [0x40] Actual [{stream.Length}]");
            }

            stream.Position = 0;
            stream.WriteByte(0);
            stream.WriteByte((byte)CommandCode);
        }
    }
}
