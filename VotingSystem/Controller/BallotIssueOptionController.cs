using System;
using System.Data;
using MySql.Data.MySqlClient;
using VotingSystem.Model;

namespace VotingSystem.Controller
{
    //Janice tried writing the ballot-issue-option controller, the problem was that the options 
    // are contained in a list in the issue, so they don't have access to the issue serial num
    // I suggest we move the add_issue_options into the issue controller (so issues and their options get added together)
    // We could have a separate func that adds options to existing issue too, but that's low priority.
    public class BallotIssueOptionController
    {
        public int AddEntry(BallotIssueOption entry)
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

                using (var cmd = new MySqlCommand("add_issue_option", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("title", entry.Title);
                    cmd.Parameters["@varTitle"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("number", entry.Number);
                    cmd.Parameters["@varNumber"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("varOptionId", MySqlDbType.Int32);
                    cmd.Parameters["@varOptionId"].Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\nCould not execute SQL procedure 'add_voter' with parameters"
                                            + "\nSerialNumber: " 
                                            + "\nTitle: " + entry.Title
                                            + "\nNumber: " + entry.Number);
                        throw;
                    }
                    // TO DO: 
                    return Convert.ToInt32(cmd.Parameters["varOptionId"].Value);
                }
            }
        }

        public void DeleteIssueOption(string issueSerialNumber, int optionNumber)
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

                using (var cmd = new MySqlCommand("delete_issue_option", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("varIssueSerialNumber", issueSerialNumber);
                    cmd.Parameters["@varIssueSerialNumber"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("varOptionNumber", optionNumber);
                    cmd.Parameters["@varOptionNumber"].Direction = ParameterDirection.Input;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\nCould not execute SQL procedure 'delete_voter' with parameters"
                                            + "\nissueSerialNumber: " + issueSerialNumber
                                            + "\noptionNumber: " + optionNumber);
                        throw;
                    }
                }
            }
        }

        // TODO: get serial number?  and get info

    }
}
