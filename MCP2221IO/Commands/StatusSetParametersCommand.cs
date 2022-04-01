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

using System.IO;

namespace MCP2221IO.Commands
{
    /// <summary>
    /// This command options of the device. It is used to poll 
    /// for the status of the device. It is also used to 
    /// establish certain I2C bus parameters/conditions.
    /// </summary>
    internal class StatusSetParametersCommand : BaseCommand
    {
        private const int I2C_CANCEL_XFER = 0x10;
        private const int I2C_SET_SPEED = 0x20;

        private readonly byte _cancelCommand;
        private readonly byte _setSpeed;
        private readonly byte _divisor;

        public StatusSetParametersCommand(bool cancelTransfer = false, byte divisor = 0) 
            : base(CommandCodes.StatusSetParameters)
        {
            if (cancelTransfer)
            {
                _cancelCommand = I2C_CANCEL_XFER;
            }

            if (divisor > 0)
            {
                _setSpeed = I2C_SET_SPEED;
                _divisor = divisor;
            }
        }

        // <inheritdoc/>
        public override void Serialise(Stream stream)
        {
            base.Serialise(stream);
            stream.WriteByte(0);
            stream.WriteByte(_cancelCommand);
            stream.WriteByte(_setSpeed);
            stream.WriteByte(_divisor);
        }
    }
}
