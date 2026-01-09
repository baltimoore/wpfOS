using System.Text.Json.Serialization;

namespace wpfOs.Model
{
    public class DomainCollection
    {
        /******************************************
         ***********                    ***********
         *****  DomainCollection properties   ******
         ***********       START        ***********
         ****************         *****************
         ******************************************/

        // JSON serializer STILL only saves public properties.
        [JsonInclude]
        public Dictionary<string, Domain> _domains;

        public int DomainCount { get { return _domains.Count; } }

        /******************************************
         ***********                    ***********
         *****  DomainCollection properties   ******
         ***********        END         ***********
         ****************         *****************
         ******************************************/



        // parameterless constructor purely for deserialization
        public DomainCollection()
        {
            _domains = new Dictionary<string, Domain>();
        }



        /******************************************
         ***********                    ***********
         *****     DomainCollection methods    ******
         ***********       START        ***********
         ****************         *****************
         ******************************************/

        public bool AddDomain(Domain domain)
        {
            // Check name uniqueness and check if the found domain is managed
            if (_domains.TryGetValue(domain.Name, out Domain foundDomain))
                if (foundDomain.Status == DomainStatus.MANAGED)
                    return false;

            return _domains.TryAdd(domain.Name, domain);
        }

        public bool RemoveDomain(string name)
        {
            return _domains.Remove(name);
        }

        public bool CheckDomainExists(string name)
        {
            return _domains.ContainsKey(name);
        }

        public bool TryGetDomain(string name, out Domain domain)
        {
            return _domains.TryGetValue(name, out domain);
        }

        public List<Domain> GetDomainsByUser(Guid userId)
        {
            return _domains.Values.Where(d => d.RequestedBy == userId).ToList();
        }

        public List<Domain> GetAllDomains()
        {
            return _domains.Values.ToList();
        }

        /******************************************
         ***********                    ***********
         *****     DomainCollection methods    ******
         ***********        END         ***********
         ****************         *****************
         ******************************************/
    }
}

