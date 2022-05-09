using MySql.Data.MySqlClient;
using System.Data;
using VotingSystem.Utils;
using VotingSystem.Model;

namespace VotingSystem.Accessor
{
    public class BallotIssueAccessor : IBallotIssueAccessor
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
                    catch (MySqlException e) when (e.ErrorCode == -2147467259)
                    {
                        //Duplicate entry, do not throw, handled by return value
                        return true;
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e + "\n" + $@"Could not execute SQL procedure 'add_issue' with parameters:
                            serialNumber: '{issue.SerialNumber}', 
                            title: '{issue.Title}'");

                        throw;
                    }
                    collision1 = Convert.ToBoolean(cmd.Parameters["v_collision"].Value);
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
                            catch (MySqlException e) when (e.ErrorCode== -2147467259)
                            {
                                //Duplicate entry, don't throw, handled by return value
                                return true;
                            }
                            catch (MySqlException e)
                            {
                                Console.WriteLine(e + "\nCould not execute SQL procedure 'add_issue_option' with parameters"
                                                    + "\nTitle: " + option.Title
                                                    + "\nNumber: " + option.Number);
                                throw;
                            }
                            collision2 = Convert.ToBoolean(cmd.Parameters["v_collision"].Value);
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

        public string GetSerial()
        {
            string serial;
            do
            {
                serial = SerialGenerator.Generate('I');
            } while (IsSerialInUse(serial));
            return serial;
        }
    }
}
