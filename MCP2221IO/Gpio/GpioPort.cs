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
        /// The GPIO port value
        /// </summary>
        public bool Value { get; set; }

        /// <summary>
        /// The GPIO port direction
        /// </summary>
        public bool IsInput { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"{nameof(Enabled)}: {Enabled} ");
            stringBuilder.Append($"{nameof(Value)}: {Value} ");
            stringBuilder.Append($"{nameof(IsInput)}: {IsInput}");

            return stringBuilder.ToString();
        }

        internal void Deserialize(Stream stream)
        {
            Enabled = stream.ReadByte() != 0xEE;
            IsInput = stream.ReadByte() == 0x00;
            Value = stream.ReadByte() == 0x01;
        }

        internal void Serialize(Stream stream)
        {
            if(!Enabled)
            {
                stream.Write(new byte[] { 0,0,0,0});
            }
            else
            {
                stream.WriteByte(0xFF);
                stream.WriteByte((byte)(Value ? 1 : 0));

                stream.WriteByte(0xFF);
                stream.WriteByte((byte)(IsInput ? 1 : 0));
            }
        }
    }
}
