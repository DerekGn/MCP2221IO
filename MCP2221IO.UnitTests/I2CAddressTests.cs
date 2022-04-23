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

using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace MCP2221IO.UnitTests
{
    public class I2CAddressTests
    {
        private readonly ITestOutputHelper _output;

        public I2CAddressTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void TestSevenBitAddress()
        {
            // Arrange Act
            I2CAddress address = new I2CAddress(0x08);

            // Assert
            address.Size.Should().Be(I2CAddressSize.SevenBit);
            address.WriteAddress.Should().Equal(new List<byte> { 0x10 });
            address.ReadAddress.Should().Equal(new List<byte> { 0x11 });
        }

        [Fact]
        public void TestSevenBitAddressException()
        {
            // Arrange Act
            Action act = () => { I2CAddress address = new I2CAddress(0x07); };

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(0x318, 0xF6, 0x18)]
        [InlineData(0x218, 0xF4, 0x18)]
        [InlineData(0x118, 0xF2, 0x18)]
        [InlineData(0x020, 0xF0, 0x20)]
        public void TestTenBitAddress(uint address, byte msb, byte lsb)
        {
            // Arrange Act
            I2CAddress i2cAddress = new I2CAddress(address, I2CAddressSize.TenBit);

            // Assert
            i2cAddress.Size.Should().Be(I2CAddressSize.TenBit);
            i2cAddress.WriteAddress.Should().Equal(new List<byte> { msb, lsb });
            i2cAddress.ReadAddress.Should().Equal(new List<byte> { (byte)(msb + 1), lsb });
        }

        [Fact]
        public void TestTenBitAddressException()
        {
            // Arrange Act
            Action act = () => { I2CAddress address = new I2CAddress(0x4FF, I2CAddressSize.TenBit); };

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
