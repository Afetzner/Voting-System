using VotingSystem.Utils;
using VotingSystem.Accessor;

namespace VotingSystem.Model
{
    public class Admin : IUser
    {
        public string SerialNumber { get; }
        public string Username { get; }
        public string Password { get; }
        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public bool IsAdmin { get { return true; } }

        public static readonly IUserAccessor Accessor = new UserDbAccessor();

        public Admin(string serialNum, string username, string password, string email, string firstName, string lastName)
        {
            SerialNumber = serialNum;
            Username = username;
            Password = password;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
    }

    public class AdminBuilder
    { 
        public string? SerialNumber; 
        public string? Username; 
        public string? Password;
        public string? Email;
        public string? FirstName;
        public string? LastName;

        public AdminBuilder WithSerialNumber(string? serialNum) 
        { 
            SerialNumber = serialNum; 
            return this;
        }

        public AdminBuilder WithUsername(string? username) 
        { 
            Username = username; 
            return this;
        }

        public AdminBuilder WithPassword(string? password) 
        { 
            Password = password; 
            return this;
        }

        public AdminBuilder WithEmail(string? email)
        {
            Email = email;
            return this;
        }

        public AdminBuilder WithFirstName(string? firstName)
        {
            FirstName = firstName;
            return this;
        }

        public AdminBuilder WithLastName(string? lastName)
        {
            LastName = lastName;
            return this;
        }
        
        public Admin Build() 
        { 
            if (SerialNumber == null || !Validation.IsValidSerialNumber(SerialNumber)) 
                throw new InvalidBuilderParameterException($@"Invalid serial number '{SerialNumber}'");
            if (Username == null || !Validation.IsValidUsername(Username)) 
                throw new InvalidBuilderParameterException($@"Invalid username '{Username}'");
            if (Password == null || !Validation.IsValidPassword(Password)) 
                throw new InvalidBuilderParameterException($@"Invalid password '{Password}'");
            if (Email == null)
                throw new InvalidBuilderParameterException($@"Invalid email (null) '{Email}'");
            if (FirstName == null || !Validation.IsValidName(FirstName))
                throw new InvalidBuilderParameterException($@"Invalid first name '{FirstName}'");
            if (LastName == null || !Validation.IsValidName(LastName))
                throw new InvalidBuilderParameterException($@"Invalid last name '{LastName}'");

            Admin admin = new(SerialNumber, Username, Password, Email, FirstName, LastName); 
            return admin;
        }
    }
}
