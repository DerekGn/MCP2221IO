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
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Xunit.Abstractions;

namespace MCP2221IO.UnitTests
{
    public class I2cAddressTests
    {
        private readonly ITestOutputHelper _output;

        public I2cAddressTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void TestSevenBitAddress()
        {
            // Arrange Act
            I2cAddress address = new I2cAddress(0x08);

            // Assert
            Assert.Equal(I2cAddressSize.SevenBit, address.Size);
            Assert.Equal(new List<byte> { 0x10 }, address.WriteAddress);
            Assert.Equal(new List<byte> { 0x11 }, address.ReadAddress);
        }

        [Fact]
        [SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "<Pending>")]
        public void TestSevenBitAddressException()
        {
            ArgumentException exception = Assert.Throws<ArgumentOutOfRangeException>(() => { I2cAddress address = new I2cAddress(0x07); });
        }

        [Theory]
        [InlineData(0x318, 0xF6, 0x18)]
        [InlineData(0x218, 0xF4, 0x18)]
        [InlineData(0x118, 0xF2, 0x18)]
        [InlineData(0x020, 0xF0, 0x20)]
        public void TestTenBitAddress(uint address, byte msb, byte lsb)
        {
            // Arrange Act
            I2cAddress i2cAddress = new I2cAddress(address, I2cAddressSize.TenBit);

            // Assert
            Assert.Equal(I2cAddressSize.TenBit, i2cAddress.Size);
            Assert.Equal(new List<byte> { msb, lsb }, i2cAddress.WriteAddress);
            Assert.Equal(new List<byte> { (byte)(msb + 1), lsb }, i2cAddress.ReadAddress);
        }

        [Fact]
        [SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "<Pending>")]
        public void TestTenBitAddressException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => { I2cAddress address = new I2cAddress(0x4FF, I2cAddressSize.TenBit); });
        }
    }
}
