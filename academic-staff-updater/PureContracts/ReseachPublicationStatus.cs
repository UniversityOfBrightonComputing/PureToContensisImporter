using System.Runtime.Serialization;

namespace academic_staff_updater.PureContracts
{
    [DataContract]
    public class ReseachPublicationStatus
    {
        [DataMember(Name = "publicationDate")]
        public ResearchPublicationDate Date { get; set; }
    }
}
