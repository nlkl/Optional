namespace Optional.Sandbox
{
    public static class Nullability
    {
        public static string GiveNotNull(Option<string> a)
        {
            return a.ValueOr((string?)null);
        }

        public static string? GiveNull(Option<string> a)
        {
            return a.ValueOr((string?)null);
        }

        public static string GiveValue(Option<string> a)
        {
            return a.ValueOr("a");
        }
    }
}