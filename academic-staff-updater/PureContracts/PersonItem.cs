using System.Collections.Generic;
using System.Runtime.Serialization;

namespace academic_staff_updater.PureContracts
{
    [DataContract]
    public class PersonItem
    {
        [DataMember(Name = "uuid")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public PersonName Name { get; set; }
        [DataMember(Name = "titles")]
        public List<PersonValue> Titles { get; set; }
        [DataMember(Name = "staffOrganisationAssociations")]
        public List<PersonAssociatedEmails> AssociatedEmails { get; set; }

        public string FirstName
        {
            get
            {
                return Name.FirstName;
            }
        }

        public string LastName
        {
            get
            {
                return Name.LastName;
            }
        }

        public string Title
        {
            get
            {
                if(Titles != null && Titles.Count > 0)
                {
                    var firstTitle = Titles[0];
                    if(firstTitle != null && firstTitle.Value != null)
                    {
                        return firstTitle.Value;
                    }                   
                }
                return "";
            }
        }

        public string Email
        {
            get
            {
                if(AssociatedEmails != null && AssociatedEmails.Count > 0 && AssociatedEmails[0].Emails.Count > 0)
                {
                    return AssociatedEmails[0].Emails[0].Value;
                }
                else
                {
                    return "";
                }
            }
        }


    }
}
