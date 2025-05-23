namespace SOLTEC.Core.Extensions;

/// <summary>
/// Provides extension methods for arrays.
/// </summary>
/// <example>
/// <![CDATA[
/// // Adding an item to the end
/// var _original = new[] { 1, 2, 3 };
/// var _extended = _original.Add(4);
/// // Adding an item to the beginning
/// var _prepended = _original.Add(0, true);
/// ]]>
/// </example>
public static class ArrayExtensions
{
    /// <summary>
    /// Adds an element to the end or beginning of the specified array.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="target">The array to which the item will be added.</param>
    /// <param name="item">The item to add to the array.</param>
    /// <param name="prepend">If true, adds the item at the beginning; otherwise, at the end.</param>
    /// <returns>A new array containing the original elements plus the added item.</returns>
    /// <example>
    /// <![CDATA[
    /// var _original = new[] { "a", "b" };
    /// var _newArr = _original.Add("c");
    /// var _prependedArr = _original.Add("x", true);
    /// ]]>
    /// </example>
    public static T[] Add<T>(this T[] target, T item, bool prepend = false)
    {
        ArgumentNullException.ThrowIfNull(target);

        var _result = new T[target.Length + 1];
        if (!prepend)
        {
            Array.Copy(target, 0, _result, 0, target.Length);
            _result[target.Length] = item;
        }
        else
        {
            _result[0] = item;
            Array.Copy(target, 0, _result, 1, target.Length);
        }

        return _result;
    }
}
