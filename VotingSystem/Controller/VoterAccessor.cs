using System;
using System.Data;
using MySql.Data.MySqlClient;
using VotingSystem.Model;
using VotingSystem.Controller;

namespace VotingSystem.Controller
{
    public class VoterAccessor
    {
        public static int AddVoter(Voter entry)
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
                    cmd.Parameters.AddWithValue("LastName", entry.LastName);
                    cmd.Parameters["@LastName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("FirstName", entry.FirstName);
                    cmd.Parameters["@FirstName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("MiddleName", entry.MiddleName);
                    cmd.Parameters["@MiddleName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("LicenseNumber", entry.LicenseNumber);
                    cmd.Parameters["@LicenseNumber"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("VoterId", MySqlDbType.Int32);
                    cmd.Parameters["@VoterId"].Direction = ParameterDirection.Output;

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

                    return Convert.ToInt32(cmd.Parameters["VoterId"].Value);
                }
            }
        }

        public void RemoveVoter(int voterId)
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

                using (MySqlCommand cmd = new MySqlCommand("remove_voter", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("VoterId", voterId);
                    cmd.Parameters["@VoterId"].Direction = ParameterDirection.Input;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\nCould not execute SQL procedure 'remove_voter' with parameters"
                                            + "\nVoterId: " + voterId);
                        throw;
                    }

                }
            }
        }
        
        public int GetEntryId(Object args)
        {
            throw new NotImplementedException();
        }

        public Object GetEntryInfo(int id)
        {
            throw new NotImplementedException();
        }
    }
}