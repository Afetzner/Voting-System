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

                    cmd.Parameters.AddWithValue("varVoterLicenseNumber", entry.Voter.LicenceNumber);
                    cmd.Parameters["@varVoterLicenseNumber"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("varElectionId", entry.Election.GetId(entry.Election));
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
                                            + "\nLicenseNumber: " + entry.Voter.LicenseNumber
                                            + "\nElectionId: " + entry.Election.GetId(entry.Election));
                        throw;
                    }

                    return Convert.ToInt32(cmd.Parameters["varBallotId"].Value);
                }
            }
        }

        public Ballot GetInfo(int voterId, int electionId)
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

                using (MySqlCommand cmd = new MySqlCommand("get_ballot_info_from_voter_id", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("varVoterId", voterId);
                    cmd.Parameters["@varVoterId"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("VarElectionId", electionId);
                    cmd.Parameters["@varElectionId"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("varCandidateLastName", MySqlDbType.VarChar);
                    cmd.Parameters["@varCandidateLastName"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("varCandidateFirstName", MySqlDbType.VarChar);
                    cmd.Parameters["@varCandidateFirstName"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("varVote", MySqlDbType.Int32);
                    cmd.Parameters["@varVote"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\nCould not execute SQL procedure 'get_ballot_info_from_voter_id' with parameters");
                        throw;
                    }
                    /* sql query needs to be modified to retrieve ballot along with
                     list of Election Issues, or create new object to hold this query -
                    this is not a Ballot*/
                    //return new Ballot(int voterId, int electionId);
                }
            }
        }
    }
}
