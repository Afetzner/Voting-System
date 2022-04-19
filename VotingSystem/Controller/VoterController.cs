using System;
using System.Data;
using MySql.Data.MySqlClient;
using VotingSystem.Model;

namespace VotingSystem.Controller
{
    public class VoterController : IDbController<Voter>
    {
        public int AddEntry(Voter entry)
        {

            using (var conn = new MySqlConnection(DbConnecter.ConnectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e + "Could not connect to database");
                    throw;
                }

                using (var cmd = new MySqlCommand("add_voter", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("varLastName", entry.LastName);
                    cmd.Parameters["@varLastName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("varFirstName", entry.FirstName);
                    cmd.Parameters["@varFirstName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("varSerialNumber", entry.SerialNumber);
                    cmd.Parameters["@varLicenseNumber"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("varVoterId", MySqlDbType.Int32);
                    cmd.Parameters["@varVoterId"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\nCould not execute SQL procedure 'add_voter' with parameters"
                                            + "\nLastName: " + entry.LastName
                                            + "\nFirstName: " + entry.FirstName
                                            + "\nSerialNumber: " + entry.SerialNumber);
                        throw;
                    }

                    return Convert.ToInt32(cmd.Parameters["varVoterId"].Value);
                }
            }
        }

        public void DeleteEntry(int voterId)
        {
            using (var conn = new MySqlConnection(DbConnecter.ConnectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e + "Could not connect to database");
                    throw;
                }

                using (var cmd = new MySqlCommand("delete_voter", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("varVoterId", voterId);
                    cmd.Parameters["@varVoterId"].Direction = ParameterDirection.Input;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\nCould not execute SQL procedure 'delete_voter' with parameters"
                                            + "\nVoterId: " + voterId);
                        throw;
                    }
                }
            }
        }
        
        public int GetId(Voter voter)
        {
            using (var conn = new MySqlConnection(DbConnecter.ConnectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e + "Could not connect to database");
                    throw;
                }

                using (var cmd = new MySqlCommand("get_voter_id_from_info", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("varLastName", voter.LastName);
                    cmd.Parameters["@varLastName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("varFirstName", voter.FirstName);
                    cmd.Parameters["@varFirstName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("varLicenseNumber", voter.SerialNumber);
                    cmd.Parameters["@varLicenseNumber"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("varVoterId", MySqlDbType.Int32);
                    cmd.Parameters["@varVoterId"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\nCould not execute SQL procedure 'get_voter_id_from_info' with parameters"
                                            + "\nLastName: " + voter.LastName
                                            + "\nFirstName: " + voter.FirstName
                                            + "\nLicenseNumber:" + voter.SerialNumber);
                        throw;
                    }
                    return Convert.ToInt32(cmd.Parameters["varVoterId"].Value);
                }
            }
        }

        public Voter GetInfo(int voterId)
        {
            using (var conn = new MySqlConnection(DbConnecter.ConnectionString))
            {
                try
                {
                    conn.Open();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e + "Could not connect to database");
                    throw;
                }

                using (var cmd = new MySqlCommand("get_voter_info_from_id", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("varVoterId", voterId);
                    cmd.Parameters["@varVoterId"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("varLastName", MySqlDbType.VarChar);
                    cmd.Parameters["@varLastName"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("varFirstName", MySqlDbType.VarChar);
                    cmd.Parameters["@varFirstName"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("varLicenseNumber", MySqlDbType.VarChar);
                    cmd.Parameters["@varLicenseNumber"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\nCould not execute SQL procedure 'get_voter_id_from_info' with parameters"
                                            + "\nVoterId: " + voterId);
                        throw;
                    }

                    var voter = new VoterBuilder()
                        .WithLastName(Convert.ToString(cmd.Parameters["varLastName"].Value))
                        .WithFirstName(Convert.ToString(cmd.Parameters["varFirstName"].Value))
                        .WithSerialNumber(Convert.ToString(cmd.Parameters["varLicenseNumber"].Value))
                        .Build();

                    return voter;
                }
            }
        }
    }
}