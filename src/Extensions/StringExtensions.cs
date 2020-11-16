using System;

namespace AuthorizationForOcelot.Extensions
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
    }
}
