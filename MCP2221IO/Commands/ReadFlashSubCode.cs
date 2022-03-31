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
    /// The specific flash section to read
    /// </summary>
    internal enum ReadFlashSubCode
    {
        /// <summary>
        /// Read Chip Settings – it will read the MCP2221 flash settings
        /// </summary>
        ReadChipSettings = 0x00,
        /// <summary>
        /// Read GP Settings – it will read the MCP2221 flash GP settings
        /// </summary>
        ReadGpSettings = 0x01,
        /// <summary>
        /// Read USB Manufacturer Descriptor String – reads the USB Manufacturer 
        /// String Descriptor used during the USB enumeration
        /// </summary>
        ReadManufacturerDescriptor = 0x02,
        /// <summary>
        /// Read USB Product Descriptor String– reads the USB Product String 
        /// Descriptor used during the USB enumeration
        /// </summary>
        ReadUSBProductDescriptor = 0x03,
        /// <summary>
        ///  Read USB Serial Number Descriptor String – reads the USB Serial 
        ///  Number String Descriptor that is used during USB enumeration. 
        ///  This serial number can be changed by the user through a specific USB HID command.
        /// </summary>
        ReadUSBSerialNumberDescriptor = 0x04,
        /// <summary>
        /// Read Chip Factory Serial Number – reads the factory-set serial 
        /// number. This serial number cannot be changed.
        /// </summary>
        ReadChipFactorySerialNumber = 0x05
    }
}