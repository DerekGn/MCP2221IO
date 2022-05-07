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

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace MCP2221IO.Commands
{
    [SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "<Pending>")]
    internal class SetI2cBusSpeedCommand : BaseCommand
    {
        public SetI2cBusSpeedCommand(int speed) : base(CommandCodes.StatusSetParameters)
        {
            if (speed > IDevice.I2cMaxSpeed)
            {
                throw new ArgumentOutOfRangeException(nameof(speed), speed, $"Must be less than {IDevice.I2cMaxSpeed}");
            }

            Speed = speed;
        }

        public int Speed { get; }

        public override void Serialize(Stream stream)
        {
            base.Serialize(stream);

            stream.WriteByte(0);    // Don't care
            stream.WriteByte(0x00); // Cancel current I2C/SMBus transfer(sub - command) no change
            stream.WriteByte(0x20); // Set I2C/SMBus communication speed (sub-command)

            stream.WriteByte((byte)((12000000 / Speed) - 2));
        }
    }
}