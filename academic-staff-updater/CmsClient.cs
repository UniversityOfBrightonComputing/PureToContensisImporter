using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zengenti.Contensis.Management;
using Zengenti.Contensis.Management.Workflow.Approval;

namespace academic_staff_updater
{
    class CmsClient
    {
        private ManagementClient Client;
        private Project Project;

        public string CmsAcademicStaffName = "academicStaff";

        public CmsClient(ManagementClient client, Project project)
        {
            Client = client;
            Project = project;
        }

        public bool AddAcademicStaff(AcademicStaff staff)
        {
            var succeeded = false;
            try
            {
                var newEntry = Project.Entries.New(CmsAcademicStaffName);
                newEntry.Set("pureId", staff.Id);
                newEntry.Set("title", staff.Title);
                newEntry.Set("firstName", staff.FirstName);
                newEntry.Set("lastName", staff.LastName);
                newEntry.Set("email", staff.Email);

                newEntry.Save();
                newEntry.Workflow.Submit("Automated entry submission");
                newEntry.Workflow.Approve();

                succeeded = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return succeeded;
        }

        public bool AddAcademicStaff(List<AcademicStaff> staffList)
        {
            var succeeded = false;
            foreach(var staff in staffList)
            {
                succeeded = AddAcademicStaff(staff);
                if (!succeeded) break;
            }

            return succeeded;
        }

        public bool DeleteAcademicStaff()
        {
            var succeeded = false;
            try
            {
                var staffList = Project.Entries.List(CmsAcademicStaffName);
                foreach (var s in staffList.Items)
                {
                    var firstName = s.Get<string>("firstName");
                    var lastName = s.Get<string>("lastName");
                    
                    s.Delete();
                    Console.WriteLine($"Deleted {firstName} {lastName}");
                }
                succeeded = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return succeeded;
        }
    }
}
