using VotingSystem.Controller;
using VotingSystem.Utils;

namespace VotingSystem.Model
{
    public class Admin : IUser
    {
        public string SerialNumber { get; }
        public string Username { get; }
        public string Password { get; }

        public static readonly IDbUserController<Admin> DbController = new AdminController();

        public Admin(string serialNum, string username, string password)
        {
            SerialNumber = serialNum;
            Username = username;
            Password = password;
        }
    }

    public class AdminBuilder
    { 
        public string SerialNumber; 
        public string Username; 
        public string Password;

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

        public AdminBuilder WithPassword(string password) 
        { 
            Password = password; 
            return this;
        }
        
        public Admin Build() 
        { 
            if (!Validation.IsValidSerialNumber(SerialNumber)) 
                throw new InvalidBuilderParameterException("Invalid serial number '" + SerialNumber + "'");
            if (!Validation.IsValidUsername(Username)) 
                throw new InvalidBuilderParameterException("Invalid username '" + Username + "'");
            if (!Validation.IsValidPassword(Password)) 
                throw new InvalidBuilderParameterException("Invalid password '" + Password + "'");

            Admin admin = new(SerialNumber, Username, Password); 
            return admin;
        }
    }
}
