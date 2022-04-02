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
using MCP2221IO.Commands;
using MCP2221IO.Gpio;
using MCP2221IO.Responses.Exceptions;
using MCP2221IO.Usb;
using Moq;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace MCP2221IO.UnitTests
{
    public class DeviceTests
    {
        private readonly Mock<IUsbDevice> _mockUsbDevice;
        private readonly ITestOutputHelper _output;
        private readonly Device _device;

        public DeviceTests(ITestOutputHelper output)
        {
            _mockUsbDevice = new Mock<IUsbDevice>();
            _device = new Device(_mockUsbDevice.Object);
            _output = output;
        }

        [Fact]
        public void TestDeviceStatusInvalidStreamLengthException()
        {
            // Arrange

            // Act
            Action act = () => { DeviceStatus deviceStatus = _device.Status; };

            // Assert
            act.Should().Throw<InvalidStreamLengthException>();
        }

        [Fact]
        public void TestDeviceStatusInvalidResponseTypeException()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()))
                .Callback<Stream, Stream>
                (
                    (a, b) =>
                    {
                        b.WriteByte(0xFF);
                        b.Write(new byte[63], 0, 63);
                    }
                );

            // Act
            Action act = () => { DeviceStatus deviceStatus = _device.Status; };

            // Assert
            act.Should().Throw<InvalidResponseTypeException>();
        }

        [Fact]
        public void TestDeviceStatusOk()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()))
                .Callback<Stream, Stream>
                (
                    (a, b) =>
                    {
                        WriteTestStatusSetParametersResponse(b, 0);
                    }
                );

            // Act
            DeviceStatus deviceStatus = _device.Status;

            // Assert
            deviceStatus.Should().NotBeNull();

            _output.WriteLine(deviceStatus.ToString());
        }

        [Fact]
        public void TestChipSettingsOk()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()))
                .Callback<Stream, Stream>
                (
                    (a, b) =>
                    {
                        WriteTestChipSettingsResponse(b);
                    }
                );

            // Act
            ChipSettings chipSettings = _device.ChipSettings;

            // Assert
            chipSettings.Should().NotBeNull();

            _output.WriteLine(chipSettings.ToString());
        }

        [Fact]
        public void TestGpioPortsOk()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()))
                .Callback<Stream, Stream>
                (
                    (a, b) =>
                    {
                        WriteGpioPorts(b);
                    }
                );

            // Act
            GpioPorts ports = _device.GpioPorts;

            // Assert
            ports.Should().NotBeNull();
            ports.Gpio0Settings.Should().NotBeNull();
            ports.Gpio1Settings.Should().NotBeNull();
            ports.Gpio2Settings.Should().NotBeNull();
            ports.Gpio3Settings.Should().NotBeNull();

            _output.WriteLine(ports.ToString());
        }

        private void WriteGpioPorts(Stream stream)
        {
            stream.Write(new byte[64], 0, 64);
            stream.Position = 0;
            stream.WriteByte((byte)CommandCodes.ReadFlashData);
            stream.Write(new byte[2], 0, 2);
            stream.WriteByte(0x18); // GIO0
            stream.WriteByte(0x19); // GIO1
            stream.WriteByte(0x1A); // GIO2
            stream.WriteByte(0x1B); // GIO3
        }

        private void WriteTestChipSettingsResponse(Stream stream)
        {
            stream.Write(new byte[64], 0, 64);
            stream.Position = 0;
            stream.WriteByte((byte)CommandCodes.ReadFlashData);
            stream.WriteByte(0);
            stream.WriteByte(0);
            stream.WriteByte(0xF1); // CDC enabled
            stream.WriteByte(0x02); // clock divider
            stream.WriteByte(0xFF); // DAC
            stream.WriteByte(0xFF); // int and adc
            stream.WriteByte(0xED); // VID
            stream.WriteByte(0xFE); // VID
            stream.WriteByte(0xAD); // PID
            stream.WriteByte(0xDE); // PID
            stream.WriteByte(0x60); // Usb
            stream.WriteByte(0x32); // usb power
        }

        private void WriteTestStatusSetParametersResponse(Stream stream, byte status)
        {
            stream.Write(new byte[64], 0, 64);
            stream.Position = 0;
            stream.WriteByte((byte)CommandCodes.StatusSetParameters);
            stream.WriteByte(status);
            stream.WriteByte((byte)I2CCancelTransferState.MarkedForCancellation);
            stream.WriteByte((byte)I2CSpeedStatus.Set);
            stream.WriteByte(0x55);
            stream.Write(new byte[3], 0, 3);
            stream.WriteByte(0XFF);
            stream.WriteByte(0xAF);
            stream.WriteByte(0xDE);
            stream.WriteByte(0xED);
            stream.WriteByte(0xFE);
            stream.WriteByte(0xAA);
            stream.WriteByte(0x55);
            stream.WriteByte(0xFF);
            stream.WriteByte(0xAD);
            stream.WriteByte(0xDE);

            stream.Write(new byte[4], 0, 4);

            stream.WriteByte(0x01);
            stream.WriteByte(0x01);
            stream.WriteByte(0x01);
            stream.WriteByte(0x02);

            stream.Write(new byte[19], 0, 19);
            
            stream.WriteByte((byte)'A');
            stream.WriteByte((byte)'6');
            stream.WriteByte((byte)'1');
            stream.WriteByte((byte)'1');
            stream.WriteByte(0xAD);
            stream.WriteByte(0xDE);
            stream.WriteByte(0xED);
            stream.WriteByte(0xFE);
            stream.WriteByte(0xAA);
            stream.WriteByte(0x55);
        }
    }
}
