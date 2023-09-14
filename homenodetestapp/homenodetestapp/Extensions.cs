using System.Text;

namespace homenodetestapp
{
    internal static class Extensions
    {
        public static string WaitToReceive(this StreamReader streamReader, string s)
        {
            var builder = new StringBuilder();

            while (true)
            {
                int i = streamReader.Read();

                builder.Append((char)i);

                if (builder.ToString().EndsWith(s))
                {
                    return builder.ToString();
                }
            }
        }
    }
}