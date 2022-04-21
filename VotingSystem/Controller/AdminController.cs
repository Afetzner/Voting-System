using System;
using System.Data;
using MySql.Data.MySqlClient;
using VotingSystem.Model;

namespace VotingSystem.Controller
{
    public class AdminController : IDbUserController<Admin>
    {
        public bool AddUser(Admin admin)
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

                using (var cmd = new MySqlCommand("add_admin", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("username", admin.Username);
                    cmd.Parameters["@username"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("password", admin.Password);
                    cmd.Parameters["@password"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("serialNumber", admin.SerialNumber);
                    cmd.Parameters["@serialNumber"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("collision", MySqlDbType.Byte);
                    cmd.Parameters["@collision"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\n" + $@"Could not execute SQL procedure 'add_admin' with parameters:
username: '{admin.Username}', 
password: '{admin.Password}', 
serialNumber: '{admin.SerialNumber}' 
(Probably a duplicate serial number)");

                        throw;
                    }

                    return Convert.ToBoolean(cmd.Parameters["collision"].Value);
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

                using (var cmd = new MySqlCommand("delete_admin", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("serialNumber", serial);
                    cmd.Parameters["@serialNumber"].Direction = ParameterDirection.Input;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\n" + $@"Could not execute SQL procedure 'remove_admin' with parameters: 
serialNumber: '{serial}'");

                        throw;
                    }
                }
            }
        }

        public Admin GetUser(string username, string password)
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

                using (var cmd = new MySqlCommand("get_admin", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Parameters["@username"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("password", password);
                    cmd.Parameters["@password"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("serialNumber", MySqlDbType.VarChar);
                    cmd.Parameters["@serialNumber"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\n" + $@"\nCould not execute SQL procedure 'get_admin' with parameters:
username: '{username}', 
password: '{password}'");

                        throw;
                    }

                    var admin = new AdminBuilder()
                        .WithUsername(username)
                        .WithPassword(password)
                        .WithSerialNumber(Convert.ToString(cmd.Parameters["SerialNumber"].Value))
                        .Build();

                    return admin;
                }
            }
        }

        public bool CheckSerial(string serial)
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

                using (var cmd = new MySqlCommand("check_admin_serial", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("serialNumber", serial);
                    cmd.Parameters["@serialNumber"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("occupied", MySqlDbType.Byte);
                    cmd.Parameters["@occupied"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\n" + $@"Could not execute SQL procedure 'check_admin_serial' with parameters: 
serialNumber: '{serial}'");

                        throw;
                    }

                    return Convert.ToBoolean(cmd.Parameters["occupied"].Value);
                }
            }
        }


    }
}
