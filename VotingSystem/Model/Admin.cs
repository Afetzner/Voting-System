namespace VotingSystem.Model
{
    public class Admin
    {
        public string Username { get; }
        public string Password { get; }

        public Admin(string username, string password)
        {
            Username = username;
            Password = password;
        }

    }
}
