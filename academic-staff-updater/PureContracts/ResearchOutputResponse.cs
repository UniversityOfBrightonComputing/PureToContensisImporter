using System.Runtime.Serialization;
using System.Collections.Generic;


namespace academic_staff_updater.PureContracts
{
    [DataContract]
    public class ResearchOutputResponse
    {
        [DataMember(Name = "items")]
        public List<ResearchItem> Items { get; set; }
        [DataMember(Name = "count")]
        public int Count { get; set; }

    }
}
