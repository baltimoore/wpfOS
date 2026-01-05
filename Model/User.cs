using System.Data;
using System.Text.Json.Serialization;

namespace wpfOs.Model
{
    public class User
    {
        /******************************************
         ***********                    ***********
         ***********  User properties   ***********
         ***********       START        ***********
         ****************         *****************
         ******************************************/

        private string _username;
        public string Username
        {
            get => _username;
            set => _username = value;
        }

        private string _passwordHash;
        public string PasswordHash
        {
            get => _passwordHash;
            set => _passwordHash = value;
        }

        private List<UserRole> _userRoles;
        public List<UserRole> Roles
        {
            get => _userRoles;
            set => _userRoles = value;
        }

        public string RolesString
        {
            get => string.Join(", ", Roles.Select(role => role.ToString()));
        }

        /******************************************
         ***********                    ***********
         ***********  User properties   ***********
         ***********         END        ***********
         ****************         *****************
         ******************************************/



        // Constructor overloads

        public User(string username, string passwordHash)
            : this(username, passwordHash, UserRole.USER)
        { }

        public User(string username, string passwordHash, UserRole role)
            : this(username, passwordHash, [role])
        { }

        [JsonConstructor]
        public User(string username, string passwordHash, List<UserRole> roles)
        {
            Username = username;
            PasswordHash = passwordHash;
            Roles = roles;
        }



        /******************************************
         ***********                    ***********
         **********     User methods    ***********
         ***********       START        ***********
         ****************         *****************
         ******************************************/
        public bool HasRole(UserRole role) => Roles.Contains(role);
    }

    public enum UserRole
    {
        USER,
        ADMIN
    }
}
