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
    }
}
