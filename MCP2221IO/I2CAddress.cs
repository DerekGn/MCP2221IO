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
using System.Collections.Generic;

namespace MCP2221IO
{
    /// <summary>
    /// Represents an I2C device address
    /// </summary>
    public class I2CAddress
    {
        public const uint SevenBitRangeLower = 0x07;
        public const uint SevenBitRangeUpper = 0x78;
        public const uint TenBitRangeUpper = 0x03FF;

        public I2CAddress(uint address) : this(address, I2CAddressSize.SevenBit)
        {
        }

        public I2CAddress(uint address, I2CAddressSize size)
        {
            Size = size;
            
            if(Size == I2CAddressSize.SevenBit)
            {
                if ((address <= SevenBitRangeLower) || (address >= SevenBitRangeUpper))
                {
                    throw new ArgumentOutOfRangeException(nameof(address), address, $"{I2CAddressSize.SevenBit} Address must between the range of 0x{SevenBitRangeLower:X2} and 0x{SevenBitRangeUpper:X2}");
                }
            }
            else
            {
                if (address > TenBitRangeUpper)
                {
                    throw new ArgumentOutOfRangeException(nameof(address), address, $"{I2CAddressSize.TenBit} Address must between the range of 0x00 and 0x{TenBitRangeUpper:X4}");
                }
            }

            CalculateAddress(address, size);
        }

        public uint Value { get; private set; }

        public IReadOnlyCollection<byte> ReadAddress { get; private set; }

        public IReadOnlyCollection<byte> WriteAddress { get; private set; }

        public I2CAddressSize Size { get; }

        private void CalculateAddress(uint address, I2CAddressSize size)
        {
            switch (size)
            {
                case I2CAddressSize.SevenBit:
                    Value = address;

                    var baseAddress = (byte)(address << 1);

                    WriteAddress = new List<byte>() { baseAddress }.AsReadOnly();
                    ReadAddress = new List<byte>() { (byte)(baseAddress + 1) }.AsReadOnly();
                    break;
                case I2CAddressSize.TenBit:
                    var msb = ((address | 0xF800) & 0xFB00) >> 7;
                    var lsb = address & 0x00FF;
                    
                    Value = address;

                    WriteAddress = new List<byte>() { (byte)msb, (byte)lsb }.AsReadOnly();
                    ReadAddress = new List<byte>() { (byte)(msb + 1), (byte)lsb }.AsReadOnly();
                    break;
                default:
                    break;
            }
        }
    }
}