namespace VotingSystem.Accessor
{
    public interface IWithSerialNumber
    {
        /// <summary>
        /// Returns true if the given serial number is already in use in the DB. Wrap in try-catch for failed connection
        /// </summary>
        /// <param name="serial">Serial number to check</param>
        /// <exception cref="MySqlException">Bad connection to DB</exception>
        public bool IsSerialInUse(string serial);

        /// <summary>
        /// Generates an unused serial number
        /// </summary>
        public string GetSerial();
    }
}
