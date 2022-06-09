namespace DotNetCoreAngular.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;

            // this checks if the birthday has passed in the curent year
            if (dob.Date > today.AddYears(-age))
                age--;

            return age;
        }
    }
}
