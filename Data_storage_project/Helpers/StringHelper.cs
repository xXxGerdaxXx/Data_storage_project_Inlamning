namespace Data_storage_project_library.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// Truncates a string to the specified length and appends "..." if truncated.
        /// Ensures no null values are returned.
        /// </summary>
        public static string Truncate(string? text, int maxLength)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty; 

            return text.Length > maxLength ? text.Substring(0, maxLength - 3) + "..." : text;
        }
    }
}
