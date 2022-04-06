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
using MCP2221IO.Settings;
using MCP2221IO.Usb;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
        public void TestReadChipSettings()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()))
                .Callback<Stream, Stream>
                (
                    (a, b) =>
                    {
                        WriteChipSettingsResponse(b);
                    }
                );

            // Act
            _device.ReadChipSettings();

            // Assert
            _device.ChipSettings.Should().NotBeNull();

            _output.WriteLine(_device.ChipSettings.ToString());
        }

        [Fact]
        public void TestWriteChipSettings()
        {
            // Arrange

            // Act
            _device.WriteChipSettings();

            // Assert
        }

        [Fact]
        public void TestReadSramSettings()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()))
                .Callback<Stream, Stream>
                (
                    (a, b) =>
                    {
                        WriteReadSramSettingsResponse(b);
                    }
                );

            // Act
            _device.ReadSramSettings();

            // Assert
            _device.SramSettings.Should().NotBeNull();

            _output.WriteLine(_device.SramSettings.ToString());
        }

        [Fact]
        public void TestWriteSramSettings()
        {
            // Arrange

            // Act
            _device.WriteSramSettings();

            // Assert
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
        public void TestGpioPortsOk()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()))
                .Callback<Stream, Stream>
                (
                    (a, b) =>
                    {
                        WriteGpioPortsResponse(b);
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

        [Fact]
        public void TestGetUsbManufacturerDescriptorOk()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()))
                .Callback<Stream, Stream>
                (
                    (a, b) =>
                    {
                        WriteStringResponse(b, "MANUFACTURER");
                    }
                );

            // Act
            string descriptor = _device.UsbManufacturerDescriptor;

            // Assert
            descriptor.Should().NotBeNull();

            _output.WriteLine(descriptor);
        }

        [Fact]
        public void TestSetUsbManufacturerDescriptorOk()
        {
            // Arrange
            Stream writeStream = null;

            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()))
                .Callback<Stream, Stream>
                (
                    (a, b) =>
                    {
                        WriteFlashWriteResponse(b, 0);
                        writeStream = a;
                    }
                );

            // Act
            _device.UsbManufacturerDescriptor = "UPDATED";

            // Assert
            writeStream.Should().NotBeNull();
        }

        [Fact]
        public void TestGetUsbProductDescriptorOk()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()))
                .Callback<Stream, Stream>
                (
                    (a, b) =>
                    {
                        WriteStringResponse(b, "PRODUCT");
                    }
                );

            // Act
            string descriptor = _device.UsbProductDescriptor;

            // Assert
            descriptor.Should().NotBeNull();

            _output.WriteLine(descriptor);
        }

        [Fact]
        public void TestSetUsbProductDescriptorOk()
        {
            // Arrange
            Stream writeStream = null;

            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()))
                .Callback<Stream, Stream>
                (
                    (a, b) =>
                    {
                        WriteFlashWriteResponse(b, 0);
                        writeStream = a;
                    }
                );

            // Act
            _device.UsbProductDescriptor = "UPDATED";

            // Assert
            writeStream.Should().NotBeNull();
        }

        [Fact]
        public void TestGetUsbSerialNumberDescriptorOk()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()))
                .Callback<Stream, Stream>
                (
                    (a, b) =>
                    {
                        WriteStringResponse(b, "SERIAL NUMBER");
                    }
                );

            // Act
            string descriptor = _device.UsbSerialNumberDescriptor;

            // Assert
            descriptor.Should().NotBeNull();

            _output.WriteLine(descriptor);
        }

        [Fact]
        public void TestSetUsbSerialNumberDescriptorOk()
        {
            // Arrange
            Stream writeStream = null;

            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()))
                .Callback<Stream, Stream>
                (
                    (a, b) =>
                    {
                        WriteFlashWriteResponse(b, 0);
                        writeStream = a;
                    }
                );

            // Act
            _device.UsbSerialNumberDescriptor = "UPDATED";

            // Assert
            writeStream.Should().NotBeNull();
        }

        [Fact]
        public void TestGetFactorySerialNumberOk()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()))
                .Callback<Stream, Stream>
                (
                    (a, b) =>
                    {
                        WriteSerialNumberResponse(b);
                    }
                );

            // Act
            string serialNumber = _device.FactorySerialNumber;

            // Assert
            serialNumber.Should().NotBeNull();

            _output.WriteLine(serialNumber);
        }

        [Fact]
        public void TestUnlockFlash()
        {
            // Arrange
            Stream writeStream = null;

            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()))
                .Callback<Stream, Stream>
                (
                    (a, b) =>
                    {
                        WriteFlashUnlockResponse(b);
                        writeStream = a;
                    }
                );

            // Act
            Action act = () => { _device.UnlockFlash(0xAA55AA55DEADBEEF); };

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void TestI2CWriteDataTest()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()))
                .Callback<Stream, Stream>
                (
                    (a, b) =>
                    {
                        WriteReponse(b, CommandCodes.WriteI2CData);
                    }
                );

            var buffer = new List<byte>() { };

            for (int i = 0; i < 1000; i++)
            {
                buffer.Add(0xFF);
            }

            // Act
            _device.I2CWriteData(0xFF, buffer);

            // Assert
            _mockUsbDevice.Verify(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()), Times.Exactly(17));
        }

        [Fact]
        public void TestI2CWriteDataNoStopTest()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()))
                .Callback<Stream, Stream>
                (
                    (a, b) =>
                    {
                        WriteReponse(b, CommandCodes.WriteI2CDataNoStop);
                    }
                );

            var buffer = new List<byte>() { };

            for (int i = 0; i < 1000; i++)
            {
                buffer.Add(0xFF);
            }

            // Act
            _device.I2CWriteDataNoStop(0xFF, buffer);

            // Assert
            _mockUsbDevice.Verify(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()), Times.Exactly(17));
        }

        [Fact]
        public void TestI2CWriteDataTestRepartStart()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()))
                .Callback<Stream, Stream>
                (
                    (a, b) =>
                    {
                        WriteReponse(b, CommandCodes.WriteI2CDataRepeatedStart);
                    }
                );

            var buffer = new List<byte>() { };

            for (int i = 0; i < 1000; i++)
            {
                buffer.Add(0xFF);
            }

            // Act
            _device.I2CWriteDataRepeatStart(0xFF, buffer);

            // Assert
            _mockUsbDevice.Verify(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()), Times.Exactly(17));
        }

        [Fact]
        public void TestI2CRead()
        {
            const int DataLength = 130;
            bool startWritten = false;
            int blockCount = 0;

            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()))
                .Callback<Stream, Stream>
                (
                    (a, b) =>
                    {
                        if (!startWritten)
                        {
                            WriteReponse(b, CommandCodes.ReadI2CData);
                            startWritten = true;
                        }
                        else
                        {
                            WriteGetI2CDataResponse(b, CommandCodes.GetI2CData, Math.Min(Device.MaxBlockSize, Math.Abs(DataLength - (blockCount++ * Device.MaxBlockSize))));
                        }
                    }
                );

            // Act
            var buffer = _device.I2CReadData(0xFF, DataLength);

            // Assert
            _mockUsbDevice.Verify(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()), Times.Exactly(4));
            buffer.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void TestI2CReadRepeatedStart()
        {
            const int DataLength = 130;
            bool startWritten = false;
            int blockCount = 0;

            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()))
                .Callback<Stream, Stream>
                (
                    (a, b) =>
                    {
                        if (!startWritten)
                        {
                            WriteReponse(b, CommandCodes.ReadI2CDataRepeatedStart);
                            startWritten = true;
                        }
                        else
                        {
                            WriteGetI2CDataResponse(b, CommandCodes.GetI2CData, Math.Min(Device.MaxBlockSize, Math.Abs(DataLength - (blockCount++ * Device.MaxBlockSize))));
                        }
                    }
                );

            // Act
            var buffer = _device.I2CReadDataRepeatedStart(0xFF, DataLength);

            // Assert
            _mockUsbDevice.Verify(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()), Times.Exactly(4));
            buffer.Should().NotBeNullOrEmpty();
        }

        private void WriteReadSramSettingsResponse(Stream stream)
        {
            stream.Write(new byte[64], 0, 64);
            stream.Position = 0;
            stream.WriteByte((byte) CommandCodes.GetSram);
            stream.WriteByte(0); // Command OK
            stream.WriteByte(0); // sram structure length
            stream.WriteByte(0); // gp structure length care
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
            stream.WriteByte(0x55); // password
            stream.WriteByte(0xAA); // password
            stream.WriteByte(0x55); // password
            stream.WriteByte(0xAA); // password
            stream.WriteByte(0x55); // password
            stream.WriteByte(0xAA); // password
            stream.WriteByte(0x55); // password
            stream.WriteByte(0xAA); // password
        }

        private void WriteGetI2CDataResponse(Stream stream, CommandCodes commandCode, int length)
        {
            stream.Write(new byte[64], 0, 64);
            stream.Position = 0;
            stream.WriteByte((byte)commandCode);
            stream.WriteByte(0);
            stream.WriteByte((byte)length);
            stream.Write(new byte[length], 0, length);
        }

        private void WriteReponse(Stream stream, CommandCodes commandCode)
        {
            stream.Write(new byte[64], 0, 64);
            stream.Position = 0;
            stream.WriteByte((byte)commandCode);
        }

        private void WriteFlashUnlockResponse(Stream stream)
        {
            stream.Write(new byte[64], 0, 64);
            stream.Position = 0;
            stream.WriteByte((byte)CommandCodes.SendFlashAccessPassword);
            stream.WriteByte(0);
        }

        private void WriteFlashWriteResponse(Stream stream, int status)
        {
            stream.Write(new byte[64], 0, 64);
            stream.Position = 0;
            stream.WriteByte((byte)CommandCodes.WriteFlashData);
            stream.WriteByte((byte)status);
        }

        private void WriteSerialNumberResponse(Stream stream)
        {
            stream.Write(new byte[64], 0, 64);
            stream.Position = 0;
            stream.WriteByte((byte)CommandCodes.ReadFlashData);
            stream.WriteByte(0);
            stream.WriteByte(8);
            stream.WriteByte(0);
            stream.WriteByte(0x55);
            stream.WriteByte(0xAA);
            stream.WriteByte(0xFE);
            stream.WriteByte(0xED);
            stream.WriteByte(0xDE);
            stream.WriteByte(0xAD);
            stream.WriteByte(0xBE);
            stream.WriteByte(0xEF);
        }

        private void WriteStringResponse(Stream stream, string value)
        {
            stream.Write(new byte[64], 0, 64);
            stream.Position = 0;
            stream.WriteByte((byte)CommandCodes.ReadFlashData);
            stream.WriteByte(0);
            stream.WriteByte((byte)((value.Length + 1) * 2));
            stream.WriteByte(3);
            var bytes = Encoding.Unicode.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        private void WriteGpioPortsResponse(Stream stream)
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

        private void WriteChipSettingsResponse(Stream stream)
        {
            stream.Write(new byte[64], 0, 64);
            stream.Position = 0;
            stream.WriteByte((byte)CommandCodes.ReadFlashData);
            stream.WriteByte(0); // Command OK
            stream.WriteByte(0); // structure length
            stream.WriteByte(0); // dont care
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
