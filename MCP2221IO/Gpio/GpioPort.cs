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
using System.IO;
using System.Text;

namespace MCP2221IO.Gpio
{
    /// <summary>
    /// A Gpio port
    /// </summary>
    public class GpioPort
    {
        private bool? _value;

        /// <summary>
        /// Indicates if the port is configured for GPIO operation
        /// </summary>
        public bool Enabled { get; private set; }

        /// <summary>
        /// The GPIO port direction
        /// </summary>
        public bool? IsInput { get; set; }

        /// <summary>
        /// The GPIO port value
        /// </summary>
        public bool? Value
        {
            get => _value;
            set
            {
                if (Enabled)
                {
                    _value = value;
                }
                else
                {
                    throw new GpioNotEnabledException();
                }
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(
                $"{nameof(Enabled)}: {Enabled,5} " +
                $"{nameof(IsInput)}: {IsInput,5} " +
                $"{nameof(Value)}: {Value,5} ");

            return stringBuilder.ToString();
        }

        internal static void Write(Stream stream, bool? value)
        {
            if (value.HasValue)
            {
                stream.WriteByte(0xFF);
                stream.WriteByte((byte)(value.Value ? 0x01 : 0x00));
            }
        }

        internal void Deserialize(Stream stream)
        {
            int value = stream.ReadByte();

            Enabled = value != 0xEE;

            if (value != 0xEE)
            {
                Value = value == 0x01;
            }

            value = stream.ReadByte();

            if (value != 0xEF)
            {
                IsInput = value == 0x01;
            }
        }

        internal void Serialize(Stream stream)
        {
            if (Enabled)
            {
                Write(stream, Value);
                Write(stream, IsInput);
            }
            else
            {
                stream.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            }
        }
    }
}