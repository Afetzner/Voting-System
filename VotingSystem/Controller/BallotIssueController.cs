using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using VotingSystem.Model;

namespace VotingSystem.Controller
{
    class BallotIssueController : IDbBallotIssueController
    {
        public bool AddIssue(BallotIssue issue)
        {
            bool collision1 = false;
            bool collision2 = false;

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

                using (var cmd = new MySqlCommand("add_issue", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("v_serialNumber", issue.SerialNumber);
                    cmd.Parameters["@v_serialNumber"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_start", issue.StartDate);
                    cmd.Parameters["v_start"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_end", issue.EndDate);
                    cmd.Parameters["v_end"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_title", issue.Title);
                    cmd.Parameters["v_title"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_description", issue.Description);
                    cmd.Parameters["v_description"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("v_collision", collision1);
                    cmd.Parameters["v_collision"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\n" + $@"Could not execute SQL procedure 'add_issue' with parameters:
                            serialNumber: '{issue.SerialNumber}', 
                            title: '{issue.Title}'");

                        throw;
                    }
                }
                if (!collision1)
                {
                    foreach (BallotIssueOption option in issue.Options)
                    {
                        using (var cmd = new MySqlCommand("add_issue_option", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("v_issueSerialNumber", issue.SerialNumber);
                            cmd.Parameters["v_issueSerialNumber"].Direction = ParameterDirection.Input;

                            cmd.Parameters.AddWithValue("v_title", option.Title);
                            cmd.Parameters["v_title"].Direction = ParameterDirection.Input;

                            cmd.Parameters.AddWithValue("v_number", option.Number);
                            cmd.Parameters["v_number"].Direction = ParameterDirection.Input;

                            cmd.Parameters.AddWithValue("v_collision", collision2);
                            cmd.Parameters["v_collision"].Direction = ParameterDirection.Output;

                            try
                            {
                                cmd.ExecuteNonQuery();
                            }
                            catch (MySqlException e)
                            {
                                Console.WriteLine(e + "\nCould not execute SQL procedure 'add_issue_option' with parameters"
                                                    + "\nTitle: " + option.Title
                                                    + "\nNumber: " + option.Number);
                                throw;
                            }
                        }
                    } //foreach option, use add_isssue_option
                } //if not collisoin1
            } //using conn
            return (collision1 || collision2); //returns fasle (as in, "no collisions" - desired result) only if both are false
        }

        public void RemoveIssue(string serial)
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

                using (var cmd = new MySqlCommand("delete_issue", conn))
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
                        Console.WriteLine(e + "\n" + $@"Could not execute SQL procedure 'delete_issue' with parameters: 
                            serialNumber: '{serial}'");

                        throw;
                    }
                }
            }
        }

        //TODO
        public List<BallotIssue> GetBallotIssues()
        {
            //Using conn = ...
            //  Using cmd = "get_ballot_issue"
            //  cmd.type = stored procedure
            //  set up output cmd args (see voterController getUser)
            //  execute cmd
            //
            //  iterate through returned values
            //  (the return values will be a table, not a single item,
            //  you'll have to figure out how to do this, no other funcs have done this yet)
            //  start building issue with serial number, title ... ect.
            //  foreach returned value {
            //    using cmd = "get_issue_options"
            //    cmd.args["serialNumber"] = current issue being built's serial number
            //    ...
            //    execute cmd
            //    foreach returned value {
            //      var option = ballotIssueOptionBuilder().WithTitle(...)... .Build()
            //      add option to issue
            //   build issue
            //return issues
            throw new NotImplementedException();
        }
    }
}
