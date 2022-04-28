using VotingSystem.Controller;
using VotingSystem.Utils;

namespace VotingSystem.Model
{
    public class Admin : IUser
    {
        public string SerialNumber { get; }
        public string Username { get; }
        public string? Password { get; }
        public string FirstName { get; }
        public string LastName { get; }

        public static readonly bool IsAdmin = true;

        public static readonly IDbUserController<Admin> DbController = new AdminController();

        public Admin(string serialNum, string username, string? password, string firstName, string lastName)
        {
            SerialNumber = serialNum;
            Username = username;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
        }
    }

    public class AdminBuilder
    { 
        public string SerialNumber; 
        public string Username; 
        public string? Password;
        public string FirstName;
        public string LastName;

        public AdminBuilder WithSerialNumber(string serialNum) 
        { 
            SerialNumber = serialNum; 
            return this;
        }

        public AdminBuilder WithUsername(string username) 
        { 
            Username = username; 
            return this;
        }

        public AdminBuilder WithPassword(string? password) 
        { 
            Password = password; 
            return this;
        }

        public AdminBuilder WithFirstName(string firstName)
        {
            FirstName = firstName;
            return this;
        }

        public AdminBuilder WithLastName(string lastName)
        {
            LastName = lastName;
            return this;
        }
        
        public Admin Build() 
        { 
            if (!Validation.IsValidSerialNumber(SerialNumber)) 
                throw new InvalidBuilderParameterException($@"Invalid serial number '{SerialNumber}'");
            if (!Validation.IsValidUsername(Username)) 
                throw new InvalidBuilderParameterException($@"Invalid username '{Username}'");
            if (!Validation.IsValidPassword(Password)) 
                throw new InvalidBuilderParameterException($@"Invalid password '{Password}'");
            if (!Validation.IsValidName(FirstName))
                throw new InvalidBuilderParameterException($@"Invalid first name '{FirstName}'");
            if (!Validation.IsValidName(LastName))
                throw new InvalidBuilderParameterException($@"Invalid last name '{LastName}'");

            Admin admin = new(SerialNumber, Username, Password, FirstName, LastName); 
            return admin;
        }
    }
}
