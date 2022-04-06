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

using MCP2221IO.Gpio;
using System.IO;
using System.Text;

namespace MCP2221IO.Settings
{
    /// <summary>
    /// A Gpio setting type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GpSetting<T> where T : System.Enum
    {
        /// <summary>
        /// The current output value on the Gpio port
        /// </summary>
        public bool OutputValue { get; set; }

        /// <summary>
        /// The Gpio port direction
        /// </summary>
        public GpioDirection Direction { get; set; }

        /// <summary>
        /// The current Gp settings port designation
        /// </summary>
        public T Designation { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{nameof(OutputValue)}: {OutputValue}");
            stringBuilder.AppendLine($"{nameof(Direction)}: {Direction}");
            stringBuilder.AppendLine($"{nameof(Designation)}: {Designation}");

            return stringBuilder.ToString();
        }

        // <inheritdoc/>
        public virtual void Deserialise(Stream stream)
        {
            int temp = stream.ReadByte();

            OutputValue = (temp & 0x10) == 0x10;
            Direction = (GpioDirection)((temp & 0x80) >> 3);
            Designation = (T)(object)(temp & 0x07);
        }
    }
}

