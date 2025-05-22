using System.ComponentModel;
using System.Data;

namespace SOLTEC.Core.Extensions;

/// <summary>
/// Provides extension methods for converting collections to DataTable.
/// </summary>
/// <example>
/// <![CDATA[
/// // Assume a class MyEntity { public int Id { get; set; } public string Name { get; set; } }
/// var list = new List<MyEntity>
/// {
///     new MyEntity { Id = 1, Name = "Alice" },
///     new MyEntity { Id = 2, Name = "Bob" }
/// };
/// DataTable table = list.ToDataTable();
/// ]]>
/// </example>
public static class EnumerableExtensions
{
    /// <summary>
    /// Converts the specified collection to a <see cref="DataTable"/>, using property names as column names.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="data">The collection to convert.</param>
    /// <returns>
    /// A <see cref="DataTable"/> containing rows for each item in <paramref name="data"/>,
    /// with columns matching public properties of <typeparamref name="T"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is null.</exception>
    /// <example>
    /// <![CDATA[
    /// var names = new[] { "Alice", "Bob" };
    /// DataTable dt = names.ToDataTable();
    /// ]]>
    /// </example>
    public static DataTable ToDataTable<T>(this IEnumerable<T> data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var _properties = TypeDescriptor.GetProperties(typeof(T));
        var _table = new DataTable();

        foreach (PropertyDescriptor _prop in _properties)
        {
            var _columnType = Nullable.GetUnderlyingType(_prop.PropertyType) ?? _prop.PropertyType;
            _table.Columns.Add(_prop.Name, _columnType);
        }

        foreach (var _item in data)
        {
            var _row = _table.NewRow();
            foreach (PropertyDescriptor _prop in _properties)
            {
                _row[_prop.Name] = _prop.GetValue(_item) ?? DBNull.Value;
            }
            _table.Rows.Add(_row);
        }

        return _table;
    }
}
