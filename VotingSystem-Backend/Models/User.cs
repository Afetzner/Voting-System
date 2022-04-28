using VotingSystem.Model;
using VotingSystem.Utils;

namespace VotingSystem.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }

        public User(string username, string password, string email, string firstName, string lastName, bool isAdmin)
        {
            Username = username;
            Password = password;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            IsAdmin = isAdmin;
        }
    }

    public class UserBuilder
    {

        public string Username;
        public string Password;
        public string Email;
        public string LastName;
        public string FirstName;
        public bool IsAdmin;

        public UserBuilder WithUsername(string username)
        {
            Username = username;
            return this;
        }

        public UserBuilder WithPassword(string password)
        {
            Password = password;
            return this;
        }

        public UserBuilder WithLastName(string lastName)
        {
            LastName = lastName;
            return this;
        }

        public UserBuilder WithFirstName(string firstName)
        {
            FirstName = firstName;
            return this;
        }

        public User Build()
        {
            if (!Validation.IsValidUsername(Username))
                throw new InvalidBuilderParameterException("Invalid username '" + Username + "'");
            if (!Validation.IsValidPassword(Password))
                throw new InvalidBuilderParameterException("Invalid password '" + Password + "'");
            if (!Validation.IsValidName(LastName))
                throw new InvalidBuilderParameterException("Invalid last name '" + LastName + "'");
            if (!Validation.IsValidName(FirstName))
                throw new InvalidBuilderParameterException("Invalid first name '" + FirstName + "'");

            User user = new(Username, Password, Email, FirstName, LastName, IsAdmin);
            return user;
        }
    }
}