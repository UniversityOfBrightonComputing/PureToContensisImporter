using System.Collections.Generic;
using System.Runtime.Serialization;

namespace academic_staff_updater.PureContracts
{
    [DataContract]
    public class PersonItem
    {
        [DataMember(Name = "pureId")]
        public int PureId { get; set; }
        [DataMember(Name = "name")]
        public PersonName Name { get; set; }
        [DataMember(Name = "titles")]
        public List<PersonValue> Titles { get; set; }
        [DataMember(Name = "staffOrganisationAssociations")]
        public List<PersonAssociatedEmails> AssociatedEmails { get; set; }
    }
}
