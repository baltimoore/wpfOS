using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text.Json;
using wpfOs.Model;

namespace wpfOs.Service
{
    public class AuthService
    {
        // Class properties
        private UserCollection _userCollection;
        private static readonly string _userCollectionFilepath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "users.dat");
        private static readonly byte[] _salt = System.Text.Encoding.UTF8.GetBytes(
            "testing salt for this fakeass operating system emulator");

        // Exposed encapsulated properties

        // Constructor
        public AuthService()
        {
            this._userCollection = LoadUsers();
        }
        ~AuthService()
        {
            SaveUsers(this._userCollection);
        }

        // Public class methods
        public void CreateUser(string newUsername, SecureString newPassword)
        {
            User newUser = new(
                newUsername,
                HashUserPassword(newPassword),
                UserRole.USER
            );
            _userCollection.Users.Add(newUser);
        }

        public void DeleteUser(User user)
        {
            if (!_userCollection.Users.Contains(user))
                return;

            _userCollection.Users.Remove(user);
            SaveUsers(this._userCollection);
        }

        public void ChangeUsername(User user, string newUsername)
        {
            User? ExistingUser = FindUser(user);
            if (ExistingUser == null)
                return;

            ExistingUser.Username = newUsername;
            SaveUsers(this._userCollection);
        }

        public void ChangePassword(User user, SecureString newPassword)
        {
            // check if user exists
            User? ExistingUser = FindUser(user);
            if (ExistingUser == null)
                return;

            ExistingUser.PasswordHash = HashUserPassword(newPassword);
            SaveUsers(this._userCollection);
        }

        public dynamic AuthenticateUser(string username, SecureString password)
        {
            // check if username exists
            User? ExistingUser = FindUser(username);
            if (ExistingUser == null)
                return null;

            // Check if passwords are identical
            if (ExistingUser.PasswordHash != HashUserPassword(password))
                return false;

            // Everything checks out~
            return ExistingUser;
        }

        public bool AuthorizeUser(User user, UserRole role)
        {
            // check if user exists
            User? ExistingUser = FindUser(user);
            if (ExistingUser == null)
                return false;

            // check if user has required role
            if (!ExistingUser.UserRoles.Contains(role))
                return false;

            return true;
        }

        // Private class methods
        static private UserCollection LoadUsers()
        {
            // If file doesn't exist, create an empty collection
            if (!File.Exists(_userCollectionFilepath))
                return new();

            string json = File.ReadAllText(_userCollectionFilepath);
            return JsonSerializer.Deserialize<UserCollection>(json);
        }

        static private void SaveUsers(UserCollection collection)
        {
            // Check if folder exists
            string dir = Path.GetDirectoryName(_userCollectionFilepath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string json = JsonSerializer.Serialize(collection);
            File.WriteAllText(_userCollectionFilepath, json);
        }

        static string HashUserPassword(SecureString newPassword)
        {
            // Copy SecurePassword into memory pseudo-securely
            IntPtr unmanagedBytes =
                Marshal.SecureStringToGlobalAllocUnicode(newPassword);
            byte[] passByte = new byte[newPassword.Length * 2];
            Marshal.Copy(unmanagedBytes, passByte, 0, passByte.Length);

            byte[] encryptedPassBytes = Rfc2898DeriveBytes.Pbkdf2(
                passByte,
                _salt,
                3,
                HashAlgorithmName.SHA512,
                30
            );

            return System.Text.Encoding.UTF8.GetString(encryptedPassBytes);
        }

        private User? FindUser(string username)
        {
            return this._userCollection.Users.FirstOrDefault(x => x.Username == username, null);
        }

        private User? FindUser(User user)
        {
            return this._userCollection.Users.FirstOrDefault(x => x == user, null);
        }
    }
}
