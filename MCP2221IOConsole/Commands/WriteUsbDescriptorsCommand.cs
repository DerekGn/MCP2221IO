﻿/*
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

namespace MCP2221IOConsole.Commands
{
    [Command("write-usb", Description = "Write Device Usb Descriptors")]
    internal class WriteUsbDescriptorsCommand : BaseCommand
    {
        public WriteUsbDescriptorsCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [Option("-um", "The USB Manufacturer String Descriptor", CommandOptionType.SingleValue)]
        public (bool hasValue, string value) Manufacturer { get; set; }

        [Option("-up","The USB Product String Descriptor", CommandOptionType.SingleValue)]
        public (bool hasValue, string value) Product { get; set; }

        [Option("-us", "The USB Serial Number String Descriptor", CommandOptionType.SingleValue)]
        public (bool hasValue, string value) SerialNumber { get; set; }

        protected override int OnExecute(CommandLineApplication app, IConsole console)
        {
            return ExecuteCommand((device) =>
            {
                if(Manufacturer.hasValue)
                {
                    device.UsbManufacturerDescriptor = Manufacturer.value;
                }

                if (Product.hasValue)
                {
                    device.UsbProductDescriptor = Manufacturer.value;
                }

                if (SerialNumber.hasValue)
                {
                    device.UsbSerialNumberDescriptor = SerialNumber.value;
                }

                if(!(Manufacturer.hasValue || Product.hasValue || SerialNumber.hasValue))
                {
                    console.Error.WriteLine("No values specified for update");
                }

                return Manufacturer.hasValue || Product.hasValue || SerialNumber.hasValue ? 0 : -1;
            });
        }
    }
}

