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
        /// Get the device <see cref="DeviceStatus"/>
        /// </summary>
        DeviceStatus Status { get; }

        /// <summary>
        /// Get the device <see cref="ChipSettings"/>
        /// </summary>
        ChipSettings ChipSettings { get; }

        /// <summary>
        /// The Flash <see cref="GpSettings"/>
        /// </summary>
        GpSettings GpSettings { get; }

        /// <summary>
        /// The <see cref="Device"/> <see cref="SramSettings"/>
        /// </summary>
        SramSettings SramSettings { get; }

        /// <summary>
        /// Gpio 0 port settings
        /// </summary>
        GpioPort GpioPort0 { get; }

        /// <summary>
        /// Gpio 1 port settings
        /// </summary>
        GpioPort GpioPort1 { get; }

        /// <summary>
        /// Gpio 2 port settings
        /// </summary>
        GpioPort GpioPort2 { get; }

        /// <summary>
        /// Gpio 3 port settings
        /// </summary>
        GpioPort GpioPort3 { get; }

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
        /// Cancel the active I2C bus transfer
        /// </summary>
        void CancelI2CBusTransfer();

        /// <summary>
        /// Update the I2C bus speed
        /// </summary>
        void UpdateI2CBusSpeed(int speed);

        /// <summary>
        /// Read the <see cref="DeviceStatus"/> from the device
        /// </summary>
        void ReadDeviceStatus();

        /// <summary>
        /// Read the <see cref="ChipSettings"/> from the device
        /// </summary>
        void ReadChipSettings();

        /// <summary>
        /// Read the <see cref="GpSettings"/> from the device
        /// </summary>
        void ReadGpSettings();

        /// <summary>
        /// Read the <see cref="SramSettings"/> from the device
        /// </summary>
        void ReadSramSettings();

        /// <summary>
        /// Read the Gpio ports
        /// </summary>
        void ReadGpioPorts();

        /// <summary>
        /// Write the <see cref="ChipSettings"/> to the device
        /// </summary>
        /// <param name="password">The password to update in flash</param>
        void WriteChipSettings(Password password);

        /// <summary>
        /// Write the GP power up settings to flash
        /// </summary>
        void WriteGpSettings();

        /// <summary>
        /// Write the <see cref="SramSettings"/> to the device
        /// </summary>
        /// <param name="clearInterrupts">Clear any pending interrupts</param>
        void WriteSramSettings(bool clearInterrupts);

        /// <summary>
        /// Write the Gpio ports
        /// </summary>
        void WriteGpioPorts();

        /// <summary>
        /// Open the device
        /// </summary>
        void Open();

        /// <summary>
        /// Close the device
        /// </summary>
        void Close();
    }
}
