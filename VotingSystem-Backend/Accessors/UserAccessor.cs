using VotingSystem.Model;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using MySql.Data.MySqlClient;
using VotingSystem.Controller;

namespace VotingSystem.Accessor
{
    public class UserDbAccessor <T> : IUserAccessor <T> where T : IUser
    {
        
        public bool AddUser(IUser user)
        {

            using (var conn = new MySqlConnection(DbConnecter.ConnectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e + "\nCould not connect to database");
                    throw;
                }

                using (var cmd = new MySqlCommand("add_user", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("v_username", user.Username);
                    cmd.Parameters["@v_username"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_password", user.Password);
                    cmd.Parameters["@v_password"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_email", user.Email);
                    cmd.Parameters["@v_email"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_firstName", user.FirstName);
                    cmd.Parameters["@v_firstName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_lastName", user.LastName);
                    cmd.Parameters["@v_lastName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_serialNumber", user.SerialNumber);
                    cmd.Parameters["@v_serialNumber"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_isAdmin", user.IsAdmin);
                    cmd.Parameters["@v_isAdmin"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("v_collision", MySqlDbType.Byte);
                    cmd.Parameters["@v_collision"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e) when (e.ErrorCode == -2147467259)
                    {
                        //duplicate key, don't throw, handled by return variable
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e.ErrorCode);
                        Console.WriteLine(e + "\n\n" 
                            + $@"Could not execute SQL procedure 'add_user' with parameters:
    username: '{user.Username}', 
    password: '{user.Password}', 
    email: '{user.Email}',
    serialNumber: '{user.SerialNumber}' ,
    firstName: '{user.FirstName}',
    lastName: '{user.LastName}'
    isAdmin: '{user.IsAdmin}'");

                        throw;
                    }

                    return Convert.ToBoolean(cmd.Parameters["v_collision"].Value);
                }
            }
        }

        public void DeleteUser(string serial)
        {
            using (var conn = new MySqlConnection(DbConnecter.ConnectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e + "\nCould not connect to database");
                    throw;
                }

                using (var cmd = new MySqlCommand("delete_user", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("v_serialNumber", serial);
                    cmd.Parameters["@v_serialNumber"].Direction = ParameterDirection.Input;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\n" + $@"Could not execute SQL procedure 'remove_user' with parameters: 
    serialNumber: '{serial}'");

                        throw;
                    }
                }
            }
        }

        public T? GetUser(string username, string password)
        {
            using (var conn = new MySqlConnection(DbConnecter.ConnectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e + "\nCould not connect to database");
                    throw;
                }

                using (var cmd = new MySqlCommand("get_user", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("v_username", username);
                    cmd.Parameters["@v_username"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_password", password);
                    cmd.Parameters["@v_password"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("v_email", MySqlDbType.VarChar);
                    cmd.Parameters["@v_email"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("v_serialNumber", MySqlDbType.VarChar);
                    cmd.Parameters["@v_serialNumber"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("v_firstName", MySqlDbType.VarChar);
                    cmd.Parameters["@v_firstName"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("v_lastName", MySqlDbType.VarChar);
                    cmd.Parameters["@v_lastName"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("v_isAdmin", MySqlDbType.Byte);
                    cmd.Parameters["@v_isAdmin"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("v_isNull", MySqlDbType.Byte);
                    cmd.Parameters["@v_isNull"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\n" + $@"\nCould not execute SQL procedure 'get_user' with parameters:
    username: '{username}', 
    password: '{password}'");

                        throw;
                    }

                    if (Convert.ToBoolean(cmd.Parameters["v_isNull"].Value))
                        return default;

                    T user;
                    bool isAdmin = Convert.ToBoolean(cmd.Parameters["@v_isAdmin"].Value);
                    // TODO Return null is user does not exist
                    if (isAdmin)
                    {
                        Admin admin = new Admin.AdminBuilder()
                            .WithUsername(username)
                            .WithPassword(password)
                            .WithEmail(Convert.ToString(cmd.Parameters["v_email"].Value))
                            .WithSerialNumber(Convert.ToString(cmd.Parameters["v_serialNumber"].Value))
                            .WithFirstName(Convert.ToString(cmd.Parameters["v_firstName"].Value))
                            .WithLastName(Convert.ToString(cmd.Parameters["v_lastName"].Value))
                            .Build();
                        user = (T)(IUser)admin;
                    }
                    else
                    {
                        Voter voter = new Voter.VoterBuilder()
                            .WithUsername(username)
                            .WithPassword(password)
                            .WithEmail(Convert.ToString(cmd.Parameters["v_email"].Value))
                            .WithSerialNumber(Convert.ToString(cmd.Parameters["v_serialNumber"].Value))
                            .WithFirstName(Convert.ToString(cmd.Parameters["v_firstName"].Value))
                            .WithLastName(Convert.ToString(cmd.Parameters["v_lastName"].Value))
                            .Build();
                        user = (T)(IUser)voter;
                    }
                    return user;
                }
            }
        }

        public bool IsSerialInUse(string serial)
        {
            using (var conn = new MySqlConnection(DbConnecter.ConnectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e + "\nCould not connect to database");
                    throw;
                }

                using (var cmd = new MySqlCommand("check_user_serial", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("v_serialNumber", serial);
                    cmd.Parameters["@v_serialNumber"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("v_occupied", MySqlDbType.Byte);
                    cmd.Parameters["@v_occupied"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\n" + $@"Could not execute SQL procedure 'check_user_serial' with parameters: 
    serialNumber: '{serial}'");

                        throw;
                    }

                    return Convert.ToBoolean(cmd.Parameters["v_occupied"].Value);
                }
            }
        }

        public bool IsUsernameInUse(string username)
        {
            using (var conn = new MySqlConnection(DbConnecter.ConnectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e + "\nCould not connect to database");
                    throw;
                }

                using (var cmd = new MySqlCommand("check_username", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("v_username", username);
                    cmd.Parameters["@v_username"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("v_occupied", MySqlDbType.Byte);
                    cmd.Parameters["@v_occupied"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\n" + $@"Could not execute SQL procedure 'check_username' with parameters: 
    username: '{username}'");

                        throw;
                    }

                    return Convert.ToBoolean(cmd.Parameters["v_occupied"].Value);
                }
            }
        }

        public String GenerateUserSerialNo()
        {
            GenerateSerialNo user = new GenerateSerialNo();
            String uniqueNo = user.generateSerialNo();
            do
            {
                uniqueNo = user.generateSerialNo();
            } while (IsSerialInUse(uniqueNo) == true);

            return uniqueNo;
        }
    }
}