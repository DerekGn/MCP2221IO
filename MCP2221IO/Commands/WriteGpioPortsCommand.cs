
using MCP2221IO.Gpio;
using System;
using System.IO;

namespace MCP2221IO.Commands
{
    internal class WriteGpioPortsCommand : BaseCommand
    {
        public WriteGpioPortsCommand(
            GpioPort gpioPort0,
            GpioPort gpioPort1,
            GpioPort gpioPort2,
            GpioPort gpioPort3) 
            : base(CommandCodes.SetGpioValues)
        {
            GpioPort0 = gpioPort0 ?? throw new ArgumentNullException(nameof(gpioPort0));
            GpioPort1 = gpioPort1 ?? throw new ArgumentNullException(nameof(gpioPort1));
            GpioPort2 = gpioPort2 ?? throw new ArgumentNullException(nameof(gpioPort2));
            GpioPort3 = gpioPort3 ?? throw new ArgumentNullException(nameof(gpioPort3));
        }

        public GpioPort GpioPort0 { get; }
        public GpioPort GpioPort1 { get; }
        public GpioPort GpioPort2 { get; }
        public GpioPort GpioPort3 { get; }

        public override void Serialise(Stream stream)
        {
            base.Serialise(stream);

            GpioPort0.Serialize(stream);
            GpioPort1.Serialize(stream);
            GpioPort2.Serialize(stream);
            GpioPort3.Serialize(stream);
        }
    }
}
