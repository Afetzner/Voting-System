using VotingSystem.Accessor;
using System;
using VotingSystem.Model;
using System.Data;
using MySql.Data.MySqlClient;

namespace VotingSystem.Model
{
    public class BallotAccessor : IBallotAccessor
    {
        //TODO
        public bool AddBallot(Ballot ballot)
        {
            throw new NotImplementedException();
        }

        public List<Ballot> GetBallotsByVoter(string voterSerial)
        {
            throw new NotImplementedException();
        }

        public List<BallotIssue> GetIssuesVotedOn(string voterSerial)
        {
            throw new NotImplementedException();
        }

        public bool IsSerialInUse(string ballotSerial)
        {
            throw new NotImplementedException();
        }

        public void RemoveBallot(string serial)
        {
            throw new NotImplementedException();
        }
    }
}
    /*
    public bool AddBallot(Ballot ballot)
{
            using (var conn = new MySqlConnection(DbConnecter.ConnectionString))
            {
                conn.Open();
            }
            catch(MySqlException e)
            {
                Console.WriteLine(e + "\nCould not connect to database");
                throw;
            }

            using (var cmd = new MySqlCommand("add_ballot"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                /*
                IN `v_ballotSerial` varchar(9),  
                IN `v_voterSerial` varchar(9),
                IN `v_issueSerial` varchar(9),
                IN `v_choiceNumber` int,
                OUT `v_collision` bool)
                cmd.Parameters.AddWithValue("v_ballotSerial", ballot.)
            }
      
    throw new NotImplementedException();
}

public List<Ballot> GetBallotsByVoter(string voterSerial)
{
            
           CREATE PROCEDURE afetzner.get_voters_ballot(
    IN `v_voterSerial` varchar(9),
    IN `v_issueSerial` varchar(9),
    OUT `v_ballotSerial` varchar(9),
    OUT `v_choiceNumber` int,
    OUT `v_choiceTitle` varchar(127))
BEGIN
  END
                
    using(var conn = new MySqlConnection(DbConnecter.ConnectionString))
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

                using (var cmd = new MySqlCommand("get_voters_ballot", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("v_voterSerial", voterSerial);
                    cmd.Parameters["v_voterSerial"].Direction = ParameterDirection.Input
                }
            }
    throw new NotImplementedException();
}

public List<BallotIssue> GetIssuesVotedOn(string voterSerial)
{
    throw new NotImplementedException();
}

public bool IsSerialInUse(string ballotSerial)
{
    using (var conn = new MySqlConnection(DbConnecter.ConnectionString))
    {
        try
        {
            conn.Open();
        }
        catch (MySqlException e)
        {
            Console.WriteLine(e + "\nCound not connect to database");
            throw;
        }

        using (var cmd = new MySqlCommand("check_ballot_serial")
    
    CREATE PROCEDURE afetzner.check_ballot_serial(
    IN `v_ballotSerial` varchar(9),
    OUT `v_occupied` bool)
    
    throw new NotImplementedException();
}

public void RemoveBallot(string serial)
{
    throw new NotImplementedException();
}
}
}
    */