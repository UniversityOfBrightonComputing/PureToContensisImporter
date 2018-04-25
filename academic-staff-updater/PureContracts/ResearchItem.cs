using System.Collections.Generic;
using System.Runtime.Serialization;

namespace academic_staff_updater.PureContracts
{
    [DataContract]
    public class ResearchItem
    {
        [DataMember(Name = "rendering")]
        public List<ResearchRendering> Renderings { get; set; }
        [DataMember(Name = "publicationStatuses")]
        public List<ReseachPublicationStatus> Statuses { get; set; }
    }
}
