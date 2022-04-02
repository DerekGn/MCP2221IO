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


    }
}
