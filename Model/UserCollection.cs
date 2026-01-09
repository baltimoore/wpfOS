using System.Text.Json.Serialization;

namespace wpfOs.Model
{
    public class UserCollection
    {
        /******************************************
         ***********                    ***********
         *****   UserCollection properties   ******
         ***********       START        ***********
         ****************         *****************
         ******************************************/

        // While the app in general doesn't need full access of all registered users,
        // JSON serializer only saves public properties.
        // so yaaaaay
        [JsonInclude]
        public Dictionary<Guid, User> _users;

        // index for username-based lookups
        private Dictionary<String, Guid> _usernameIndex;

        public int UserCount { get { return _users.Count; } }

        /******************************************
         ***********                    ***********
         *****   UserCollection properties   ******
         ***********        END         ***********
         ****************         *****************
         ******************************************/



        // parameterless constructor purely for deserialization
        public UserCollection() {
            _users = new Dictionary<Guid, User>();
            _usernameIndex = new Dictionary<String, Guid>();
        }

        // could be inside constructor,
        // but eh
        public void RebuildUsernameIndex()
        {
            _usernameIndex.Clear();
            foreach (var user in _users.Values)
            {
                _usernameIndex[user.Username] = user.Id;
            }
        }



        /******************************************
         ***********                    ***********
         *****     UserCollection methods    ******
         ***********       START        ***********
         ****************         *****************
         ******************************************/

        public bool AddUser(User user)
        {
            // Check username uniqueness
            if (_usernameIndex.ContainsKey(user.Username))
                return false;

            // Add to both dictionaries
            if (! _users.TryAdd(user.Id, user))
                return false;

            _usernameIndex[user.Username] = user.Id;
            return true;
        }

        public bool RemoveUser(string username)
        {
            if (! _usernameIndex.TryGetValue(username, out Guid userId))
                return false;

            return RemoveUser(userId);
        }

        public bool RemoveUser(Guid userId)
        {
            if (! _users.TryGetValue(userId, out User user))
                return false;

            _usernameIndex.Remove(user.Username);
            _users.Remove(userId);
            return true;
        }

        public bool CheckUserExists(string username)
        {
            return _usernameIndex.ContainsKey(username);
        }

        public bool CheckUserExists(Guid userId)
        {
            return _users.ContainsKey(userId);
        }

        public bool TryGetUser(string username, out User user)
        {
            user = null;
            if (! _usernameIndex.TryGetValue(username, out Guid userId))
                return false;

            return _users.TryGetValue(userId, out user);
        }

        public bool TryGetUser(Guid userId, out User user)
        {
            return _users.TryGetValue(userId, out user);
        }

        public bool RenameUser(string oldUsername, string newUsername)
        {
            // Check if user exists
            if (! _usernameIndex.TryGetValue(oldUsername, out Guid userId))
                return false;

            return RenameUser(userId, newUsername);
        }
        
        public bool RenameUser(Guid userId, string newUsername)
        {
            // Check if user exists
            if (!_users.TryGetValue(userId, out User user))
                return false;

            // Check if new username is not already taken
            if (_usernameIndex.ContainsKey(newUsername))
                return false;

            _usernameIndex[newUsername] = userId;
            user.Username = newUsername;
            return true;
        }

        /******************************************
         ***********                    ***********
         *****     UserCollection methods    ******
         ***********        END         ***********
         ****************         *****************
         ******************************************/
    }
}
