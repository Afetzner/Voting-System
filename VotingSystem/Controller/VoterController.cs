using System;
using System.Data;
using System.Data.SqlTypes;
using MySql.Data.MySqlClient;
using VotingSystem.Model;
using VotingSystem.Controller;

namespace VotingSystem.Controller
{
    public class VoterController : IDbController<Voter>
    {
        public int AddEntry(Voter entry)
        {

            using (MySqlConnection conn = new MySqlConnection(DbConnecter.ConnectionString))
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

                using (MySqlCommand cmd = new MySqlCommand("add_voter", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("varLastName", entry.LastName);
                    cmd.Parameters["@varLastName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("varFirstName", entry.FirstName);
                    cmd.Parameters["@varFirstName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("varMiddleName", entry.MiddleName);
                    cmd.Parameters["@varMiddleName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("varLicenseNumber", entry.LicenseNumber);
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
                                            + "\nMiddleName: " + entry.MiddleName
                                            + "\nLicenseNumber: " + entry.LicenseNumber);
                        throw;
                    }

                    return Convert.ToInt32(cmd.Parameters["varVoterId"].Value);
                }
            }
        }

        public void DeleteEntry(int voterId)
        {
            using (MySqlConnection conn = new MySqlConnection(DbConnecter.ConnectionString))
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

                using (MySqlCommand cmd = new MySqlCommand("delete_voter", conn))
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
            using (MySqlConnection conn = new MySqlConnection(DbConnecter.ConnectionString))
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

                using (MySqlCommand cmd = new MySqlCommand("get_voter_id_from_info", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("varLastName", voter.LastName);
                    cmd.Parameters["@varLastName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("varFirstName", voter.FirstName);
                    cmd.Parameters["@varFirstName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("varMiddleName", voter.MiddleName);
                    cmd.Parameters["@varMiddleName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("varLicenseNumber", voter.LicenseNumber);
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
                                            + "\nMiddleName: " + voter.MiddleName
                                            + "\nLicenseNumber:" + voter.LicenseNumber);
                        throw;
                    }
                    return Convert.ToInt32(cmd.Parameters["varVoterId"].Value);
                }
            }
        }

        public Voter GetInfo(int voterId)
        {
            using (MySqlConnection conn = new MySqlConnection(DbConnecter.ConnectionString))
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

                using (MySqlCommand cmd = new MySqlCommand("get_voter_info_from_id", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("varVoterId", voterId);
                    cmd.Parameters["@varVoterId"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("varLastName", MySqlDbType.VarChar);
                    cmd.Parameters["@varLastName"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("varFirstName", MySqlDbType.VarChar);
                    cmd.Parameters["@varFirstName"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("varMiddleName", MySqlDbType.VarChar);
                    cmd.Parameters["@varMiddleName"].Direction = ParameterDirection.Output;

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

                    Voter voter = new VoterBuilder()
                        .WithLastName(Convert.ToString(cmd.Parameters["varLastName"].Value))
                        .WithFirstName(Convert.ToString(cmd.Parameters["varFirstName"].Value))
                        .WithMiddleName(Convert.ToString(cmd.Parameters["varMiddleName"].Value))
                        .WithLicenseNumber(Convert.ToString(cmd.Parameters["varLicenseNumber"].Value))
                        .Build();

                    return voter;
                }
            }
        }
    }
}