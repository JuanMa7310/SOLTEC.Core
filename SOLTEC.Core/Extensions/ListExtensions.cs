namespace SOLTEC.Core.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IList{T}"/> instances.
/// </summary>
/// <example>
/// <![CDATA[
/// // Null list returns true
/// IList<int> _numbers = null;
/// bool _result1 = _numbers.IsNullOrEmpty(); // true
/// 
/// // Empty list returns true
/// _numbers = new List<int>();
/// bool _result2 = _numbers.IsNullOrEmpty(); // true
/// 
/// // List with elements returns false
/// _numbers.Add(5);
/// bool _result3 = _numbers.IsNullOrEmpty(); // false
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
    /// IList<string> _names = null;
    /// bool _isEmpty = _names.IsNullOrEmpty();   // true
    /// _names = new List<string>();
    /// _isEmpty = _names.IsNullOrEmpty();        // true
    /// names.Add("Alice");
    /// _isEmpty = _names.IsNullOrEmpty();        // false
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
