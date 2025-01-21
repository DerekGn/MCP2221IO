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

using MCP2221IO.Commands;
using MCP2221IO.Exceptions;
using MCP2221IO.Gp;
using MCP2221IO.Gpio;
using MCP2221IO.Settings;
using MCP2221IO.Usb;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace MCP2221IO.UnitTests
{
    public class DeviceTests
    {
        private readonly Device _device;
        private readonly Mock<IHidDevice> _mockHidDevice;
        private readonly ITestOutputHelper _output;

        public DeviceTests(ITestOutputHelper output)
        {
            _mockHidDevice = new Mock<IHidDevice>();
            _device = new Device(Mock.Of<ILogger<Device>>(), _mockHidDevice.Object);
            _output = output;
        }

        [Fact]
        public void TestCancelI2cBusTransfer()
        {
            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(TestPayloads.DeviceStatusResponse);

            // Act
            _device.CancelI2cBusTransfer();

            // Assert
            Assert.NotNull(_device.Status);
        }

        [Fact]
        public void TestGetFactorySerialNumberOk()
        {
            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(TestPayloads.FactorySerialNumberResponse);

            // Act
            string serialNumber = _device.FactorySerialNumber!;

            // Assert
            Assert.NotNull(serialNumber);

            _output.WriteLine(serialNumber);
        }

        [Fact]
        public void TestGetUsbManufacturerDescriptorOk()
        {
            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(TestPayloads.ManufacturerDescriptorResponse);

            // Act
            string descriptor = _device.UsbManufacturerDescriptor;

            // Assert
            Assert.NotNull(descriptor);

            _output.WriteLine(descriptor);
        }

        [Fact]
        public void TestGetUsbProductDescriptorOk()
        {
            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(TestPayloads.ProductDescriptorResponse);

            // Act
            string descriptor = _device.UsbProductDescriptor;

            // Assert
           Assert.NotNull(descriptor);

            _output.WriteLine(descriptor);
        }

        [Fact]
        public void TestGetUsbSerialNumberDescriptorOk()
        {
            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(TestPayloads.SerialNumberDescriptorResponse);

            // Act
            string descriptor = _device.UsbSerialNumberDescriptor;

            // Assert
           Assert.NotNull(descriptor);

            _output.WriteLine(descriptor);
        }

        [Fact]
        public void TestI2cRead()
        {
            const int DataLength = 130;
            int blockCount = 0;

            // Arrange
            _mockHidDevice.SetupSequence(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.ReadI2cData))
                .Returns(WriteGetI2cDataResponse(CommandCodes.GetI2cData, Math.Min(Device.MaxBlockSize, Math.Abs(DataLength - (blockCount++ * Device.MaxBlockSize)))))
                .Returns(WriteGetI2cDataResponse(CommandCodes.GetI2cData, Math.Min(Device.MaxBlockSize, Math.Abs(DataLength - (blockCount++ * Device.MaxBlockSize)))))
                .Returns(WriteGetI2cDataResponse(CommandCodes.GetI2cData, Math.Min(Device.MaxBlockSize, Math.Abs(DataLength - (blockCount * Device.MaxBlockSize)))));

            // Act
            var buffer = _device.I2cReadData(new I2cAddress(0x0A), DataLength);

            // Assert
            _mockHidDevice.Verify(_ => _.WriteRead(It.IsAny<byte[]>()), Times.Exactly(4));
            Assert.NotNull(buffer);
            Assert.NotEmpty(buffer);
        }

        [Fact]
        [SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "<Pending>")]
        public void TestI2cReadAddressNull()
        {
            // Arrange

            // Act
            Action act = () => { var buffer = _device.I2cReadData(null, 1); };

            // Assert
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void TestI2cReadRepeatedStart()
        {
            const int DataLength = 130;
            int blockCount = 0;

            // Arrange
            _mockHidDevice.SetupSequence(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.ReadI2cDataRepeatedStart))
                .Returns(WriteGetI2cDataResponse(CommandCodes.GetI2cData, Math.Min(Device.MaxBlockSize, Math.Abs(DataLength - (blockCount++ * Device.MaxBlockSize)))))
                .Returns(WriteGetI2cDataResponse(CommandCodes.GetI2cData, Math.Min(Device.MaxBlockSize, Math.Abs(DataLength - (blockCount++ * Device.MaxBlockSize)))))
                .Returns(WriteGetI2cDataResponse(CommandCodes.GetI2cData, Math.Min(Device.MaxBlockSize, Math.Abs(DataLength - (blockCount * Device.MaxBlockSize)))));

            // Act
            var buffer = _device.I2cReadDataRepeatedStart(new I2cAddress(0x0A), DataLength);

            // Assert
            _mockHidDevice.Verify(_ => _.WriteRead(It.IsAny<byte[]>()), Times.Exactly(4));

            Assert.NotNull(buffer);
            Assert.NotEmpty(buffer);
        }

        [Fact]
        public void TestI2cScanBusSevenBitAddress()
        {
            // Arrange
            _mockHidDevice.SetupSequence(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.ReadI2cData))
                .Returns(WriteGetI2cDataResponse(CommandCodes.GetI2cData, 1));

            // Act
            var result = _device.I2cScanBusInternal(false, 9);

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void TestI2cScanBusTenBitAddress()
        {
            // Arrange
            _mockHidDevice.SetupSequence(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.ReadI2cData))
                .Returns(WriteGetI2cDataResponse(CommandCodes.GetI2cData, 1));

            // Act
            var result = _device.I2cScanBusInternal(true, 9);

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void TestI2cWriteData()
        {
            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.WriteI2cData));

            var buffer = new List<byte>() { };

            for (int i = 0; i < 1000; i++)
            {
                buffer.Add(0xFF);
            }

            // Act
            _device.I2cWriteData(new I2cAddress(0x0A), buffer);

            // Assert
            _mockHidDevice.Verify(_ => _.WriteRead(It.IsAny<byte[]>()), Times.Exactly(17));
        }

        [Fact]
        public void TestI2cWriteDataAddressNull()
        {
            // Arrange

            // Act
            Action act = () => { _device.I2cWriteData(null, new List<byte>()); };

            // Assert
            Assert.Throws<ArgumentNullException>(() => act());
        }

        [Fact]
        public void TestI2cWriteDataDataNull()
        {
            // Arrange

            // Act
            Action act = () => { _device.I2cWriteData(new I2cAddress(0x0A), null); };

            // Assert
            Assert.Throws<ArgumentNullException>(() => act());
        }

        [Fact]
        public void TestI2cWriteDataDataSize()
        {
            // Arrange

            // Act
            Action act = () => { _device.I2cWriteData(new I2cAddress(0x0A), new byte[0xFFFFF]); };

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => act());
        }

        [Fact]
        public void TestI2cWriteDataNoStop()
        {
            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.WriteI2cDataNoStop));

            var buffer = new List<byte>() { };

            for (int i = 0; i < 1000; i++)
            {
                buffer.Add(0xFF);
            }

            // Act
            _device.I2cWriteDataNoStop(new I2cAddress(0x0A), buffer);

            // Assert
            _mockHidDevice.Verify(_ => _.WriteRead(It.IsAny<byte[]>()), Times.Exactly(17));
        }

        [Fact]
        public void TestI2cWriteDataTestRepeatStart()
        {
            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.WriteI2cDataRepeatedStart));

            var buffer = new List<byte>() { };

            for (int i = 0; i < 1000; i++)
            {
                buffer.Add(0xFF);
            }

            // Act
            _device.I2cWriteDataRepeatStart(new I2cAddress(0x0A), buffer);

            // Assert
            _mockHidDevice.Verify(_ => _.WriteRead(It.IsAny<byte[]>()), Times.Exactly(17));
        }

        [Fact]
        public void TestOpen()
        {
            // Arrange

            // Act
            _device.Open();

            // Assert
            _mockHidDevice.Verify(_ => _.Open(), Times.Once);
        }

        [Fact]
        public void TestReadChipSettings()
        {
            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(TestPayloads.ChipSettingsResponse);

            // Act
            _device.ReadChipSettings();

            // Assert
            Assert.NotNull(_device.ChipSettings);

            _output.WriteLine(_device.ChipSettings!.ToString());
        }

        [Fact]
        public void TestReadDeviceStatus()
        {
            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(TestPayloads.DeviceStatusResponse);

            // Act
            _device.ReadDeviceStatus();

            // Assert
            Assert.NotNull(_device.Status);

            _output.WriteLine(_device.Status!.ToString());
        }

        [Fact]
        public void TestReadGpioSettings()
        {
            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(TestPayloads.GpioSettingsResponse);

            // Act
            _device.ReadGpioPorts();

            // Assert
            _mockHidDevice.Verify(_ => _.WriteRead(It.IsAny<byte[]>()), Times.Once);

            _output.WriteLine($"GpioPort0: {_device.GpioPort0}");
            _output.WriteLine($"GpioPort1: {_device.GpioPort1}");
            _output.WriteLine($"GpioPort2: {_device.GpioPort2}");
            _output.WriteLine($"GpioPort3: {_device.GpioPort3}");
        }

        [Fact]
        public void TestReadGpSettings()
        {
            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(TestPayloads.GpSettingsResponse);

            // Act
            _device.ReadGpSettings();

            // Assert
            Assert.NotNull(_device.GpSettings);

            _output.WriteLine(_device.GpSettings!.ToString());
        }

        [Fact]
        public void TestReadSramSettings()
        {
            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(TestPayloads.ReadSramSettingsResponse);

            // Act
            _device.ReadSramSettings();

            // Assert
            Assert.NotNull(_device.SramSettings);

            _output.WriteLine(_device.SramSettings!.ToString());
        }

        [Fact]
        public void TestReset()
        {
            // Arrange

            // Act
            _device.Reset();

            // Assert
            _mockHidDevice.Verify(_ => _.Write(It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public void TestSetI2cBusSpeed()
        {
            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(TestPayloads.DeviceStatusResponse);

            // Act
            _device.SetI2cBusSpeed(100);

            // Assert
            Assert.NotNull( _device.Status);
        }

        [Fact]
        public void TestSetI2cBusSpeedTimeout()
        {
            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(TestPayloads.DeviceStatusSetSpeedFailedResponse);

            // Act
            Action act = () => { _device.SetI2cBusSpeed(100); };

            // Assert
            Assert.Throws<I2cOperationException>(() => act());
        }

        [Fact]
        public void TestSetUsbManufacturerDescriptorOk()
        {
            // Arrange
            byte[]? descriptor = null;

            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteFlashWriteResponse(0))
                .Callback<byte[]>(b => descriptor = b);

            // Act
            _device.UsbManufacturerDescriptor = "UPDATED";

            // Assert
            Assert.NotNull(descriptor);
        }

        [Fact]
        public void TestSetUsbProductDescriptorOk()
        {
            // Arrange
            byte[]? descriptor = null;

            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteFlashWriteResponse(0))
                .Callback<byte[]>(b => descriptor = b);

            // Act
            _device.UsbProductDescriptor = "UPDATED";

            // Assert
            Assert.NotNull(descriptor);
        }

        [Fact]
        public void TestSetUsbSerialNumberDescriptorOk()
        {
            // Arrange
            byte[]? descriptor = null;

            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteFlashWriteResponse(0))
                .Callback<byte[]>(b => descriptor = b);

            // Act
            _device.UsbSerialNumberDescriptor = "SERIAL NUMBER";

            // Assert
           Assert.NotNull(descriptor);
        }

        [Fact]
        public void TestSmBusBlockRead()
        {
            // Arrange
            _mockHidDevice.SetupSequence(_ => _.WriteRead(It.IsAny<byte[]>()))
               .Returns(WriteReponse(CommandCodes.WriteI2cDataNoStop))
               .Returns(WriteGetI2cDataResponse(CommandCodes.ReadI2cDataRepeatedStart, 5))
               .Returns(WriteGetI2cDataResponse(CommandCodes.GetI2cData, 0x01, 0x02, 0x03, 0x04, 0x5, 0xBC));

            // Act
            var result = _device.SmBusBlockRead(new I2cAddress(0x0B), 0x55, 5, true);

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void TestSmBusBlockWrite()
        {
            List<byte> writeData = new List<byte>();

            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.WriteI2cData))
                .Callback((byte[] b) =>
                {
                    writeData.AddRange(b);
                });

            // Act
            _device.SmBusBlockWrite(new I2cAddress(0x0B), 0xAA, new List<byte>() { 0x01, 0x02, 0x45 }, true);

            // Assert
            var expected = new List<byte>() { 0x16, 0xAA, 0x03, 0x01, 0x02, 0x45, 0x89 };
            Assert.Equal(expected.Count, writeData.Intersect(expected).Count());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestSmBusQuickCommand(bool write)
        {
            // Arrange
            if (write)
            {
                _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                    .Returns(WriteReponse(CommandCodes.WriteI2cData));
            }
            else
            {
                _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                    .Returns(WriteReponse(CommandCodes.ReadI2cData));
            }

            // Act
            _device.SmBusQuickCommand(new I2cAddress(0x10, I2cAddressSize.SevenBit), write);

            // Assert
            _mockHidDevice.Verify(_ => _.WriteRead(It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public void TestSmBusReadByte()
        {
            // Arrange
            _mockHidDevice.SetupSequence(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.ReadI2cData))
                .Returns(WriteGetI2cDataResponse(CommandCodes.GetI2cData, 1));

            // Act
            var result = _device.SmBusReadByte(new I2cAddress(10, I2cAddressSize.SevenBit), true);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void TestSmBusReadByteCommand()
        {
            // Arrange
            _mockHidDevice.SetupSequence(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.WriteI2cDataNoStop))
                .Returns(WriteGetI2cDataResponse(CommandCodes.ReadI2cDataRepeatedStart, sizeof(byte) + 1))
                .Returns(WriteGetI2cDataResponse(CommandCodes.GetI2cData, 0x55, 0xAC));

            // Act
            var result = _device.SmBusReadByteCommand(new I2cAddress(10, I2cAddressSize.SevenBit), 0x55, true);

            // Assert
            Assert.Equal(0x55, result);
        }

        [Fact]
        public void TestSmBusReadIntCommand()
        {
            // Arrange
            _mockHidDevice.SetupSequence(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.WriteI2cDataNoStop))
                .Returns(WriteGetI2cDataResponse(CommandCodes.ReadI2cDataRepeatedStart, sizeof(int) + 1))
                .Returns(WriteGetI2cDataResponse(CommandCodes.GetI2cData, 0x55, 0x55, 0x55, 0x55, 0xB7));

            // Act
            var result = _device.SmBusReadIntCommand(new I2cAddress(10, I2cAddressSize.SevenBit), 0x55, true);

            // Assert
            Assert.Equal(0x55555555, result);
        }

        [Fact]
        public void TestSmBusReadLongCommand()
        {
            // Arrange
            _mockHidDevice.SetupSequence(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.WriteI2cDataNoStop))
                .Returns(WriteGetI2cDataResponse(CommandCodes.ReadI2cDataRepeatedStart, sizeof(long) + 1))
                .Returns(WriteGetI2cDataResponse(CommandCodes.GetI2cData, 0x55, 0x55, 0x55, 0x55, 0xAA, 0xAA, 0xAA, 0xAA, 0x93));

            // Act
            var result = _device.SmBusReadLongCommand(new I2cAddress(10, I2cAddressSize.SevenBit), 0x55, true);

            // Assert
            Assert.Equal(-6148914692668172971L, result);
        }

        [Fact]
        public void TestSmBusReadWordCommand()
        {
            // Arrange
            _mockHidDevice.SetupSequence(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.WriteI2cDataNoStop))
                .Returns(WriteGetI2cDataResponse(CommandCodes.ReadI2cDataRepeatedStart, sizeof(short) + 1))
                .Returns(WriteGetI2cDataResponse(CommandCodes.GetI2cData, 0x55, 0xAA, 0x12));

            // Act
            var result = _device.SmBusReadShortCommand(new I2cAddress(10, I2cAddressSize.SevenBit), 0x55, true);

            // Assert
            Assert.Equal(-21931, result);
        }

        [Fact]
        public void TestSmBusWriteByte()
        {
            List<byte> writeData = new List<byte>();

            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.WriteI2cData))
                .Callback((byte[] b) =>
                {
                    writeData.AddRange(b);
                });

            // Act
            _device.SmBusWriteByte(new I2cAddress(10, I2cAddressSize.SevenBit), 0x55, true);

            // Assert
            var expected = new List<byte>() { 0x14, 0x55, 0xAC };
            Assert.Equal(expected.Count, writeData.Intersect(expected).Count());
        }

        [Fact]
        public void TestSmBusWriteByteCommand()
        {
            List<byte> writeData = new List<byte>();

            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.WriteI2cData))
                .Callback((byte[] b) =>
                {
                    writeData.AddRange(b);
                });

            // Act
            _device.SmBusWriteByteCommand(new I2cAddress(10, I2cAddressSize.SevenBit), 0x55, 0xAA, true);

            // Assert
            var expected = new List<byte>() { 0x14, 0x55, 0xAA, 0x12 };
            Assert.Equal(expected.Count, writeData.Intersect(expected).Count());
        }

        [Fact]
        public void TestSmBusWriteIntCommand()
        {
            List<byte> writeData = new List<byte>();

            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.WriteI2cData))
                .Callback((byte[] b) =>
                {
                    writeData.AddRange(b);
                });

            // Act
            _device.SmBusWriteIntCommand(new I2cAddress(10, I2cAddressSize.SevenBit), 0x55, 0xFEED, true);

            // Assert
            var expected = new List<byte>() { 0x14, 0x55, 0xED, 0xFE, 0x00 };
            Assert.Equal(expected.Count, writeData.Intersect(expected).Count());
        }

        [Fact]
        public void TestSmBusWriteLongCommand()
        {
            List<byte> writeData = new List<byte>();

            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.WriteI2cData))
                .Callback((byte[] b) =>
                {
                    writeData.AddRange(b);
                });

            // Act
            _device.SmBusWriteLongCommand(new I2cAddress(10, I2cAddressSize.SevenBit), 0x55, 0xFEED, true);

            // Assert
            var expected = new List<byte>() { 0x14, 0x55, 0xED, 0xFE, 0x00 };
            Assert.Equal(expected.Count, writeData.Intersect(expected).Count());
        }

        [Fact]
        public void TestSmBusWriteShortCommand()
        {
            List<byte> writeData = new List<byte>();

            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteReponse(CommandCodes.WriteI2cData))
                .Callback((byte[] b) =>
                {
                    writeData.AddRange(b);
                });

            // Act
            _device.SmBusWriteShortCommand(new I2cAddress(10, I2cAddressSize.SevenBit), 0x55, 2000, true);

            // Assert
            var expected = new List<byte>() { 0x14, 0x55, 0xD0, 0x07, 0x4B };
            Assert.Equal(expected.Count, writeData.Intersect(expected).Count());
        }

        [Fact]
        public void TestUnlockFlash()
        {
            // Arrange

            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteFlashUnlockResponse());

            // Act
            var exception = Record.Exception(() => _device.UnlockFlash(new Password("AA55AA55DEADBEEF")));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void TestWriteChipSettings()
        {
            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteFlashWriteResponse(0));

            _device.ChipSettings = new ChipSettings();

            // Act
            _device.WriteChipSettings(new Password(new List<byte>()));

            // Assert
            _mockHidDevice.Verify(_ => _.WriteRead(It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public void TestWriteGpSettings()
        {
            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteFlashWriteResponse(0));

            _device.GpSettings = new GpSettings()
            {
                Gp0PowerUpSetting = new GpSetting<Gp0Designation>(),
                Gp1PowerUpSetting = new GpSetting<Gp1Designation>(),
                Gp2PowerUpSetting = new GpSetting<Gp2Designation>(),
                Gp3PowerUpSetting = new GpSetting<Gp3Designation>()
            };

            // Act
            _device.WriteGpSettings();

            // Assert
            _mockHidDevice.Verify(_ => _.WriteRead(It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public void TestWriteSramSettings()
        {
            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(TestPayloads.WriteSramSettingsResponse);

            _device.SramSettings = new SramSettings()
            {
                Gp0Settings = new GpSetting<Gp0Designation>(),
                Gp1Settings = new GpSetting<Gp1Designation>(),
                Gp2Settings = new GpSetting<Gp2Designation>(),
                Gp3Settings = new GpSetting<Gp3Designation>()
            };

            // Act
            _device.WriteSramSettings(true);

            // Assert
            _mockHidDevice.Verify(_ => _.WriteRead(It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public void WriteGpioPorts()
        {
            // Arrange
            _mockHidDevice.Setup(_ => _.WriteRead(It.IsAny<byte[]>()))
                .Returns(WriteSetGpioValuesResponse());

            _device._gpioPortsRead = true;
            _device.GpioPort0 = new GpioPort();
            _device.GpioPort1 = new GpioPort();
            _device.GpioPort2 = new GpioPort();
            _device.GpioPort3 = new GpioPort();
            // Act
            _device.WriteGpioPorts();

            // Assert
            _mockHidDevice.Verify(_ => _.WriteRead(It.IsAny<byte[]>()), Times.Once);
        }

        private static MemoryStream GetStream()
        {
            MemoryStream stream = new MemoryStream();

            stream.Write(new byte[65], 0, 65);
            stream.Position = 0;
            stream.WriteByte(0);
            return stream;
        }

        private static byte[] WriteFlashUnlockResponse()
        {
            MemoryStream stream = GetStream();
            stream.WriteByte((byte)CommandCodes.SendFlashAccessPassword);
            stream.WriteByte(0);

            return stream.ToArray();
        }

        private static byte[] WriteFlashWriteResponse(int status)
        {
            MemoryStream stream = GetStream();
            stream.WriteByte((byte)CommandCodes.WriteFlashData);
            stream.WriteByte((byte)status);

            return stream.ToArray();
        }

        private static byte[] WriteGetI2cDataResponse(CommandCodes commandCode, params byte[] data)
        {
            MemoryStream stream = GetStream();
            stream.WriteByte((byte)commandCode);
            stream.WriteByte(0);
            stream.WriteByte(0);
            stream.WriteByte((byte)data.Length);
            stream.Write(data);

            return stream.ToArray();
        }

        private static byte[] WriteGetI2cDataResponse(CommandCodes commandCode, int length)
        {
            MemoryStream stream = GetStream();
            stream.WriteByte((byte)commandCode);
            stream.WriteByte(0);
            stream.WriteByte(0);
            stream.WriteByte((byte)length);
            stream.Write(new byte[length], 0, length);

            return stream.ToArray();
        }

        private static byte[] WriteReponse(CommandCodes commandCode)
        {
            MemoryStream stream = GetStream();
            stream.WriteByte((byte)commandCode);

            return stream.ToArray();
        }

        private static byte[] WriteSetGpioValuesResponse()
        {
            MemoryStream stream = GetStream();
            stream.WriteByte((byte)CommandCodes.SetGpioValues);

            return stream.ToArray();
        }
    }
}