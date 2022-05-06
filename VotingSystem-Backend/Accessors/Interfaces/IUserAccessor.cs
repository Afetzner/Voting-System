using MySql.Data.MySqlClient;
using VotingSystem.Utils;
using VotingSystem.Model;

namespace VotingSystem.Accessor
{
    public interface IUserAccessor <T> : IWithSerialNumber where T : IUser
    {

        /// <summary>
        /// Adds a user to the DB. No need to manage connection, but still wrap in try-catch for failed connection
        /// </summary>
        /// <param name="user">object being added</param>
        /// <returns>id of new entry</returns>
        /// <exception cref="MySqlException">Bad connection to DB</exception>
        public bool AddUser(IUser user);

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
        public T? GetUser(string username, string password);

        /// <summary>
        /// Returns if the given username is already in use in the DB. Wrap in try-catch for failed connection
        /// </summary>
        /// <param name="serial">Serial number to check</param>
        /// <exception cref="MySqlException">Bad connection to DB</exception>
        public bool IsUsernameInUse(string username);

        /// <summary>
        /// Returns the serial number of a user, if they exists
        /// </summary>
        /// <param name="email">user's email</param>
        /// <param name="password">user's password</param>
        /// <returns>The serial number of the user</returns>
        /// <exception cref="MySqlException">Bad connection to DB</exception>
        /// <exception cref="InvalidBuilderParameterException">Corrupt data from DB</exception>
        public T? GetUserByEmail(string email, string password);
    }
}