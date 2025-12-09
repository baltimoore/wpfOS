namespace wpfOs.Model
{
    public class User
    {
        // Class properties
        private string _username;
        private string _passwordHash;
        private UserRole[] _userRoles;

        // Exposed encapsulated properties
        public string Username
        {
            get => _username;
            set => _username = value;
        }
        public string PasswordHash
        {
            get => _passwordHash;
            set => _passwordHash = value;
        }
        public UserRole[] UserRoles
        {
            get => _userRoles;
            set => _userRoles = value;
        }

        // Constructor
        public User(string username, string hash)
        {
            Username = username;
            PasswordHash = hash;
            UserRoles = [UserRole.USER];
        }
        public User(string username, string hash, UserRole role)
        {
            Username = username;
            PasswordHash = hash;
            UserRoles = [role];
        }
        public User(string username, string hash, UserRole[] roles)
        {
            Username = username;
            PasswordHash = hash;
            UserRoles = roles;
        }

        // Class methods
    }

    public enum UserRole
    {
        USER,
        ADMIN
    }
}
