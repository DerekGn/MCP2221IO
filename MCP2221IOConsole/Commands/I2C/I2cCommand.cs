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
using System.Diagnostics.CodeAnalysis;

namespace MCP2221IOConsole.Commands.I2c
{
    [Command(Description = "Execute I2C functions")]
    [Subcommand(typeof(I2cSetTransferSpeedCommand))]
    [Subcommand(typeof(I2cCancelTransferCommand))]
    [Subcommand(typeof(I2cScanBusCommand))]
    [Subcommand(typeof(I2cReadDataCommand))]
    [Subcommand(typeof(I2cReadDataRepeatStartCommand))]
    [Subcommand(typeof(I2cWriteDataCommand))]
    [Subcommand(typeof(I2cWriteDataRepeatStartCommand))]
    [Subcommand(typeof(I2cWriteDataNoStopCommand))]
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "<Pending>")]
    internal class I2cCommand : BaseCommand
    {
        public I2cCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
