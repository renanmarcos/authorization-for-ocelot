using System;

namespace AuthorizationForOcelot.SystemExtensions
{
    public static class StringExtensions
    {
        private const StringComparison DefaultComparison = StringComparison.CurrentCultureIgnoreCase;

        public static bool EqualsIgnoringCase(this string value, string another)
        {
            return value.Equals(another, DefaultComparison);
        }

        public static bool ContainsIgnoringCase(this string value, string another)
        {
            return value.Contains(another, DefaultComparison);
        }

        public static bool EndsWithIgnoringCase(this string value, string another)
        {
            return value.EndsWith(another, DefaultComparison);
        }
    }
}
