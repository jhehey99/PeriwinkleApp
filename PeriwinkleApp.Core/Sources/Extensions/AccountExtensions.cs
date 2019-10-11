namespace PeriwinkleApp.Core.Sources.Extensions
{
    public static class AccountExtensions
    {
        public static string GenderString (this char? gender)
        {
            if (gender is 'm' || gender is 'M')
                return "Male";
            else if (gender is 'f' || gender is 'F')
                return "Female";
            
            return null;
        }
    }
}
