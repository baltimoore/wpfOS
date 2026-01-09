using System.Text.Json.Serialization;

namespace wpfOs.Model
{
    public class Domain
    {
        /******************************************
         ***********                    ***********
         ***********  Domain properties ***********
         ***********       START        ***********
         ****************         *****************
         ******************************************/

        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        private Guid _requestedBy;
        public Guid RequestedBy
        {
            get => _requestedBy;
            set => _requestedBy = value;
        }

        private DomainStatus _status;
        public DomainStatus Status
        {
            get => _status;
            set => _status = value;
        }

        /******************************************
         ***********                    ***********
         ***********  Domain properties ***********
         ***********         END        ***********
         ****************         *****************
         ******************************************/



        // Constructor overloads
        [JsonConstructor]
        public Domain(string name, Guid requestedBy, DomainStatus status = DomainStatus.PENDING)
        {
            Name = name;
            RequestedBy = requestedBy;
            Status = status;
        }
    }

    public enum DomainStatus
    {
        PENDING,
        MANAGED,
        REJECTED,
        CLOSED
    }
}
