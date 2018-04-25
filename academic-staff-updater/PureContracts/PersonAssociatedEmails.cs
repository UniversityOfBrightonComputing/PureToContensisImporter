using System.Collections.Generic;
using System.Runtime.Serialization;

namespace academic_staff_updater.PureContracts
{
    [DataContract]
    public class PersonAssociatedEmails
    {
        [DataMember(Name = "emails")]
        public List<PersonValue> Emails { get; set; }
    }
}
