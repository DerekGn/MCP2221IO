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

namespace MCP2221IO
{
    /// <summary>
    /// The I2C transfer cancel state
    /// </summary>
    public enum I2cCancelTransferState
    {
        /// <summary>
        /// No special operation (i.e., Cancel current I2C/SMBus transfer)
        /// </summary>
        NoSpecialOperation = 0x00,
        /// <summary>
        /// The current I2C/SMBus transfer was marked for
        /// cancellation. The actual I2C/SMBus transfer cancellation 
        /// and bus release will need some time (a few hundred
        /// microseconds, depending on the communication speed
        /// initially chosen for the canceled transfer)
        /// </summary>
        MarkedForCancellation = 0x10,
        /// <summary>
        /// The I2C engine (inside MCP2221) was already in Idle mode. 
        /// The cancellation command had no effect.
        /// </summary>
        EngineIdle = 0x11
    }
}