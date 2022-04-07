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

namespace MCP2221IO.Commands
{
    /// <summary>
    /// Write flash data string
    /// </summary>
    internal class WriteFlashDataStringCommand : WriteFlashDataCommand
    {
        /// <summary>
        /// Create a new instance of a <see cref="WriteFlashDataStringCommand"/>
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <param name="subCode">The <see cref="WriteFlashSubCode"/></param>
        public WriteFlashDataStringCommand(string value, WriteFlashSubCode subCode) : base(subCode)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new System.ArgumentException($"'{nameof(value)}' cannot be null or whitespace.", nameof(value));
            }

            Value = value;
        }

        /// <summary>
        /// The value to write
        /// </summary>
        public string Value { get; }

        // <inheritdoc/>
        public override void Serialize(Stream stream)
        {
            base.Serialize(stream);

            stream.WriteByte((byte)((Value.Length * 2) + 2));

            var bytes = Encoding.Unicode.GetBytes(Value);

            stream.Write(bytes, 0, bytes.Length);
        }
    }
}