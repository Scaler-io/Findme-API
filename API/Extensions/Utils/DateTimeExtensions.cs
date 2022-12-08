namespace API.Extensions.Utils
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime dob)
        {
            var today = DateTime.UtcNow;
            var age = today.AddYears(-dob.Year).Year;
            if (dob.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
