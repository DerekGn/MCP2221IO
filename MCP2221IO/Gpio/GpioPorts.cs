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
    /// The devices Gpio ports
    /// </summary>
    public class GpioPorts
    {
        /// <summary>
        /// Gpio zero settings
        /// </summary>
        public Gpio0Settings Gpio0Settings { get; private set; }
        /// <summary>
        /// Gpio one settings
        /// </summary>
        public Gpio1Settings Gpio1Settings { get; private set; }

        /// <summary>
        /// Gpio two settings
        /// </summary>
        public Gpio2Settings Gpio2Settings { get; private set; }
        /// <summary>
        /// Gpio three settings
        /// </summary>
        public Gpio3Settings Gpio3Settings { get; private set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{nameof(Gpio0Settings)}:\r\n{Gpio0Settings}");
            stringBuilder.AppendLine($"{nameof(Gpio0Settings)}:\r\n{Gpio1Settings}");
            stringBuilder.AppendLine($"{nameof(Gpio0Settings)}:\r\n{Gpio2Settings}");
            stringBuilder.AppendLine($"{nameof(Gpio0Settings)}:\r\n{Gpio3Settings}");

            return stringBuilder.ToString();
        }

        internal void Deserialise(Stream stream)
        {
            Gpio0Settings = new Gpio0Settings();
            Gpio0Settings.Deserialise(stream);

            Gpio1Settings = new Gpio1Settings();
            Gpio1Settings.Deserialise(stream);

            Gpio2Settings = new Gpio2Settings();
            Gpio2Settings.Deserialise(stream);
            
            Gpio3Settings = new Gpio3Settings();
            Gpio3Settings.Deserialise(stream);
        }
    }
}