namespace academic_staff_updater
{
    public class PersonsResponse : PureApiResponse
    {
        public Person[] items;

        public class Person
        {
            public int pureId;
            public Name name;
            public Value[] titles;
            public AssociatedEmail[] staffOrganisationAssociations;

            public int Id
            {
                get
                {
                    return pureId;
                }
            }

            public string FirstName
            {
                get
                {
                    return name.firstName;
                }
            }

            public string LastName
            {
                get
                {
                    return name.lastName;
                }
            }

            public string Title
            {
                get
                {
                    if (titles != null && titles.Length > 0)
                    {
                        return titles[0].value;
                    }
                    return "";
                }
            }

            public string Email
            {
                get
                {
                    if (staffOrganisationAssociations != null)
                    {
                        foreach (var assocEmail in staffOrganisationAssociations)
                        {
                            if (assocEmail.emails != null)
                            {
                                foreach (var email in assocEmail.emails)
                                {
                                    if (email.value != null)
                                    {
                                        return email.value;
                                    }

                                }
                            }
                        }
                    }
                    //default
                    return "";
                }
            }

            public class Name
            {
                public string firstName;
                public string lastName;
            }

            public class AssociatedEmail
            {
                public Value[] emails;
            }
        }

    }
}
