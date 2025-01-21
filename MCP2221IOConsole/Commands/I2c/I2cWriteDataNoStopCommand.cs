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

using McMaster.Extensions.CommandLineUtils;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MCP2221IOConsole.Commands.I2c
{
    [Command(Name = "write-data-no-stop", Description = "Write Data to a device on the I2C bus with NO-STOP")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "<Pending>")]
    internal class I2cWriteDataNoStopCommand : BaseI2cWriteCommand
    {
        public I2cWriteDataNoStopCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override int OnExecute(CommandLineApplication application, IConsole console)
        {
            return ExecuteCommand((device) =>
            {
                var address = ParseAddress();

                console.WriteLine("Writing [{Count}] bytes to device [{Address}] with NO-STOP", Data!.Count, address);

                device.I2cWriteDataNoStop(address, Data);

                console.WriteLine("Wrote [{Count}] bytes to device", Data.Count);

                return 0;
            });
        }
    }
}