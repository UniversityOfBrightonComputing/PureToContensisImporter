using System.Runtime.Serialization;

namespace academic_staff_updater.PureContracts
{
    [DataContract]
    public class ResearchRendering
    {
        [DataMember(Name = "format")]
        public string Format { get; set; }
        [DataMember(Name = "value")]
        public string Html { get; set; }
    }
}
