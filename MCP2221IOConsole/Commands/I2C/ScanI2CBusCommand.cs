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

namespace MCP2221IOConsole.Commands.I2c
{
    [Command(Description = "Scan the I2C Bus")]
    internal class ScanI2cBusCommand : BaseCommand
    {
        public ScanI2cBusCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [Option(Templates.TenBitAddressing, "Use Ten bit device addressing", CommandOptionType.SingleValue)]
        public (bool HasValue, bool Value) TenBitAddressing { get; set; }

        protected override int OnExecute(CommandLineApplication app, IConsole console)
        {
            return ExecuteCommand((device) =>
            {
                console.WriteLine($"Scanning the I2C Bus 10 Bit Addressing [{TenBitAddressing.HasValue && TenBitAddressing.Value}]");
                
                var result = device.I2cScanBus(TenBitAddressing.HasValue && TenBitAddressing.Value);

                console.WriteLine($"Found [{result.Count}] I2C Devices");

                foreach (var address in result)
                {
                    console.WriteLine($"Device [0x{address.Value:X4}]");
                }

                return 0;
            });
        }
    }
}