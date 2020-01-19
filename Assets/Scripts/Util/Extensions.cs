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
            foreach (T s in enumerable1) {
                if (occurrences.ContainsKey(s)) {
                    occurrences[s]++;
                } else {
                    occurrences.Add(s, 1);
                }
            }
            foreach (T s in enumerable2) {
                if (occurrences.ContainsKey(s)) {
                    occurrences[s]--;
                } else {
                    return false;
                }
            }
            
            return occurrences.Values.All(c => c == 0);
        }
    }
}