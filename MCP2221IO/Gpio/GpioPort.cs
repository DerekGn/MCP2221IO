using System.IO;
using System.Text;

namespace MCP2221IO.Gpio
{
    /// <summary>
    /// A Gpio port
    /// </summary>
    public class GpioPort
    {
        /// <summary>
        /// Indicates if the port is configured for GPIO operation
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// The GPIO port output
        /// </summary>
        public bool Output { get; set; }

        /// <summary>
        /// The GPIO port direction
        /// </summary>
        public GpioDirection Direction { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"{nameof(Enabled)}: {Enabled} ");
            stringBuilder.Append($"{nameof(Output)}: {Output} ");
            stringBuilder.Append($"{nameof(Direction)}: {Direction}");

            return stringBuilder.ToString();
        }

        internal void Deserialise(Stream stream)
        {
            var temp = stream.ReadByte();

            Enabled = temp != 0xEE;

        }
    }
}
