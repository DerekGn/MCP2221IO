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
using MCP2221IO.Settings;
using MCP2221IO.Usb;
using Microsoft.Extensions.Logging;
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
        private readonly Mock<IHidDevice> _mockUsbDevice;
        private readonly ITestOutputHelper _output;
        private readonly Device _device;

        public DeviceTests(ITestOutputHelper output)
        {
            _mockUsbDevice = new Mock<IHidDevice>();
            _device = new Device(Mock.Of<ILogger<IDevice>>(), _mockUsbDevice.Object);
            _output = output;
        }

        [Fact]
        public void TestReadDeviceStatus()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteTestStatusSetParametersResponse(0));

            // Act
            _device.ReadDeviceStatus();

            // Assert
            _device.Status.Should().NotBeNull();

            _output.WriteLine(_device.Status.ToString());
        }

        [Fact]
        public void TestReadChipSettings()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteChipSettingsResponse());

            // Act
            _device.ReadChipSettings();

            // Assert
            _device.ChipSettings.Should().NotBeNull();

            _output.WriteLine(_device.ChipSettings.ToString());
        }

        [Fact]
        public void TestReadGpSettings()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteGpSettingsResponse());

            // Act
            _device.ReadGpSettings();

            // Assert
            _device.GpSettings.Should().NotBeNull();

            _output.WriteLine(_device.GpSettings.ToString());
        }

        [Fact]
        public void TestReadSramSettings()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReadSramSettingsResponse());

            // Act
            _device.ReadSramSettings();

            // Assert
            _device.SramSettings.Should().NotBeNull();

            _output.WriteLine(_device.SramSettings.ToString());
        }

        [Fact]
        public void TestReadGpioSettings()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReadGpioPortsResponse());

            // Act
            _device.ReadGpioPorts();

            // Assert
            _mockUsbDevice.Verify(_ => _.WriteRead(It.IsAny<byte[]>()), Times.Once);

            _output.WriteLine($"GpioPort0: {_device.GpioPort0}");
            _output.WriteLine($"GpioPort1: {_device.GpioPort1}");
            _output.WriteLine($"GpioPort2: {_device.GpioPort2}");
            _output.WriteLine($"GpioPort3: {_device.GpioPort3}");
        }

        [Fact]
        public void TestWriteChipSettings()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteFlashWriteResponse(0));

            _device.ChipSettings = new ChipSettings();

            // Act
            _device.WriteChipSettings(new Password(new List<byte>()));

            // Assert
            _mockUsbDevice.Verify(_ => _.WriteRead(It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public void TestWriteGpSettings()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteFlashWriteResponse(0));

            _device.GpSettings = new GpSettings()
            {
                Gp0PowerUpSetting = new GpSetting<Gpio0Designation>(),
                Gp1PowerUpSetting = new GpSetting<Gpio1Designation>(),
                Gp2PowerUpSetting = new GpSetting<Gpio2Designation>(),
                Gp3PowerUpSetting = new GpSetting<Gpio3Designation>()
            };

            // Act
            _device.WriteGpSettings();

            // Assert
            _mockUsbDevice.Verify(_ => _.WriteRead(It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public void TestWriteSramSettings()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteFlashWriteResponse(0));

            _device.SramSettings = new SramSettings()
            {
                Gp0Settings = new GpSetting<Gpio0Designation>(),
                Gp1Settings = new GpSetting<Gpio1Designation>(),
                Gp2Settings = new GpSetting<Gpio2Designation>(),
                Gp3Settings = new GpSetting<Gpio3Designation>()
            };

            // Act
            _device.WriteSramSettings(true);

            // Assert
            _mockUsbDevice.Verify(_ => _.WriteRead(It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public void WriteGpioPorts()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteSetGpioValuesResponse());
            
            _device._gpioPortsRead = true;
            _device.GpioPort0 = new GpioPort();
            _device.GpioPort1 = new GpioPort();
            _device.GpioPort2 = new GpioPort();
            _device.GpioPort3 = new GpioPort();
            // Act
            _device.WriteGpioPorts();

            // Assert
            _mockUsbDevice.Verify(_ => _.WriteRead(It.IsAny<byte[]>()), Times.Once);
        }

        //[Fact]
        //public void TestDeviceStatusInvalidStreamLengthException()
        //{
        //    // Arrange

        //    // Act
        //    Action act = () => { DeviceStatus deviceStatus = _device.Status; };

        //    // Assert
        //    act.Should().Throw<InvalidStreamLengthException>();
        //}

        //[Fact]
        //public void TestDeviceStatusInvalidResponseTypeException()
        //{
        //    // Arrange
        //    _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()))
        //        .Callback<Stream, Stream>
        //        (
        //            (a, b) =>
        //            {
        //                b.WriteByte(0xFF);
        //                b.Write(new byte[63], 0, 63);
        //            }
        //        );

        //    // Act
        //    Action act = () => { DeviceStatus deviceStatus = _device.Status; };

        //    // Assert
        //    act.Should().Throw<InvalidResponseTypeException>();
        //}

        //[Fact]
        //public void TestDeviceStatusOk()
        //{
        //    // Arrange
        //    _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<Stream>(), It.IsAny<Stream>()))
        //        .Callback<Stream, Stream>
        //        (
        //            (a, b) =>
        //            {
        //                WriteTestStatusSetParametersResponse(b, 0);
        //            }
        //        );

        //    // Act
        //    DeviceStatus deviceStatus = _device.Status;

        //    // Assert
        //    deviceStatus.Should().NotBeNull();

        //    _output.WriteLine(deviceStatus.ToString());
        //}

        [Fact]
        public void TestGetUsbManufacturerDescriptorOk()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteStringResponse("MANUFACTURER"));
            
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
            byte[] descriptor = null;

            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteFlashWriteResponse(0))
                .Callback<byte[]>(b => descriptor = b);

            // Act
            _device.UsbManufacturerDescriptor = "UPDATED";

            // Assert
            descriptor.Should().NotBeNull();
        }

        [Fact]
        public void TestGetUsbProductDescriptorOk()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteStringResponse("PRODUCT"));

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
            byte[] descriptor = null;

            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteFlashWriteResponse(0))
                .Callback<byte[]>(b => descriptor = b);

            // Act
            _device.UsbProductDescriptor = "UPDATED";

            // Assert
            descriptor.Should().NotBeNull();
        }

        [Fact]
        public void TestGetUsbSerialNumberDescriptorOk()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteStringResponse("SERIAL NUMBER"));

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
            byte[] descriptor = null;

            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteFlashWriteResponse(0))
                .Callback<byte[]>(b => descriptor = b);

            // Act
            _device.UsbSerialNumberDescriptor = "SERIAL NUMBER";

            // Assert
            descriptor.Should().NotBeNull();
        }

        [Fact]
        public void TestGetFactorySerialNumberOk()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteSerialNumberResponse());

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

            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteFlashUnlockResponse());

            // Act
            Action act = () => { _device.UnlockFlash(0xAA55AA55DEADBEEF); };

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void TestI2CWriteDataTest()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.WriteI2CData));

            var buffer = new List<byte>() { };

            for (int i = 0; i < 1000; i++)
            {
                buffer.Add(0xFF);
            }

            // Act
            _device.I2CWriteData(0xFF, buffer);

            // Assert
            _mockUsbDevice.Verify(_ => _.WriteRead(It.IsAny<byte[]>()), Times.Exactly(17));
        }

        [Fact]
        public void TestI2CWriteDataNoStopTest()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.WriteI2CDataNoStop));

            var buffer = new List<byte>() { };

            for (int i = 0; i < 1000; i++)
            {
                buffer.Add(0xFF);
            }

            // Act
            _device.I2CWriteDataNoStop(0xFF, buffer);

            // Assert
            _mockUsbDevice.Verify(_ => _.WriteRead(It.IsAny<byte[]>()), Times.Exactly(17));
        }

        [Fact]
        public void TestI2CWriteDataTestRepartStart()
        {
            // Arrange
            _mockUsbDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.WriteI2CDataRepeatedStart));

            var buffer = new List<byte>() { };

            for (int i = 0; i < 1000; i++)
            {
                buffer.Add(0xFF);
            }

            // Act
            _device.I2CWriteDataRepeatStart(0xFF, buffer);

            // Assert
            _mockUsbDevice.Verify(_ => _.WriteRead(It.IsAny<byte[]>()), Times.Exactly(17));
        }

        [Fact]
        public void TestI2CRead()
        {
            const int DataLength = 130;
            int blockCount = 0;

            // Arrange
            _mockUsbDevice.SetupSequence(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.ReadI2CData))
                .Returns(WriteGetI2CDataResponse(CommandCodes.GetI2CData, Math.Min(Device.MaxBlockSize, Math.Abs(DataLength - (blockCount++ * Device.MaxBlockSize)))))
                .Returns(WriteGetI2CDataResponse(CommandCodes.GetI2CData, Math.Min(Device.MaxBlockSize, Math.Abs(DataLength - (blockCount++ * Device.MaxBlockSize)))))
                .Returns(WriteGetI2CDataResponse(CommandCodes.GetI2CData, Math.Min(Device.MaxBlockSize, Math.Abs(DataLength - (blockCount++ * Device.MaxBlockSize)))));

            // Act
            var buffer = _device.I2CReadData(0xFF, DataLength);

            // Assert
            _mockUsbDevice.Verify(_ => _.WriteRead(It.IsAny<byte[]>()), Times.Exactly(4));
            buffer.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void TestI2CReadRepeatedStart()
        {
            const int DataLength = 130;
            int blockCount = 0;

            // Arrange
            _mockUsbDevice.SetupSequence(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.ReadI2CDataRepeatedStart))
                .Returns(WriteGetI2CDataResponse(CommandCodes.GetI2CData, Math.Min(Device.MaxBlockSize, Math.Abs(DataLength - (blockCount++ * Device.MaxBlockSize)))))
                .Returns(WriteGetI2CDataResponse(CommandCodes.GetI2CData, Math.Min(Device.MaxBlockSize, Math.Abs(DataLength - (blockCount++ * Device.MaxBlockSize)))))
                .Returns(WriteGetI2CDataResponse(CommandCodes.GetI2CData, Math.Min(Device.MaxBlockSize, Math.Abs(DataLength - (blockCount++ * Device.MaxBlockSize)))));

            // Act
            var buffer = _device.I2CReadDataRepeatedStart(0xFF, DataLength);

            // Assert
            _mockUsbDevice.Verify(_ => _.WriteRead(It.IsAny<byte[]>()), Times.Exactly(4));
            buffer.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void TestReset()
        {
            // Arrange

            // Act
            _device.Reset();

            // Assert
            _mockUsbDevice.Verify(_ => _.Write(It.IsAny<byte[]>()), Times.Once);
        }

        private byte[] WriteSetGpioValuesResponse()
        {
            MemoryStream stream = new MemoryStream();

            stream.Write(new byte[64], 0, 64);
            stream.Position = 0;
            stream.WriteByte((byte)CommandCodes.SetGpioValues);

            return stream.ToArray();
        }

        private byte[] WriteSramWriteResponse()
        {
            MemoryStream stream = new MemoryStream();

            stream.Write(new byte[64], 0, 64);
            stream.Position = 0;
            stream.WriteByte((byte)CommandCodes.SetSram);

            return stream.ToArray();
        }

        private byte[] WriteReadGpioPortsResponse()
        {
            MemoryStream stream = new MemoryStream();

            stream.Write(new byte[64], 0, 64);
            stream.Position = 0;
            stream.WriteByte((byte)CommandCodes.GetGpioValues);
            stream.WriteByte(0); // Command OK
            stream.WriteByte(0xEE); // GPIO0 Pin Value
            stream.WriteByte(0xEF); // GPIO0 Direction Value
            stream.WriteByte(0x00); // GPIO1 Pin Value
            stream.WriteByte(0x00); // GPIO1 Direction Value
            stream.WriteByte(0x01); // GPIO2 Pin Value
            stream.WriteByte(0x01); // GPIO2 Direction Value
            stream.WriteByte(0xEE); // GPIO3 Pin Value
            stream.WriteByte(0xEF); // GPIO3 Direction Value

            return stream.ToArray();
        }

        private byte[] WriteGpSettingsResponse()
        {
            MemoryStream stream = new MemoryStream();

            stream.Write(new byte[64], 0, 64);
            stream.Position = 0;
            stream.WriteByte((byte)CommandCodes.ReadFlashData);
            stream.WriteByte(0); // Command OK
            stream.WriteByte(0); // sram structure length
            stream.WriteByte(0); // do not care
            stream.WriteByte(0x18); // GIO0
            stream.WriteByte(0x19); // GIO1
            stream.WriteByte(0x1A); // GIO2
            stream.WriteByte(0x1B); // GIO3

            return stream.ToArray();
        }

        private byte[] WriteReadSramSettingsResponse()
        {
            MemoryStream stream = new MemoryStream();

            stream.Write(new byte[64], 0, 64);
            stream.Position = 0;
            stream.WriteByte((byte)CommandCodes.GetSram);
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
            stream.WriteByte(0x18); // GIO0
            stream.WriteByte(0x19); // GIO1
            stream.WriteByte(0x1A); // GIO2
            stream.WriteByte(0x1B); // GIO3

            return stream.ToArray();
        }

        private byte[] WriteGetI2CDataResponse(CommandCodes commandCode, int length)
        {
            MemoryStream stream = new MemoryStream();

            stream.Write(new byte[64], 0, 64);
            stream.Position = 0;
            stream.WriteByte((byte)commandCode);
            stream.WriteByte(0);
            stream.WriteByte((byte)length);
            stream.Write(new byte[length], 0, length);

            return stream.ToArray();
        }

        private byte[] WriteReponse(CommandCodes commandCode)
        {
            MemoryStream stream = new MemoryStream();

            stream.Write(new byte[64], 0, 64);
            stream.Position = 0;
            stream.WriteByte((byte)commandCode);

            return stream.ToArray();
        }

        private byte[] WriteFlashUnlockResponse()
        {
            MemoryStream stream = new MemoryStream();

            stream.Write(new byte[64], 0, 64);
            stream.Position = 0;
            stream.WriteByte((byte)CommandCodes.SendFlashAccessPassword);
            stream.WriteByte(0);

            return stream.ToArray();
        }

        private byte[] WriteFlashWriteResponse(int status)
        {
            MemoryStream stream = new MemoryStream();

            stream.Write(new byte[64], 0, 64);
            stream.Position = 0;
            stream.WriteByte((byte)CommandCodes.WriteFlashData);
            stream.WriteByte((byte)status);

            return stream.ToArray();
        }

        private byte[] WriteSerialNumberResponse()
        {
            MemoryStream stream = new MemoryStream();

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

            return stream.ToArray();
        }

        private byte[] WriteStringResponse(string value)
        {
            MemoryStream stream = new MemoryStream();

            stream.Write(new byte[64], 0, 64);
            stream.Position = 0;
            stream.WriteByte((byte)CommandCodes.ReadFlashData);
            stream.WriteByte(0);
            stream.WriteByte((byte)((value.Length + 1) * 2));
            stream.WriteByte(3);
            var bytes = Encoding.Unicode.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);

            return stream.ToArray();
        }

        private byte[] WriteChipSettingsResponse()
        {
            MemoryStream stream = new MemoryStream();

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

            return stream.ToArray();
        }

        private byte[] WriteTestStatusSetParametersResponse(byte status)
        {
            MemoryStream stream = new MemoryStream();

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

            return stream.ToArray();
        }
    }
}
