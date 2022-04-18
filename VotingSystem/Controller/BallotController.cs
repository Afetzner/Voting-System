using System;
using System.Data;
using System.Data.SqlTypes;
using MySql.Data.MySqlClient;
using VotingSystem.Model;
using VotingSystem.Controller;

namespace VotingSystem.Controller
{
    public class BallotController : IDbController<Ballot>
    {
        public int AddEntry(Ballot entry)
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

                using (MySqlCommand cmd = new MySqlCommand("add_ballot", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("varVoterId", entry.VoterId);
                    cmd.Parameters["@varVoterId"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("varElectionId", entry.ElectionId);
                    cmd.Parameters["@varElectionId"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("varBallotId", MySqlDbType.Int32);
                    cmd.Parameters["@varBallotId"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\nCould not execute SQL procedure 'add_ballot' with parameters"
                                            + "\nVoterID: " + entry.VoterId
                                            + "\nElectionId: " + entry.ElectionId);
                        throw;
                    }

                    return Convert.ToInt32(cmd.Parameters["varBallotId"].Value);
                }
            }
        }


        public void DeleteEntry(int ballotId)
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

                using (MySqlCommand cmd = new MySqlCommand("delete_ballot", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("varBallotId", ballotId);
                    cmd.Parameters["@varBallotId"].Direction = ParameterDirection.Input;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\nCould not execute SQL procedure 'delete_ballot' with parameters"
                                            + "\nBallotId: " + ballotId);
                        throw;
                    }
                }
            }
        }

        public int GetId(Ballot ballot) 
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

                using (MySqlCommand cmd = new MySqlCommand("get_ballot_id_from_info", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("varVoterId", ballot.VoterId);
                    cmd.Parameters["@varVoterId"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("varElectionId", ballot.ElectionId);
                    cmd.Parameters["@varElectionId"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("varBallotId", MySqlDbType.Int32);
                    cmd.Parameters["@varBallotId"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\nCould not execute SQL procedure 'get_ballot_id_from_info' with parameters"
                                            + "\nVoterId: " + ballot.VoterId
                                            + "\nElectionId: " + ballot.ElectionId);
                        throw;
                    }
                    return Convert.ToInt32(cmd.Parameters["varBallotId"].Value);
                }
            }
        }


        public Ballot GetInfo(int ballotId)
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

                using (MySqlCommand cmd = new MySqlCommand("get_ballot_info_from_id", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("varBallotId", ballotId);
                    cmd.Parameters["@varBallotId"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("varVoterId", MySqlDbType.VarChar);
                    cmd.Parameters["@varVoterId"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("varElectionId", MySqlDbType.VarChar);
                    cmd.Parameters["@varElectionId"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\nCould not execute SQL procedure 'get_ballot_info_from_id' with parameters"
                                            + "\nBallot Id: " + ballotId); 
                        throw;
                    }
                    Ballot ballot = new BallotBuilder()
                        .WithVoter(Convert.ToInt32(cmd.Parameters["varVoterId"].Value))
                        .WithElection(Convert.ToInt32(cmd.Parameters["varElectionId"].Value))
                        .Build();

                    return ballot;
                }
            }
        }
    }
}
