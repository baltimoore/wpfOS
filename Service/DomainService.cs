using System.IO;
using System.Text.Json;
using wpfOs.Model;
using wpfOs.View.Apps.Settings;

namespace wpfOs.Service
{
    public class DomainService
    {
        /******************************************
         ***********                    ***********
         ******   Domain service  properties   ******
         ***********       START        ***********
         ****************         *****************
         ******************************************/

        private readonly DomainCollection _domainCollection;

        private static readonly string _domainCollectionFilepath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "domains.dat");

        private static readonly JsonSerializerOptions _serialOptions =
            new JsonSerializerOptions { IncludeFields = true };

        /******************************************
         ***********                    ***********
         ******   Domain service  properties   ******
         ***********         END        ***********
         ****************         *****************
         ******************************************/



        // Constructor
        public DomainService()
        {
            this._domainCollection = LoadDomains();
        }

        // Destructor
        ~DomainService()
        {
            // Gotta make sure stuff is saved at termination
            SaveDomains(this._domainCollection);
        }



        /******************************************
         **********                      **********
         *******    Service functionality    ******
         **********        START         **********
         ****************         *****************
         ******************************************/

        /* 
         * Admin only methods
         */

        // I don't want to store the auth service within itself
        public List<Domain> GetAllDomains(AuthService authService, User currentUser)
        {
            // cred check
            if (!authService.AuthorizeUser(currentUser, UserRole.ADMIN))
                throw new ArgumentException("Jums nav tiesību veikt šo darbību.");

            return _domainCollection.GetAllDomains();
        }

        public void UpdateDomainStatus(
            Domain domain, DomainStatus newStatus,
            AuthService authService, User currentUser
        ) {
            // cred check
            if (!authService.AuthorizeUser(currentUser, UserRole.ADMIN))
                throw new ArgumentException("Jums nav tiesību veikt šo darbību.");

            if (! _domainCollection.TryGetDomain(domain.Name, out Domain storedDomain))
                throw new ArgumentException("Domēns nav atrasts.");

            storedDomain.Status = newStatus;

            SaveDomains(this._domainCollection);
            return;
        }

        /* 
         * all user methods
         */

        public void RequestDomain(User user, string name)
        {
            Domain newDomain = new(
                name,
                user.Id,
                DomainStatus.PENDING
            );

            // AddDomain already checks if name is taken
            if (!_domainCollection.AddDomain(newDomain))
                throw new ArgumentException("Domēna nosaukums jau ir aizņemts.");

            SaveDomains(this._domainCollection);
            return;
        }

        public void CancelDomain(User user, Domain domain)
        {
            // check if user has permissions to cancel
            if (domain.RequestedBy != user.Id)
                throw new ArgumentException("Jums nav tiesību veikt šo darbību.");

            domain.Status = DomainStatus.CLOSED;

            SaveDomains(this._domainCollection);
            return;
        }

        public List<Domain> GetUserDomains(User user)
        {
            return _domainCollection.GetDomainsByUser(user.Id);
        }

        /******************************************
         **********                      **********
         *******    Service functionality    ******
         ***********         END        ***********
         ******************       *****************
         ******************************************/



        /******************************************
         **********                      **********
         *******    Service inner methods    ******
         **********        START         **********
         ****************         *****************
         ******************************************/

        static private DomainCollection LoadDomains()
        {
            // If file doesn't exist, create an empty collection
            if (!File.Exists(_domainCollectionFilepath))
                return new();

            string json = File.ReadAllText(_domainCollectionFilepath);
            return JsonSerializer.Deserialize<DomainCollection>(json, _serialOptions);
        }

        static private void SaveDomains(DomainCollection collection)
        {
            // Check if folder exists
            string dir = Path.GetDirectoryName(_domainCollectionFilepath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string json = JsonSerializer.Serialize(collection, _serialOptions);
            File.WriteAllText(_domainCollectionFilepath, json);
            return;
        }

        /******************************************
         **********                      **********
         *******    Service inner methods    ******
         ***********         END        ***********
         ****************         *****************
         ******************************************/
    }
}

