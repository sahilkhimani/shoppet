using System.Globalization;

namespace shoppetApi.Helper
{
    public static class HelperMethods
    {
        public static string ApplyTitleCase(string name)
        {
            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            return ti.ToTitleCase(name);
        }

        public static object? ParseId(object id)
        {
            if (id is string stringId)
            {
                if (int.TryParse(stringId, out int intId))
                {
                    if (intId <= 0) return null; 
                    return intId;
                }
                return stringId;
            }
            return id; 
        }
    }
}
