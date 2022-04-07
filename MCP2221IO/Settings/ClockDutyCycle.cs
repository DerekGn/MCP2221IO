
namespace MCP2221IO.Settings
{
    /// <summary>
    /// The clock duty cycle
    /// </summary>
    public enum ClockDutyCycle
    {
        /// <summary>
        /// Duty cycle 75% (75% of one clock period is logic ‘1’ and 25% of one clock period is logic ‘0’)
        /// </summary>
        DutyCycle75 = 0b11,
        /// <summary>
        /// Duty cycle 50% (50% of one clock period is logic ‘ 1’ and 50% of one clock period is logic ‘0’) (factory default)
        /// </summary>
        DutyCycle50 = 0b10,
        /// <summary>
        /// Duty cycle 25% (25% of one clock period is logic ‘1’ and 75% of one clock period is logic ‘0’)
        /// </summary>
        DutyCycle25 = 0b01,
        /// <summary>
        /// Duty cycle 0% (100% of one clock period is logic ‘0’)
        /// </summary>
        DutyCycle0 = 0b00
    }
}
