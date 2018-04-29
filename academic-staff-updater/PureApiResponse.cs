using System;
using System.Linq;

namespace academic_staff_updater
{

    public abstract class PureApiResponse
    {
        public int count;
        public NavigationLink[] navigationLink;
        private Array items;

        public bool MorePages
        {
            get
            {
                if (navigationLink != null)
                {
                    return navigationLink.Any(x => x.Ref == "next");
                }
                else
                {
                    return false;
                }
            } 
        }

        public Array Items { get => items; set => items = value; }

        public class NavigationLink
        {
            public string Ref;
            public string href;
        } 

        public class Value
        {
            public string value;
        }
    }
}
