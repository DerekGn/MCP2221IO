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
using MCP2221IO;
using System;
using System.ComponentModel.DataAnnotations;

namespace MCP2221IOConsole.Commands.SmBus
{
    [Command(Name = "read-byte", Description = "Execute SmBus byte read")]
    internal class SmBusReadCommand : BaseSmBusCommand
    {
        public SmBusReadCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [Required]
        [Option(Templates.Pec, "Use SmBus pec", CommandOptionType.SingleValue)]
        public bool Pec { get; set; }

        protected override int OnExecute(CommandLineApplication app, IConsole console)
        {
            return ExecuteCommand((device) =>
            {
                var deviceAddress = new I2cAddress(Address, I2cAddressSize.SevenBit);

                console.WriteLine($"Reading a byte from the SmBus device address [{deviceAddress}]");

                var result = device.SmBusReadByte(deviceAddress, Pec);

                console.WriteLine($"Read [0X{result:X}] from SmBus device");

                return 0;
            });
        }
    }
}