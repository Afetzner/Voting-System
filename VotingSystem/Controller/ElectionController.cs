using System;
using System.Data;
using System.Data.SqlTypes;
using MySql.Data.MySqlClient;
using VotingSystem.Model;
using VotingSystem.Controller;

namespace VotingSystem.Controller
{
    public class ElectionController : IDbController<Election>
    {
        public int AddEntry(Election entry)
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

                using (MySqlCommand cmd = new MySqlCommand("add_election", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("varState", entry.State);
                    cmd.Parameters["@varState"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("varDistrict", entry.District);
                    cmd.Parameters["@varDistrict"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("varStartDate", entry.StartDate);
                    cmd.Parameters["@varStartDate"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("varEndDate", entry.EndDate);
                    cmd.Parameters["@varEndDate"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("varElectionId", MySqlDbType.Int32);
                    cmd.Parameters["@varElectionId"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\nCould not execute SQL procedure 'add_election' with parameters"
                                            + "\nState: " + entry.State
                                            + "\nDistrict: " + entry.District
                                            + "\nStartDate: " + entry.StartDate
                                            + "\nEndDate: " + entry.EndDate);
                        throw;
                    }

                    return Convert.ToInt32(cmd.Parameters["varElectionId"].Value);
                }
            }
        }

        public void DeleteEntry(int electionId)
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

                using (MySqlCommand cmd = new MySqlCommand("delete_election", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("varElectionId", electionId);
                    cmd.Parameters["@varElectionId"].Direction = ParameterDirection.Input;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\nCould not execute SQL procedure 'delete_election' with parameters"
                                            + "\nElectionId: " + electionId);
                        throw;
                    }
                }
            }
        }

        public int GetId(Election election)
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

                using (MySqlCommand cmd = new MySqlCommand("get_election_id_from_info", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("varState", election.State);
                    cmd.Parameters["@varState"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("varDistrict", election.District);
                    cmd.Parameters["@varDistrict"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("varStartDate", election.StartDate);
                    cmd.Parameters["@varStartDate"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("varEndDate", election.EndDate);
                    cmd.Parameters["@varEndDate"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("varElectionId", MySqlDbType.Int32);
                    cmd.Parameters["@varElectionId"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\nCould not execute SQL procedure 'get_election_id_from_info' with parameters"
                                            + "\nState: " + election.State
                                            + "\nDistrict: " + election.District
                                            + "\nStartDate: " + election.StartDate
                                            + "\nEndDate: " + election.EndDate);
                        throw;
                    }
                    return Convert.ToInt32(cmd.Parameters["varElectionId"].Value);
                }
            }
        }

        public Election GetInfo(int electionId)
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

                using (MySqlCommand cmd = new MySqlCommand("get_election_info_from_id", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("varElectionId", electionId);
                    cmd.Parameters["@varElectionId"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("varState", MySqlDbType.VarChar);
                    cmd.Parameters["@varState"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("varDistrict", MySqlDbType.Int32);
                    cmd.Parameters["@varDistrict"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("varStartDate", MySqlDbType.Date);
                    cmd.Parameters["@varStartDate"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("varEndDate", MySqlDbType.Date);
                    cmd.Parameters["@varEndDate"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\nCould not execute SQL procedure 'get_election_id_from_info' with parameters"
                                            + "\nElectionId: " + electionId);
                        throw;
                    }

                    Election election = new ElectionBuilder()
                        .WithState(Convert.ToString(cmd.Parameters["varState"].Value))
                        .WithDistrict(Convert.ToInt32(cmd.Parameters["varDistrict"].Value))
                        .WithStartDate(Convert.ToString(cmd.Parameters["varStartDate"].Value))
                        .WithEndDate(Convert.ToString(cmd.Parameters["varEndDate"].Value))
                        .Build();

                    return election;
                }
            }
        }
    }
}
