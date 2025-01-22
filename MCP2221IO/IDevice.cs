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
        /// <summary>
        /// The maximum supported I2C speed
        /// </summary>
        const int I2cMaxSpeed = 400000;

        /// <summary>
        /// The minimum supported I2C speed
        /// </summary>
        const int I2cMinSpeed = 46875;

        /// <summary>
        /// The maximum I2C read/write buffer size
        /// </summary>
        const ushort MaxI2cBlockSize = 0xFFFF;

        /// <summary>
        /// The maximum Smbus read/write buffer size
        /// </summary>
        const ushort MaxSmBusBlockSize = 0xFF;

        /// <summary>
        /// Get the device <see cref="DeviceStatus"/>
        /// </summary>
        DeviceStatus? Status { get; }

        /// <summary>
        /// Get the device <see cref="ChipSettings"/>
        /// </summary>
        ChipSettings? ChipSettings { get; }

        /// <summary>
        /// The Flash <see cref="GpSettings"/>
        /// </summary>
        GpSettings? GpSettings { get; }

        /// <summary>
        /// The <see cref="Device"/> <see cref="SramSettings"/>
        /// </summary>
        SramSettings? SramSettings { get; }

        /// <summary>
        /// Gpio 0 port settings
        /// </summary>
        GpioPort? GpioPort0 { get; }

        /// <summary>
        /// Gpio 1 port settings
        /// </summary>
        GpioPort? GpioPort1 { get; }

        /// <summary>
        /// Gpio 2 port settings
        /// </summary>
        GpioPort? GpioPort2 { get; }

        /// <summary>
        /// Gpio 3 port settings
        /// </summary>
        GpioPort? GpioPort3 { get; }

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
        string? FactorySerialNumber { get; }

        /// <summary>
        /// Unlock the flash
        /// </summary>
        /// <param name="password">The 8 byte password</param>
        void UnlockFlash(Password password);

        /// <summary>
        /// Reset the device
        /// </summary>
        void Reset();

        /// <summary>
        /// Cancel the active I2C bus transfer
        /// </summary>
        void CancelI2cBusTransfer();

        /// <summary>
        /// Set the I2C bus speed
        /// </summary>
        void SetI2cBusSpeed(int speed);

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

        /// <summary>
        /// Write data to an I2C client device 
        /// </summary>
        /// <param name="address">The address of the client device</param>
        /// <param name="data">The data to write to the device</param>
        void I2cWriteData(I2cAddress address, IList<byte> data);

        /// <summary>
        /// Write data to an I2C client device with repeated start
        /// </summary>
        /// <param name="address">The address of the client device</param>
        /// <param name="data">The data to write to the device</param>
        void I2cWriteDataRepeatStart(I2cAddress address, IList<byte> data);

        /// <summary>
        /// Write data to an I2C client device with no stop
        /// </summary>
        /// <param name="address">The address of the client device</param>
        /// <param name="data">The data to write to the device</param>
        void I2cWriteDataNoStop(I2cAddress address, IList<byte> data);

        /// <summary>
        /// Read data from an I2C client device
        /// </summary>
        /// <param name="address">The address of the client device</param>
        /// <param name="length">The number of bytes to read</param>
        /// <returns>The bytes read from the client device</returns>
        IList<byte> I2cReadData(I2cAddress address, ushort length);

        /// <summary>
        /// Read data from an I2C client device with repeated start
        /// </summary>
        /// <param name="address">The address of the client device</param>
        /// <param name="length">The number of bytes to read</param>
        /// <returns>The bytes read from the client device</returns>
        IList<byte> I2cReadDataRepeatedStart(I2cAddress address, ushort length);

        /// <summary>
        /// Scan the I2C bus and return a list of <see cref="I2cAddress"/> of devices found
        /// </summary>
        /// <param name="useTenBitAddressing">Perform scanning of bus with 10 bit address</param>
        /// <returns>A <see cref="IList{T}"/> of <see cref="I2cAddress"/> instances</returns>
        IList<I2cAddress> I2cScanBus(bool useTenBitAddressing);

        /// <summary>
        /// Send a quick command on the bus
        /// </summary>
        /// <param name="address">The <see cref="I2cAddress"/></param>
        /// <param name="write">Indicates a read or write operation on the bus</param>
        void SmBusQuickCommand(I2cAddress address, bool write);

        /// <summary>
        /// Read a byte from the bus
        /// </summary>
        /// <param name="address">The <see cref="I2cAddress"/></param>
        /// <param name="errorCheck">Perform packet error check on response</param>
        /// <returns>The byte read from the bus</returns>
        byte SmBusReadByte(I2cAddress address, bool errorCheck = false);

        /// <summary>
        /// Write a byte to the bus
        /// </summary>
        /// <param name="address">The <see cref="I2cAddress"/></param>
        /// <param name="data">The <see cref="byte"/> data to write</param>
        /// <param name="errorCheck">Perform packet error check on response</param>
        void SmBusWriteByte(I2cAddress address, byte data, bool errorCheck = false);

        /// <summary>
        /// Read a byte from the bus with a command code
        /// </summary>
        /// <param name="address">The <see cref="I2cAddress"/></param>
        /// <param name="command">The <see cref="byte"/> command</param>
        /// <param name="pec">Read data with packet error check</param>
        /// <returns>The byte read from the bus</returns>
        byte SmBusReadByteCommand(I2cAddress address, byte command, bool errorCheck = false);

        /// <summary>
        /// Write a byte to the bus with a command code
        /// </summary>
        /// <param name="address">The <see cref="I2cAddress"/></param>
        /// <param name="command">The <see cref="byte"/> command</param>
        /// <param name="data">The <see cref="byte"/> data</param>
        /// <param name="errorCheck">Perform packet error check on response</param>
        void SmBusWriteByteCommand(I2cAddress address, byte command, byte data, bool errorCheck = false);

        /// <summary>
        /// Read a <see cref="short"/> from the bus with a command code
        /// </summary>
        /// <param name="address">The <see cref="I2cAddress"/></param>
        /// <param name="command">The <see cref="byte"/> command</param>
        /// <param name="pec">Read data with packet error check</param>
        /// <returns>The short read from the bus</returns>
        short SmBusReadShortCommand(I2cAddress address, byte command, bool errorCheck = false);

        /// <summary>
        /// Write a <see cref="short"/> to the bus with a command code
        /// </summary>
        /// <param name="address">The <see cref="I2cAddress"/></param>
        /// <param name="command">The <see cref="byte"/> command</param>
        /// <param name="data">The <see cref="short"/> data</param>
        /// <param name="errorCheck">Perform packet error check on response</param>
        void SmBusWriteShortCommand(I2cAddress address, byte command, short data, bool errorCheck = false);

        /// <summary>
        /// Read a <see cref="int"/> from the bus with a command code
        /// </summary>
        /// <param name="address">The <see cref="I2cAddress"/></param>
        /// <param name="command">The <see cref="byte"/> command</param>
        /// <param name="pec">Read data with packet error check</param>
        /// <returns>Returns an <see cref="int"/> read from the bus</returns>
        int SmBusReadIntCommand(I2cAddress address, byte command, bool errorCheck = false);

        /// <summary>
        /// Write a <see cref="int"/> to the bus with a command code
        /// </summary>
        /// <param name="address">The <see cref="I2cAddress"/></param>
        /// <param name="command">The <see cref="byte"/> command</param>
        /// <param name="data">The <see cref="short"/> data</param>
        /// <param name="errorCheck">Perform packet error check on response</param>
        void SmBusWriteIntCommand(I2cAddress address, byte command, int data, bool errorCheck = false);

        /// <summary>
        /// Read a <see cref="long"/> from the bus with a command code
        /// </summary>
        /// <param name="address">The <see cref="I2cAddress"/></param>
        /// <param name="command">The <see cref="byte"/> command</param>
        /// <param name="pec">Read data with packet error check</param>
        /// <returns>The long int value read from the bus</returns>
        long SmBusReadLongCommand(I2cAddress address, byte command, bool errorCheck = false);

        /// <summary>
        /// Write a <see cref="long"/> to the bus with a command code
        /// </summary>
        /// <param name="address">The <see cref="I2cAddress"/></param>
        /// <param name="command">The <see cref="byte"/> command</param>
        /// <param name="data">The <see cref="short"/> data</param>
        /// <param name="errorCheck">Perform packet error check on response</param>
        void SmBusWriteLongCommand(I2cAddress address, byte command, long data, bool errorCheck = false);

        /// <summary>
        /// Read a block of bytes from the bus with a command code
        /// </summary>
        /// <param name="address">The <see cref="I2cAddress"/></param>
        /// <param name="command">The <see cref="byte"/> command</param>
        /// <param name="count">The number of bytes to read from the bus</param>
        /// <param name="pec">Read data with packet error check</param>
        /// <returns>The <see cref="IList{T}"/> of <see cref="byte"/> read from the bus</returns>
        IList<byte> SmBusBlockRead(I2cAddress address, byte command, byte count, bool errorCheck = false);

        /// <summary>
        /// Write a block of bytes to the bus with a command code
        /// </summary>
        /// <param name="address">The <see cref="I2cAddress"/></param>
        /// <param name="command">The <see cref="byte"/> command</param>
        /// <param name="block">The <see cref="IList{T}"/> of <see cref="byte"/></param>
        /// <param name="errorCheck">Perform packet error check on response</param>
        void SmBusBlockWrite(I2cAddress address, byte command, IList<byte> block, bool errorCheck = false);
    }
}
