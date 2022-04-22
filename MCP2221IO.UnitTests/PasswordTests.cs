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
using MCP2221IO.Settings;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace MCP2221IO.UnitTests
{
    public class PasswordTests
    {
        private readonly ITestOutputHelper _output;

        public PasswordTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void TestOk()
        {
            // Arrange
            List<byte> bytes = new List<byte>() { 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55 };

            // Act
            Password password = new Password(bytes);

            // Assert
            password.Value.Should().Be("AA55AA55AA55AA55");
            password.Bytes.Should().Contain(bytes);
        }

        [Fact]
        public void TestStringOk()
        {
            // Arrange
            List<byte> bytes = new List<byte>() { 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55 };
            string test = "AA55AA55AA55AA55";

            // Act
            Password password = new Password(test);

            // Assert
            password.Value.Should().Be("AA55AA55AA55AA55");
            password.Bytes.Should().Contain(bytes);
        }
    }
}
