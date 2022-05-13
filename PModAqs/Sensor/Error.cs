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

namespace PModAqs.Sensor
{
    [Flags]
    internal enum Error
    {
        /// <summary>
        /// The CCS811 received an I²C write request 
        /// addressed to this station but with
        /// invalid register address ID
        /// </summary>
        WriteRegInvalid = 0x01,
        /// <summary>
        /// The CCS811 received an I²C read request
        /// to a mailbox ID that is invalid
        /// </summary>
        ReadRegInvalid = 0x02,
        /// <summary>
        /// The CCS811 received an I²C request
        /// to write an unsupported mode to MEAS_MODE
        /// </summary>
        MeasureModeInvalid = 0x04,
        /// <summary>
        /// The sensor resistance measurement 
        /// has reached or exceeded the maximum range
        /// </summary>
        MaxResistance = 0x08,
        /// <summary>
        /// The Heater current in the CCS811 is not in range
        /// </summary>
        HeaterFault = 0x10,
        /// <summary>
        /// The Heater voltage is not being applied correctly
        /// </summary>
        HeaterSupply = 0x10
    }
}
