using MySql.Data.MySqlClient;
using VotingSystem.Utils;

namespace VotingSystem.Controller
{
    public interface IDbUserController <T>
    {
        private static readonly string ConnectionString = DbConnecter.ConnectionString;

        /// <summary>
        /// Adds a u to the DB. No need to manage connection, but still wrap in try-catch for failed connection
        /// </summary>
        /// <param name="user">object being added</param>
        /// <returns>id of new entry</returns>
        /// <exception cref="MySqlException">Bad connection to DB</exception>
        public bool AddUser(T user);

        /// <summary>
        /// Removes a user by serial number from the DB. No need to manage connection, but still wrap in try-catch for failed connection
        /// </summary>
        /// <param name="serial">serial number of entry to be removed</param>
        /// <exception cref="MySqlException">Bad connection to DB</exception>
        public void DeleteUser(string serial);

        /// <summary>
        /// Returns the serial number of a user, if they exists
        /// </summary>
        /// <param name="username">user's username</param>
        /// <param name="password">user's password</param>
        /// <returns>The serial number of the user</returns>
        /// <exception cref="MySqlException">Bad connection to DB</exception>
        /// <exception cref="InvalidBuilderParameterException">Corrupt data from DB</exception>
        public T GetUser(string username, string? password);

        /// <summary>
        /// Returns true if the given serial number is already in use in the DB. Wrap in try-catch for failed connection
        /// </summary>
        /// <param name="serial">Serial number to check</param>
        /// <exception cref="MySqlException">Bad connection to DB</exception>
        public bool IsSerialInUse(string serial);
    }
}