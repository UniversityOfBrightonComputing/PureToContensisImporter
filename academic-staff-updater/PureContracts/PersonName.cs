using System.Collections.Generic;
using System.Runtime.Serialization;

namespace academic_staff_updater.PureContracts
{
    [DataContract]
    public class PersonName
    {
        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }
        [DataMember(Name = "lasttName")]
        public string LastName { get; set; }
    }
}
