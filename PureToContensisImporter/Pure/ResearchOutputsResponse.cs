namespace PureToContensisImporter.Pure
{
    class ResearchOutputsResponse : PureApiResponse
    {
        #pragma warning disable CS0649
        public Output[] items;
        #pragma warning restore  CS0649
        public class Output
        {
            #pragma warning disable CS0649
            public Value[] rendering;
            #pragma warning restore CS0649
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
