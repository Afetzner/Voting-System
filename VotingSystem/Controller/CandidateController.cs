using System;
using System.Data;
using System.Data.SqlTypes;
using MySql.Data.MySqlClient;
using VotingSystem.Model;
using VotingSystem.Controller;

namespace VotingSystem.Controller
{
    public class CandidateController : IDbController<Candidate>
    {
        public int AddEntry(Candidate entry)
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

                using (MySqlCommand cmd = new MySqlCommand("add_candidate", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("varLastName", entry.LastName);
                    cmd.Parameters["@varLastName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("varFirstName", entry.FirstName);
                    cmd.Parameters["@varFirstName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("varCandidateaId", MySqlDbType.Int32);
                    cmd.Parameters["@varCandidateId"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\nCould not execute SQL procedure 'add_candidate' with parameters"
                                            + "\nLastName: " + entry.LastName
                                            + "\nFirstName: " + entry.FirstName);
                        throw;
                    }

                    return Convert.ToInt32(cmd.Parameters["varCandidateId"].Value);
                }
            }
        }

        public void DeleteEntry(int candidateId)
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

                using (MySqlCommand cmd = new MySqlCommand("delete_candidate", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("varCandidateId", candidateId);
                    cmd.Parameters["@varCandidateId"].Direction = ParameterDirection.Input;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\nCould not execute SQL procedure 'delete_candidate' with parameters"
                                            + "\nCandidateId: " + candidateId);
                        throw;
                    }
                }
            }
        }

        public int GetId(Candidate candidate)
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

                using (MySqlCommand cmd = new MySqlCommand("get_candidate_id_from_info", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("varLastName", candidate.LastName);
                    cmd.Parameters["@varLastName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("varFirstName", candidate.FirstName);
                    cmd.Parameters["@varFirstName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("varCandidateaId", MySqlDbType.Int32);
                    cmd.Parameters["@varCandidateId"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\nCould not execute SQL procedure 'get_candidate_id_from_info' with parameters"
                                            + "\nLastName: " + candidate.LastName
                                            + "\nFirstName: " + candidate.FirstName);
                        throw;
                    }
                    return Convert.ToInt32(cmd.Parameters["varCandidateId"].Value);
                }
            }
        }

        public Candidate GetInfo(int candidateId)
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

                using (MySqlCommand cmd = new MySqlCommand("get_candidate_info_from_id", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("varCandidateId", candidateId);
                    cmd.Parameters["@varCandidateId"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("varLastName", MySqlDbType.VarChar);
                    cmd.Parameters["@varLastName"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("varFirstName", MySqlDbType.VarChar);
                    cmd.Parameters["@varFirstName"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\nCould not execute SQL procedure 'get_candidate_id_from_info' with parameters"
                                            + "\nCandidateId: " + candidateId);
                        throw;
                    }

                    Cnadidate candidate = new CandidateBuilder()
                        .WithLastName(Convert.ToString(cmd.Parameters["varLastName"].Value))
                        .WithFirstName(Convert.ToString(cmd.Parameters["varFirstName"].Value))
                        .Build();

                    return candidate;
                }
            }
        }
    }
}