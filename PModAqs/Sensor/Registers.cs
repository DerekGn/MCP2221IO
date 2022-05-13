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

namespace PModAqs.Sensor
{
    internal enum Registers : byte
    {
        /// <summary>
        /// Status register
        /// </summary>
        Status = 0x00,
        /// <summary>
        /// Measurement mode and conditions register
        /// </summary>
        Mode = 0x01,
        /// <summary>
        /// Algorithm result. The most significant 2 bytes contain a
        /// ppm estimate of the equivalent CO2 (eCO2) level, and
        /// the next two bytes contain a ppb estimate of the total
        /// VOC level.
        /// </summary>
        ResultData = 0x02,
        /// <summary>
        /// Raw ADC data values for resistance and current source used.
        /// </summary>
        RawData = 0x03,
        /// <summary>
        /// Temperature and humidity data can be written to
        /// enable compensation.
        /// </summary>
        EnvironmentData = 0x05,
        /// <summary>
        /// The NTC internal resistor
        /// </summary>
        Ntc = 0x06,
        /// <summary>
        /// Thresholds for operation when interrupts are only
        /// generated when eCO2 ppm crosses a threshold
        /// </summary>
        Thresholds = 0x10,
        /// <summary>
        /// The encoded current baseline value can be read. A
        /// previously saved encoded baseline can be written.
        /// </summary>
        Baseline = 0x11,
        /// <summary>
        /// Hardware ID
        /// </summary>
        HwId = 0x20,
        /// <summary>
        /// Hardware Version
        /// </summary>
        HwVersion = 0x21,
        /// <summary>
        /// Firmware Boot Version. The first 2 bytes contain the
        /// firmware version number for the boot code.
        /// </summary>
        FirmwareBootVersion = 0x23,
        /// <summary>
        /// Firmware Application Version. The first 2 bytes contain
        /// the firmware version number for the application code
        /// </summary>
        FirmwareAppVersion = 0x24,
        /// <summary>
        /// Internal Status register
        /// </summary>
        InternalState = 0xA0,
        /// <summary>
        /// Error ID. When the status register reports an error its
        /// source is located in this register
        /// </summary>
        ErrorId = 0xE0,
        /// <summary>
        /// To prevent accidental APP_ERASE a sequence of four bytes must be
        /// written to this register in a single I²C sequence: 0xE7, 0xA7,
        /// 0xE6, 0x09
        /// </summary>
        ApplicationErase = 0xF1,
        /// <summary>
        /// Nine byte, write only register for sending small chunks of
        /// application data which will be written in order to the CCS811 flash code
        /// </summary>
        ApplicationData = 0xF2,
        /// <summary>
        /// To change the mode of the CCS811 from Boot mode to running the
        /// application, a single byte write of 0xF4 is required.
        /// </summary>
        ApplicationStart = 0xF4,
        /// <summary>
        /// If the correct 4 bytes (0x11 0xE5 0x72 0x8A) are written
        /// to this register in a single sequence the device will reset
        /// and return to BOOT mode
        /// </summary>
        SwReset = 0xFF
    }
}
