namespace eBoardAPI.Extensions;

public static class CSharpExtension
{
    public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> items)
    {
            foreach (var item in items)
                collection.Remove(item);
    }
}