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
using System.Linq;

namespace MCP2221IOConsole.Commands.I2c
{
    [Command(Name="read-data", Description = "Read data from a device on the I2C bus")]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "<Pending>")]
    internal class I2cReadDataCommand : BaseReadI2cDataCommand
    {
        public I2cReadDataCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override int OnExecute(CommandLineApplication app, IConsole console)
        {
            return ExecuteCommand((device) =>
            {
                var deviceAddress = ParseAddress();

                console.WriteLine($"Reading the I2C bus device address [{deviceAddress}]");

                var result = device.I2cReadData(deviceAddress, Length);

                console.WriteLine($"Read [{result.Count}] bytes from I2C device. [{BitConverter.ToString(result.ToArray())}]");

                return 0;
            });
        }
    }
}