using System;
using System.Linq;

namespace academic_staff_updater.Pure
{

    public abstract class PureApiResponse
    {
        public int count;
        public NavigationLink[] navigationLink;

        public bool MorePages()
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
