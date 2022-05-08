using VotingSystem.Model;

namespace VotingSystem.Controller
{
    /// <summary>
    /// Routes API methods to accessors, updates cache
    /// </summary>
    public static class UserEngine
    {
        private static readonly ResultCacheEngine _cacheManager = ResultCacheEngine.SharedCacheManager;

        public static bool AddUser(IUser user)
        {
            bool coll;
            if (user.IsAdmin)
                coll = Admin.Accessor.AddUser(user);
            else
                coll = Voter.Accessor.AddUser(user);
            return coll;
        }

        public static IUser? GetUser(string usernameSlashEmail, string password)
        {
            IUser? user = Voter.Accessor.GetUser(usernameSlashEmail, password);
            if (user == null)
                user = Admin.Accessor.GetUser(usernameSlashEmail, password);
            else
                _cacheManager.ForgetUserCache(user.SerialNumber);

            return user;
        }

        public static bool IsSerialInUse(string serial)
        {
            if (serial[0] == 'V')
                return Voter.Accessor.IsSerialInUse(serial);
            if (serial[0] == 'A')
                return Admin.Accessor.IsSerialInUse(serial);
            else
                return false;
        }

        public static bool IsUsernameInUse(string username)
        {
            return Voter.Accessor.IsUsernameInUse(username);
        }

        public static string GetSerial(char first)
        {
            if (first == 'V' || first == 'v')
                return Voter.Accessor.GetSerial();
            else if (first == 'A' || first == 'a')
                return Admin.Accessor.GetSerial();
            else
                throw new ArgumentException("Only first letters 'V' and 'A' supported for user serials");
        }
    }
}