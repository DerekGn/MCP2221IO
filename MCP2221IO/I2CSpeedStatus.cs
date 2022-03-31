namespace MCP2221IO
{
    /// <summary>
    /// The I2C speed change state
    /// </summary>
    public enum I2CSpeedStatus
    {
        /// <summary>
        /// No Set I2C/SMBus communication speed was issued
        /// </summary>
        NotIssued = 0x00,
        /// <summary>
        /// The new I2C/SMBus communication speed is now considered.
        /// </summary>
        Set = 0x20,
        /// <summary>
        /// The I 2 C/SMBus communication speed was not set (e.g., I2C transfer in progress)
        /// </summary>
        NotSet = 0x21,
    }
}
