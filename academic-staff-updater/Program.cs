using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zengenti.Contensis.Management;
using Zengenti.Contensis.Management.Workflow.Approval;
using Zengenti.Data;

namespace academic_staff_updater
{
    class Program : BaseHelper
    {
        public static ManagementClient contensisClient;

        static void Main(string[] args)
        {
            contensisClient = ManagementClient.Create(
                clientRootUrl,
                clientId,
                clientSharedSecret
            );

            // Get the project
            var project = contensisClient.Projects.Get(targetProject);

            AcademicStaff exampleStaff = GetExample();

            string result = AddAcademicStaff(project, exampleStaff);
            if (result == "")
            {
                Console.WriteLine("Seemed to go well");
            }
            else
            {
                Console.WriteLine("We got an error");
                Console.WriteLine(result);

            }
        }

        static string AddAcademicStaff(Project project, AcademicStaff staff)
        {
            var resultMessage = "";
            try
            {
                var newEntry = project.Entries.New(targetContentType);
                newEntry.Set("pureId", staff.PureId);
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

        static AcademicStaff GetExample()
        {
            return new AcademicStaff
            {
                PureId = "9999",
                Title = "Dr",
                FirstName = "Dave",
                LastName = "Lister",
                Email = "email@example.com",
            };
        }
    }
}
