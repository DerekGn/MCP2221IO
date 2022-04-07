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

using MCP2221IO.Commands;
using MCP2221IO.Gpio;
using System.IO;

namespace MCP2221IO.Responses
{
    internal class ReadGpioPortsResponse : BaseResponse
    {
        public ReadGpioPortsResponse() : base(CommandCodes.GetGpioValues)
        {
        }

        public GpioPort GpioPort0 { get; private set; }

        public GpioPort GpioPort1 { get; private set; }

        public GpioPort GpioPort2 { get; private set; }

        public GpioPort GpioPort3 { get; private set; }

        public override void Deserialise(Stream stream)
        {
            base.Deserialise(stream);

            GpioPort0 = new GpioPort();
            GpioPort1 = new GpioPort();
            GpioPort2 = new GpioPort();
            GpioPort3 = new GpioPort();

            GpioPort0.Deserialise(stream);
            GpioPort1.Deserialise(stream);
            GpioPort2.Deserialise(stream);
            GpioPort3.Deserialise(stream);
        }
    }
}
