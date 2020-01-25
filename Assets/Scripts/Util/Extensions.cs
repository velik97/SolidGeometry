using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Util
{
    public static class Extensions
    {
        public static bool HasSameItems<T>([NotNull] this IEnumerable<T> enumerable1, IEnumerable<T> enumerable2)
        {
            Dictionary<T, int> occurrences = new Dictionary<T, int>();
            int nullsCount = 0;
            foreach (T s in enumerable1)
            {
                if (s == null)
                {
                    nullsCount++;
                }
                else if (occurrences.ContainsKey(s))
                {
                    occurrences[s]++;
                }
                else
                {
                    occurrences.Add(s, 1);
                }
            }
            
            foreach (T s in enumerable2)
            {
                if (s == null)
                {
                    nullsCount--;
                }
                else if (occurrences.ContainsKey(s))
                {
                    occurrences[s]--;
                }
                else
                {
                    return false;
                }
            }
            
            return nullsCount == 0 && occurrences.Values.All(c => c == 0);
        }
    }
}