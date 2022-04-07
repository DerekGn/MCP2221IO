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

namespace MCP2221IO.Commands
{
    /// <summary>
    /// HID command codes
    /// </summary>
    public enum CommandCodes
    {
        StatusSetParameters = 0x10,
        /// <summary>
        /// Get I2C Data
        /// </summary>
        GetI2CData = 0x40,
        /// <summary>
        /// Set the GPIO port values
        /// </summary>
        SetGpioValues = 0x50,
        /// <summary>
        /// Get the GPIO port values
        /// </summary>
        GetGpioValues = 0x51,
        /// <summary>
        /// Read flash data
        /// </summary>
        ReadFlashData = 0xB0,
        /// <summary>
        /// Write flash data
        /// </summary>
        WriteFlashData = 0xB1,
        /// <summary>
        /// Send the flash unlock code
        /// </summary>
        SendFlashAccessPassword = 0xB2,
        /// <summary>
        /// Reset the device
        /// </summary>
        Reset = 0x70,
        /// <summary>
        /// Get SRAM data
        /// </summary>
        SetSram = 0x60,
        /// <summary>
        /// Set SRAM data
        /// </summary>
        GetSram = 0x61,
        /// <summary>
        /// Read I2C data from the device
        /// </summary>
        ReadI2CData = 0x91,
        /// <summary>
        /// Write I2CData to the device
        /// </summary>
        WriteI2CData = 0x90,
        /// <summary>
        /// Write I2CData to the device with repeat start
        /// </summary>
        WriteI2CDataRepeatedStart = 0x92,
        /// <summary>
        /// Read I2CData to the device with repeat start
        /// </summary>
        ReadI2CDataRepeatedStart = 0x93,
        /// <summary>
        /// Write I2CData to the device with no stop
        /// </summary>
        WriteI2CDataNoStop = 0x94,
    }
}
