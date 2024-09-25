namespace WeatherApp.Extentions
{
    public static class StringExtention
    {
        public static string FirstToUpper(this string? str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str ?? string.Empty; 
            }

            return $"{str[0].ToString().ToUpper()}{str[1..]}";
        }
    }
}