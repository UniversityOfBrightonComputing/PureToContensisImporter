using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zengenti.Contensis.Management;
using Zengenti.Contensis.Management.Workflow.Approval;
using Zengenti.Data;

namespace academic_staff_updater
{
    class Program
    {

        static void Main(string[] args)
        {

            // 1. Get list of current staff from Pure
            // 2. Construct AcademicStaff objects for each member
            // 3. Get list of staff from CMS
            // 4. For each Pure staff member
            // 1. Check if exists in CMS, if not create
            // 2. Mark staff member as "found in import"
            // 3. Update staff member
            // 5. At end for each NOT "found in import" delete from CMS
            //ManagementClient contensisClient = ContensisClientFactory.GetClient();
            //string targetProject = ConfigurationManager.AppSettings["cmsMainProject"];

            //// Get the project
            //var project = contensisClient.Projects.Get(targetProject);

            //AcademicStaff exampleStaff = GetExample();

            //string result = AddAcademicStaff(project, exampleStaff);
            //if (result == "")
            //{
            //    Console.WriteLine("Seemed to go well");
            //}
            //else
            //{
            //    Console.WriteLine("We got an error");
            //    Console.WriteLine(result);

            //}

            //var exampleStaff = GetExample();
            //var pureClient = PureClientFactory.GetClient();
            //Console.WriteLine(pureClient.GetResearchRenderingForStaff(exampleStaff));

            var pureClient = PureClientFactory.GetClient();
            var persons = pureClient.GetPersons(10);
            var staff = GetAcademicStaffFromPersons(persons);

            Console.WriteLine("Breakpoint");

            Console.WriteLine(staff.ToString());

            //List<AcademicStaff> pureStaff = pureClient.GetAcademicStaff();

            //ManagementClient contensisClient = ContensisClientFactory.GetClient();
            //string targetProject = ConfigurationManager.AppSettings["cmsMainProject"];
            //var project = contensisClient.Projects.Get("website");

            //foreach(var staff in pureStaff)
            //{
            //    AddAcademicStaff(project, staff);
            //}



        }

        static List<AcademicStaff> GetAcademicStaffFromPersons(List<PersonsResponse.Person> persons)
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

        static string AddAcademicStaff(Project project, AcademicStaff staff)
        {
            var resultMessage = "";
            try
            {
                var newEntry = project.Entries.New("academicStaff");
                newEntry.Set("pureId", staff.Id);
                newEntry.Set("title", staff.Title);
                newEntry.Set("firstName", staff.FirstName);
                newEntry.Set("lastName", staff.LastName);
                newEntry.Set("email", staff.Email);

                newEntry.Save();
                newEntry.Workflow.Submit("Automated entry submission");
                newEntry.Workflow.Approve();

                //var staffList = project.Entries.List("academicStaff");
                //foreach(var s in staffList.Items)
                //{
                //    var name = s.Get<string>("firstName");
                //    var lastName = s.Get<string>("lastName");
                //    Console.WriteLine(name + " " + lastName);
                //    s.Delete();

                //}


            }
            catch(Exception e)
            {
                resultMessage = e.Message;
            }

            return resultMessage;


        }

        //static AcademicStaff GetExample()
        //{
        //    return new AcademicStaff
        //    {
        //        PureId = "39620",
        //        Title = "Dr",
        //        FirstName = "Dave",
        //        LastName = "Lister",
        //        Email = "email@example.com",
        //    };
        //}
    }
}
