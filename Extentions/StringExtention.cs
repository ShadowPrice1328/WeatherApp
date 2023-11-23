namespace WeatherApp.Extentions
{
    public static class StringExtention
    {
        public static string FirstToUpper(this string str) =>
            $"{str[0].ToString().ToUpper()}{str.Substring(1)}";
    }
}