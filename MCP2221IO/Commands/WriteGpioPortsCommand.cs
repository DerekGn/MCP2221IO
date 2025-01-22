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
using System;
using System.IO;

namespace MCP2221IO.Commands
{
    /// <summary>
    /// Write Gpio Ports command
    /// </summary>
    internal class WriteGpioPortsCommand : BaseCommand
    {
        public WriteGpioPortsCommand(
            GpioPort gpioPort0,
            GpioPort gpioPort1,
            GpioPort gpioPort2,
            GpioPort gpioPort3)
            : base(CommandCodes.SetGpioValues)
        {
            GpioPort0 = gpioPort0 ?? throw new ArgumentNullException(nameof(gpioPort0));
            GpioPort1 = gpioPort1 ?? throw new ArgumentNullException(nameof(gpioPort1));
            GpioPort2 = gpioPort2 ?? throw new ArgumentNullException(nameof(gpioPort2));
            GpioPort3 = gpioPort3 ?? throw new ArgumentNullException(nameof(gpioPort3));
        }

        public GpioPort GpioPort0 { get; }
        public GpioPort GpioPort1 { get; }
        public GpioPort GpioPort2 { get; }
        public GpioPort GpioPort3 { get; }

        public override void Serialize(Stream stream)
        {
            base.Serialize(stream);

            WriteDnc(stream);

            GpioPort0.Serialize(stream);
            GpioPort1.Serialize(stream);
            GpioPort2.Serialize(stream);
            GpioPort3.Serialize(stream);
        }
    }
}
