using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace academic_staff_updater
{
    public class AcademicStaff
    {
        public string PureId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public AcademicStaff() { }


        public AcademicStaff(string pureId, string title, string firstName, string lastName, string email = "")
        {
            PureId = pureId;
            Title = title;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

    }
}
