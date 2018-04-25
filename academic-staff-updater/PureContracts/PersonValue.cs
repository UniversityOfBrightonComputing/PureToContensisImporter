using System.Collections.Generic;
using System.Runtime.Serialization;

namespace academic_staff_updater.PureContracts
{
    [DataContract]
    public class PersonValue
    {
        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}
