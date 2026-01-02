using System;
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
        /******************************************
         ***********                    ***********
         ******   Auth service  properties   ******
         ***********       START        ***********
         ****************         *****************
         ******************************************/

        private readonly UserCollection _userCollection;

        private static readonly string _userCollectionFilepath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "users.dat");

        private static readonly byte[] _salt = System.Text.Encoding.UTF8.GetBytes(
            "testing salt for this fakeass operating system emulator");

        private static readonly JsonSerializerOptions _serialOptions =
            new JsonSerializerOptions { IncludeFields = true };

        /******************************************
         ***********                    ***********
         ******   Auth service  properties   ******
         ***********         END        ***********
         ****************         *****************
         ******************************************/



        // Constructor
        public AuthService()
        {
            this._userCollection = LoadUsers();
        }

        // Destructor
        ~AuthService()
        {
            // Gotta make sure stuff is saved at termination
            SaveUsers(this._userCollection);
        }



        /******************************************
         **********                      **********
         *******    Service functionality    ******
         **********        START         **********
         ****************         *****************
         ******************************************/

        public void CreateUser(string username, SecureString password, UserRole role = UserRole.USER)
        {
            User newUser = new(
                username,
                HashUserPassword(password),
                role
            );

            // AddUser already checks if username is taken
            if (! _userCollection.AddUser(newUser))
                throw new ArgumentException("Lietotājvārds jau ir aizņemts.");

            SaveUsers(this._userCollection);
            return;
        }

        public void DeleteUser(User user)
        {
            if (! _userCollection.RemoveUser(user.Username))
                throw new ArgumentException("Lietotājs nav atrasts.");

            SaveUsers(this._userCollection);
            return;
        }

        public void ChangeUsername(User user, string newUsername)
        {
            if (! _userCollection.CheckUserExists(user.Username))
                throw new ArgumentException("Lietotājs nav atrasts.");

            if (! _userCollection.RenameUser(user.Username, newUsername))
                throw new ArgumentException($"{user.Username} ir aizņemts.");

            // successfully registered
            SaveUsers(this._userCollection);
            return;
        }

        public void ChangePassword(User user, SecureString newPassword)
        {
            if (! _userCollection.TryGetUser(user.Username, out User storedUser))
                throw new ArgumentException("Lietotājs nav atrasts.");

            storedUser.PasswordHash = HashUserPassword(newPassword);

            SaveUsers(this._userCollection);
            return;
        }

        public User AuthenticateUser(string username, SecureString password)
        {
            if (!_userCollection.TryGetUser(username, out User storedUser))
                throw new ArgumentException("Lietotājs nav atrasts.");

            // Check if passwords are identical
            if (storedUser.PasswordHash != HashUserPassword(password))
                throw new ArgumentException("Autentifikācija nav bijusi veiksmīga");

            // Everything's clear
            return storedUser;
        }

        public bool AuthorizeUser(User user, UserRole role)
        {
            if (!_userCollection.TryGetUser(user.Username, out User storedUser))
                throw new ArgumentException("Lietotājs nav atrasts.");

            // check if user has required role
            if (! storedUser.HasRole(role))
                return false;

            return true;
        }

        public static bool PasswordsAreEqual(SecureString ss1, SecureString ss2)
        {
            string cryptSS1 = HashUserPassword(ss1);
            string cryptSS2 = HashUserPassword(ss2);
            return cryptSS1 == cryptSS2;
        }

        /******************************************
         **********                      **********
         *******    Service functionality    ******
         ***********         END        ***********
         ****************         *****************
         ******************************************/



        /******************************************
         **********                      **********
         *******    Service inner methods    ******
         **********        START         **********
         ****************         *****************
         ******************************************/

        static private UserCollection LoadUsers()
        {
            // If file doesn't exist, create an empty collection
            if (! File.Exists(_userCollectionFilepath))
                return new();

            string json = File.ReadAllText(_userCollectionFilepath);
            return JsonSerializer.Deserialize<UserCollection>(json);
        }

        static private void SaveUsers(UserCollection collection)
        {
            // Check if folder exists
            string dir = Path.GetDirectoryName(_userCollectionFilepath);
            if (! Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string json = JsonSerializer.Serialize(collection);
            // Could also encrypt here before we write, but eh
            File.WriteAllText(_userCollectionFilepath, json);
            return;
        }

        static private string HashUserPassword(SecureString newPassword)
        {
            // Copy SecurePassword into memory pseudo-securely
            IntPtr unmanagedBytes =
                Marshal.SecureStringToGlobalAllocUnicode(newPassword);
            byte[] passByte = new byte[newPassword.Length * 2];
            Marshal.Copy(unmanagedBytes, passByte, 0, passByte.Length);

            // Encrypt the password
            byte[] encryptedPassBytes = Rfc2898DeriveBytes.Pbkdf2(
                passByte,
                _salt,
                3,
                HashAlgorithmName.SHA512,
                30
            );

            return Convert.ToBase64String(encryptedPassBytes);
        }

        /******************************************
         **********                      **********
         *******    Service inner methods    ******
         ***********         END        ***********
         ****************         *****************
         ******************************************/
    }
}
