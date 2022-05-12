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
using System.Linq;

namespace MCP2221IOConsole.Commands.SmBus
{
    [Command(Name = "block-read", Description = "Execute SmBus block read command")]
    internal class SmBusBlockReadCommand : BaseSmBusCommandCommand
    {
        public SmBusBlockReadCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [Required]
        [Range(typeof(byte), "0", "0xFF")]
        [Option(Templates.Count, "The SmBus block count", CommandOptionType.SingleValue)]
        public byte Count { get; set; }

        protected override int OnExecute(CommandLineApplication app, IConsole console)
        {
            return ExecuteCommand((device) =>
            {
                var deviceAddress = new I2cAddress(Address, I2cAddressSize.SevenBit);

                console.WriteLine($"Reading a block of data from the SmBus device address [{deviceAddress}]");

                var result = device.SmBusBlockRead(deviceAddress, Command, Count, Pec);

                console.WriteLine($"Read [{result.Count}] bytes from SmBus device. [{BitConverter.ToString(result.ToArray())}]");

                return 0;
            });
        }
    }
}