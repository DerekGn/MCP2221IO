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

using MCP2221IO.Gpio;
using MCP2221IO.Settings;
using System;
using System.Collections.Generic;

namespace MCP2221IO
{
    public interface IDevice : IDisposable
    {
        const int I2CMaxSpeed = 500000;
        const int I2CMinSpeed = 46875;

        /// <summary>
        /// Get the device <see cref="ChipSettings"/>
        /// </summary>
        ChipSettings ChipSettings { get; }

        /// <summary>
        /// The <see cref="Device"/> <see cref="SramSettings"/>
        /// </summary>
        SramSettings SramSettings { get; }

        /// <summary>
        /// Get the device <see cref="DeviceStatus"/>
        /// </summary>
        DeviceStatus Status { get; }

        /// <summary>
        /// Get the Gpio settings
        /// </summary>
        GpioPorts GpioPorts { get; }

        /// <summary>
        /// The USB manufacturer descriptor value
        /// </summary>
        string UsbManufacturerDescriptor { get; set; }

        /// <summary>
        /// The USB product descriptor value
        /// </summary>
        string UsbProductDescriptor { get; set; }

        /// <summary>
        /// The USB serial number value
        /// </summary>
        string UsbSerialNumberDescriptor { get; set; }

        /// <summary>
        /// The factory serial number as a hex string
        /// </summary>
        string FactorySerialNumber { get; }

        /// <summary>
        /// Unlock the flash
        /// </summary>
        /// <param name="password">The 8 byte password</param>
        void UnlockFlash(ulong password);

        /// <summary>
        /// Write data to an I2C client device 
        /// </summary>
        /// <param name="address">The address of the client device</param>
        /// <param name="data">The data to write to the device</param>
        void I2CWriteData(byte address, IList<byte> data);

        /// <summary>
        /// Write data to an I2C client device with repeated start
        /// </summary>
        /// <param name="address">The address of the client device</param>
        /// <param name="data">The data to write to the device</param>
        void I2CWriteDataRepeatStart(byte address, IList<byte> data);

        /// <summary>
        /// Write data to an I2C client device with no stop
        /// </summary>
        /// <param name="address">The address of the client device</param>
        /// <param name="data">The data to write to the device</param>
        void I2CWriteDataNoStop(byte address, IList<byte> data);

        /// <summary>
        /// Read data from an I2C client device
        /// </summary>
        /// <param name="address">The address of the client device</param>
        /// <param name="length">The number of bytes to read</param>
        /// <returns>The bytes read from the client device</returns>
        IList<byte> I2CReadData(byte address, ushort length);

        /// <summary>
        /// Read data from an I2C client device with repeated start
        /// </summary>
        /// <param name="address">The address of the client device</param>
        /// <param name="length">The number of bytes to read</param>
        /// <returns>The bytes read from the client device</returns>
        IList<byte> I2CReadDataRepeatedStart(byte address, ushort length);

        /// <summary>
        /// Reset the device
        /// </summary>
        void Reset();

        /// <summary>
        /// Read the <see cref="ChipSettings"/> from the device
        /// </summary>
        void ReadChipSettings();

        /// <summary>
        /// Write the <see cref="ChipSettings"/> to the device
        /// </summary>
        void WriteChipSettings();

        /// <summary>
        /// Read the <see cref="SramSettings"/> from the device
        /// </summary>
        void ReadSramSettings();

        /// <summary>
        /// Write the <see cref="SramSettings"/> to the device
        /// </summary>
        void WriteSramSettings();
    }
}
