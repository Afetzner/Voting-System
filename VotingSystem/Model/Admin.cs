namespace VotingSystem.Model
{
    public class Admin
    {
        public string SerialNumber { get; }
        public string Username { get; }
        public string Password { get; }

        public Admin(string serialNum, string username, string password)
        {
            SerialNumber = serialNum;
            Username = username;
            Password = password;
        }

    }
}
