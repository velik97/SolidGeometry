using System;

namespace Util
{
    public static class StringExtensions
    {
        public static string GetOrdinalForm(this int number)
        {
            int abs = Math.Abs(number);
            string suffix = "-th";
            
            if (abs == 1)
            {
                suffix = "-st";
            }
            else if (abs == 2)
            {
                suffix = "-nd";
            }
            else if (abs == 3)
            {
                suffix = "-rd";
            }

            return number + suffix;
        }
    }
}