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

namespace MCP2221IO.Gpio
{
    /// <summary>
    /// A Gpio port
    /// </summary>
    public class GpioPort
    {
        /// <summary>
        /// Indicates if the port is configured for GPIO operation
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// The GPIO port value
        /// </summary>
        public bool Value { get; set; }

        /// <summary>
        /// The GPIO port direction
        /// </summary>
        public bool IsInput { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(
                $"{nameof(Enabled)}: {Enabled,5} " +
                $"{nameof(IsInput)}: {IsInput,5} " +
                $"{nameof(Value)}: {Value,5} ");

            return stringBuilder.ToString();
        }

        internal void Deserialize(Stream stream)
        {
            Enabled = stream.ReadByte() != 0xEE;
            IsInput = stream.ReadByte() == 0x00;
            Value = stream.ReadByte() == 0x01;
        }

        internal void Serialize(Stream stream)
        {
            if (!Enabled)
            {
                stream.Write(new byte[] { 0, 0, 0, 0 });
            }
            else
            {
                stream.WriteByte(0xFF);
                stream.WriteByte((byte)(Value ? 1 : 0));

                stream.WriteByte(0xFF);
                stream.WriteByte((byte)(IsInput ? 1 : 0));
            }
        }
    }
}
