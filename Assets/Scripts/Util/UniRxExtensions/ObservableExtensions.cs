using System;
using UniRx;

namespace Util.UniRxExtensions
{
    public static class ObservableExtensions
    {
        public static IObservable<bool> And(this IObservable<bool> property1, IObservable<bool> property2)
        {
            return property1.CombineLatest(property2, (p1, p2) => p1 && p2);
        }
        
        public static IObservable<bool> Or(this IObservable<bool> property1, IObservable<bool> property2)
        {
            return property1.CombineLatest(property2, (p1, p2) => p1 || p2);
        }
        
        public static IObservable<bool> Xor(this IObservable<bool> property1, IObservable<bool> property2)
        {
            return property1.CombineLatest(property2, (p1, p2) => p1 ^ p2);
        }
        
        public static IObservable<bool> Not(this IObservable<bool> property)
        {
            return property.Select(p => !p);
        }

        public static IObservable<T> NotNull<T>(this IObservable<T> source)
        {
            return source.Where(value => value != null);
        }
    }
}