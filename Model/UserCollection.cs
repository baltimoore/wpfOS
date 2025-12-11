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
        public Dictionary<String, User> _users;

        /******************************************
         ***********                    ***********
         *****   UserCollection properties   ******
         ***********        END         ***********
         ****************         *****************
         ******************************************/



        // parameterless constructor purely for deserialization
        public UserCollection() {
            _users = new Dictionary<String, User>();
        }



        /******************************************
         ***********                    ***********
         *****     UserCollection methods    ******
         ***********       START        ***********
         ****************         *****************
         ******************************************/

        public bool AddUser(User user)
        {
            return _users.TryAdd(user.Username, user);
        }

        public bool RemoveUser(string username)
        {
            return _users.Remove(username);
        }

        public bool CheckUserExists(string username)
        {
            return _users.ContainsKey(username);
        }

        public bool TryGetUser(string username, out User user)
        {
            return _users.TryGetValue(username, out user);
        }

        public bool RenameUser(string oldUsername, string newUsername)
        {
            // Check if user exists
            if (! _users.TryGetValue(oldUsername, out User user)) return false;
            // Check if username is not already taken
            if (_users.ContainsKey(newUsername)) return false;

            _users.Remove(oldUsername);
            user.Username = newUsername;
            _users[newUsername] = user;
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
