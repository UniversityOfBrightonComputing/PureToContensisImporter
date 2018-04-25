using System;
using System.Runtime.Serialization;


namespace academic_staff_updater.PureContracts
{
    [DataContract]
    public class ResearchPublicationDate
    {
        [DataMember(Name = "year")]
        public Int32 Year { get; set; }
        [DataMember(Name = "month")]
        public Int32 Month { get; set; }
        [DataMember(Name = "day")]
        public Int32 Day { get; set; }

        public DateTime GetDate()
        {
            return new DateTime(this.Year, this.Month, this.Day);
        }
    }
}
