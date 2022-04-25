namespace VotingSystem.Controller
{
    public interface IDbUserController <T>
    {
        private static readonly string ConnectionString = DbConnecter.ConnectionString;

        /// <summary>
        /// Adds a u to the DB. No need to manage connection
        /// </summary>
        /// <param name="user">object being added</param>
        /// <returns>id of new entry</returns>
        public bool AddUser(T user);

        /// <summary>
        /// Removes a user by serial number from the DB. No need to manage connection
        /// </summary>
        /// <param name="serial">serial number of entry to be removed</param>
        public void DeleteUser(string serial);

        /// <summary>
        /// Returns the serial number of a user, if they exists
        /// </summary>
        /// <param name="username">user's username</param>
        /// <param name="password">user's password</param>
        /// <returns>The serial number of the user</returns>
        public T GetUser(string username, string password);

        /// <summary>
        /// Returns true if the given serial number is already in use in the DB
        /// </summary>
        /// <param name="serial">Serial number to check</param>
        public bool IsSerialInUse(string serial);
    }
}