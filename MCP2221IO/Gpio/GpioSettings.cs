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
    /// The devices Gpio settings
    /// </summary>
    public abstract class GpioSettings
    {
        public bool OutputValue { get; set; }

        public GpioDirection Direction { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"\tOutputValue:\t{OutputValue}");
            stringBuilder.AppendLine($"\tDirection:\t\t{Direction}");

            return stringBuilder.ToString();
        }

        public virtual void Deserialise(Stream stream)
        {
            int temp = stream.ReadByte();

            OutputValue = (temp & 0x10) == 0x10;
            Direction = (GpioDirection)((temp & 0x80) >> 3);
        }
    }
}