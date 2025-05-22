namespace SOLTEC.Core.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IList{T}"/> instances.
/// </summary>
/// <example>
/// <![CDATA[
/// // Null list returns true
/// IList<int> numbers = null;
/// bool result1 = numbers.IsNullOrEmpty(); // true
/// 
/// // Empty list returns true
/// numbers = new List<int>();
/// bool result2 = numbers.IsNullOrEmpty(); // true
/// 
/// // List with elements returns false
/// numbers.Add(5);
/// bool result3 = numbers.IsNullOrEmpty(); // false
/// ]]>
/// </example>
public static class ListExtensions
{
    /// <summary>
    /// Determines whether the specified list is null or contains no elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="collection">The list to check.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="collection"/> is null or has zero elements; otherwise, <c>false</c>.
    /// </returns>
    /// <example>
    /// <![CDATA[
    /// IList<string> names = null;
    /// bool isEmpty = names.IsNullOrEmpty(); // true
    /// names = new List<string>();
    /// isEmpty = names.IsNullOrEmpty(); // true
    /// names.Add("Alice");
    /// isEmpty = names.IsNullOrEmpty(); // false
    /// ]]>
    /// </example>
    public static bool IsNullOrEmpty<T>(this IList<T> collection)
    {
        if (collection == null)
        {
            return true;
        }

        return collection.Count == 0;
    }
}
