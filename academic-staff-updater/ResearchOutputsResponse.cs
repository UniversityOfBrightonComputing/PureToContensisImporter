namespace academic_staff_updater
{
    class ResearchOutputsResponse : PureApiResponse
    {
        public new Output[] items;

        public class Output
        {
            public Value[] rendering;

            public string Html
            {
                get
                {
                    if (rendering != null && rendering.Length > 0)
                    {
                        return rendering[0].value;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

        }

    }
}
