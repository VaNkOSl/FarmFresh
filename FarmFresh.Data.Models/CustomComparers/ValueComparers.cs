using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FarmFresh.Data.Models.CustomComparers
{
    public static class ValueComparers
    {
        public static ValueComparer<List<T>> CollectionComparer<T>()
            => new(
                (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c != null ? c.ToList() : new List<T>());
    }
}
