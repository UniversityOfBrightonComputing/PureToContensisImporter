using System;
using System.Collections.Generic;
using academic_staff_updater.Contensis;
using academic_staff_updater.Pure;

namespace academic_staff_updater
{
    class Program
    {

        static void Main(string[] args)
        {
            // 1. Get list of current staff from Pure
            var pureClient = PureClientFactory.GetClient();
            var persons = pureClient.GetPersons(5);

            // 2. Construct AcademicStaff objects for each member
            var staff = ConvertPersonsToAcademicStaff(persons);

            // 3. Delete current Academic Staff in CMS
            var cmsClient = Contensis.CmsClientFactory.GetClient();
            bool deleteSuccess = cmsClient.DeleteAcademicStaff();
            bool addSuccess = false;
            
            // 4. Add the AcademicStaff to CMS
            if(deleteSuccess)
            {
               addSuccess = cmsClient.AddAcademicStaff(staff);   
            }

            if (addSuccess)
            {
                Console.WriteLine("Old CMS Staff deleted and new data from Pure uploaded");
            }
            else
            {
                Console.WriteLine("There were issues - check the console output, hopefully we wrote out some of the error messages");
            }

        }

        /// <summary>
        /// Converts a list from Pure PersonsResponse.Person to CMS AcademicStaff
        /// </summary>
        /// <param name="persons"></param>
        /// <returns></returns>
        static List<AcademicStaff> ConvertPersonsToAcademicStaff(List<PersonsResponse.Person> persons)
        {
            var staff = new List<AcademicStaff>();
            foreach (var person in persons)
            {
                var member = new AcademicStaff
                {
                    Id = person.Id,
                    Title = person.Title,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Email = person.Email
                };
                staff.Add(member);
            }
            return staff;
        }
    }
}
