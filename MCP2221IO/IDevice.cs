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

        ///// <summary>
        ///// Get or Set the communication speed for I2C/SMBus operations.
        ///// Accepted values are between <see cref="I2CMinSpeed"/> and <see cref="I2CMinSpeed"/>
        ///// </summary>
        ///// <remarks>
        ///// NOTE: The speed may fail to be set if an I2C/SMBus operation is already in progress or in a
        ///// timeout situation.The <see cref="I2CCancelCurrentTransfer"/> function can be used to free the bus
        ///// before retrying to set the speed.
        ///// </remarks>
        //int Speed { get; set; }

        ///// <summary>
        ///// Gets or Set the time the MCP2221 will wait after sending the "read" command before trying to
        ///// read back the data from the I2C/SMBus slave and the maximum number of retries if data
        ///// couldn't be read back.
        ///// </summary>
        //CommParameters CommParameters { get; set; }

        ////ChipSettings ChipSettings { get; set; }

        ////GpSettings GpSettings { get; set; }

        //string Manufacturer { get; set; }

        //string Product { get; set; }

        //string SerialNumber { get; set; }

        //long FactorySerialNumber { get; }



        ////UsbPowerAttributes UsbPowerAttributes { get; set; }

        ////void Unlock(long code);

        ////void Reset();

        ///// <summary>
        ///// Cancel the current I2C/SMBus transfer
        ///// </summary>
        //void I2CCancelCurrentTransfer();

        /////// <summary>
        /////// Read I2C data from a slave
        /////// </summary>
        /////// <param name="bytesToRead">The number of bytes to read from the slave. Valid range is between 1 and 65535</param>
        /////// <param name="slaveAddress">7bit or 8bit I2C slave address, depending on the value of the <paramref name="use7BitAddress"/> flag. 
        /////// For 8 bit addresses, the R/W LSB of the address is set to 1 inside the function.</param>
        /////// <param name="use7BitAddress">If true 7 bit address will be used for the slave.</param>
        /////// <returns>A list of bytes read from the addressed slave</returns>
        /////// <remarks>
        /////// if the "Mcp2221_SetSpeed" function has not been called for the provided handle, the default
        /////// speed of 100kbps will be configured and used.Otherwise, the speed will not be reconfigured.
        /////// </remarks>
        ////IList<byte> I2CRead(uint bytesToRead, byte slaveAddress, bool use7BitAddress);

        ////void I2cWrite(IList<byte> bytes, byte slaveAddress, bool use7BitAddress);
    }
}
