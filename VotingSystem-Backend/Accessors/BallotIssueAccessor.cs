using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using VotingSystem.Model;
using VotingSystem.Utils;

namespace VotingSystem.Accessor
{
    public class BallotIssueAccessor
    {
        /// <summary>
        /// Adds an issue and its options to the DB
        /// </summary>
        /// <param name="issue">Issue to be added</param>
        /// <returns>false on serial number/title collision</returns>
        /// <exception cref="MySqlException">Bad connection to DB</exception>
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

        /// <summary>
        /// Removes an issue and its options from the DB
        /// </summary>
        /// <param name="serial">Serial number of issue</param>
        /// <exception cref="MySqlException">Bad connection to DB</exception>
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

        /// <summary>
        /// Gets all the ballot issue (and their options) from the DB
        /// </summary>
        /// <exception cref="MySqlException">Bad connection to DB</exception>
        /// <exception cref="InvalidBuilderParameterException">Corrupt data from DB</exception>
        public List<BallotIssue> GetBallotIssues()
        {
            List<BallotIssue> ballotIssueList = new List<BallotIssue>();

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

                using (var cmd = new MySqlCommand("get_issues", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        List<BallotIssueOption> optionsList = new List<BallotIssueOption>();

                        cmd.Parameters.Add("v_serialNumber", MySqlDbType.VarChar);
                        cmd.Parameters["v_serialNumber"].Direction = ParameterDirection.Output;

                        cmd.Parameters.Add("v_start", MySqlDbType.DateTime);
                        cmd.Parameters["v_start"].Direction = ParameterDirection.Output;

                        cmd.Parameters.Add("v_end", MySqlDbType.DateTime);
                        cmd.Parameters["v_end"].Direction = ParameterDirection.Output;

                        cmd.Parameters.Add("v_title", MySqlDbType.VarChar);
                        cmd.Parameters["v_title"].Direction = ParameterDirection.Output;

                        cmd.Parameters.Add("v_description", MySqlDbType.VarChar);
                        cmd.Parameters["v_description"].Direction = ParameterDirection.Output;

                        //TODO Not used
                        String? ballotIssueSerialNumber = Convert.ToString(cmd.Parameters["v_serialNumber"].Value);

                        using(var cmd2 = new MySqlCommand("get_options", conn))
                        {
                            cmd2.CommandType = CommandType.StoredProcedure;
                            MySqlDataReader reader2 = cmd2.ExecuteReader();

                            while (reader2.Read())
                            {
                                cmd2.Parameters.AddWithValue("v_serialNumber", ballotIssueSerialNumber);
                                cmd2.Parameters["v_serialNumber"].Direction = ParameterDirection.Input;

                                cmd2.Parameters.Add("v_number", MySqlDbType.Int32);
                                cmd2.Parameters["v_number"].Direction = ParameterDirection.Output;

                                cmd2.Parameters.Add("v_title", MySqlDbType.VarChar);
                                cmd2.Parameters["v_title"].Direction = ParameterDirection.Output;

                                cmd2.Parameters.AddWithValue("v_count", MySqlDbType.Int32);
                                cmd2.Parameters["v_count"].Direction = ParameterDirection.Output;

                                var newOption = new BallotIssueOption.BallotIssueOptionBuilder()
                                    .WithOptionNumber(Convert.ToInt32(cmd2.Parameters["v_number"].Value))
                                    .WithTitle(Convert.ToString(cmd2.Parameters["v_title"].Value))
                                    .Build();

                                optionsList.Add(newOption);
                            }
                        }
                        var ballotIssue = new BallotIssue.BallotIssueBuilder()
                            .WithSerialNumber(Convert.ToString(cmd.Parameters["v_serialNumber"].Value))
                            .WithStartDate(Convert.ToDateTime(cmd.Parameters["v_start"].Value))
                            .WithEndDate(Convert.ToDateTime(cmd.Parameters["v_end"].Value))
                            .WithTitle(Convert.ToString(cmd.Parameters["v_title"].Value))
                            .WithDescription(Convert.ToString(cmd.Parameters["v_description"].Value))
                            .WithOptions(optionsList)
                            .Build();
                        ballotIssueList.Add(ballotIssue);
                    }
                }
            }
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
            return ballotIssueList;
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

                using (var cmd = new MySqlCommand("check_issue_serial", conn))
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
                        Console.WriteLine(e + "\n" +
                                          $@"Could not execute SQL procedure 'check_issue_serial' with parameters: 
serialNumber: '{serial}'");

                        throw;
                    }

                    return Convert.ToBoolean(cmd.Parameters["v_occupied"].Value);
                }
            }
        }

    }
}
