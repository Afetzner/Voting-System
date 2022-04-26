using System;
using System.Data;
using MySql.Data.MySqlClient;
using VotingSystem.Model;

namespace VotingSystem.Controller
{
    public class VoterController : IDbUserController<Voter>
    {
        public bool AddUser(Voter voter)
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

                using (var cmd = new MySqlCommand("add_voter", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("v_username", voter.Username);
                    cmd.Parameters["@v_username"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_password", voter.Password);
                    cmd.Parameters["@v_password"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_firstName", voter.FirstName);
                    cmd.Parameters["@v_firstName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_lastName", voter.LastName);
                    cmd.Parameters["@v_lastName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_serialNumber", voter.SerialNumber);
                    cmd.Parameters["@v_serialNumber"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("v_collision", MySqlDbType.Byte);
                    cmd.Parameters["@v_collision"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\n" + $@"Could not execute SQL procedure 'add_voter' with parameters:
username: '{voter.Username}', 
password: '{voter.Password}', 
firstName: '{voter.FirstName}',
lastName: '{voter.LastName}', 
serialNumber: '{voter.SerialNumber}' 
(Probably a duplicate serial number)");

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

                using (var cmd = new MySqlCommand("delete_voter", conn))
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
                        Console.WriteLine(e + "\n" + $@"Could not execute SQL procedure 'remove_voter' with parameters: 
serialNumber: '{serial}'");

                        throw;
                    }
                }
            }
        }

        public Voter GetUser(string username, string? password)
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

                using (var cmd = new MySqlCommand("get_voter", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("v_username", username);
                    cmd.Parameters["@v_username"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_password", password);
                    cmd.Parameters["@v_password"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("v_firstName", MySqlDbType.VarChar);
                    cmd.Parameters["@v_firstName"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("v_lastName", MySqlDbType.VarChar);
                    cmd.Parameters["@v_lastName"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("v_serialNumber", MySqlDbType.VarChar);
                    cmd.Parameters["@v_serialNumber"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\n" + $@"\nCould not execute SQL procedure 'get_voter' with parameters:
username: '{username}', 
password: '{password}'" );

                        throw;
                    }

                    var voter = new VoterBuilder()
                        .WithUsername(username)
                        .WithPassword(password)
                        .WithFirstName(Convert.ToString(cmd.Parameters["v_firstName"].Value))
                        .WithLastName(Convert.ToString(cmd.Parameters["v_lastName"].Value))
                        .WithSerialNumber(Convert.ToString(cmd.Parameters["v_SerialNumber"].Value))
                        .Build();

                    return voter;
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

                using (var cmd = new MySqlCommand("check_voter_serial", conn))
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
                        Console.WriteLine(e + "\n" + $@"Could not execute SQL procedure 'check_voter_serial' with parameters: 
serialNumber: '{serial}'");

                        throw;
                    }

                    return Convert.ToBoolean(cmd.Parameters["v_occupied"].Value);
                }
            }
        }

    }
}