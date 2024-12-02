namespace SOLTEC.Core.Enums;

/// <summary>
/// Defines the mapping between a column in a SqliteBulkCopy instance's data source and a column in the instance's destination table.
/// </summary>
public enum FbColumnTypesExtended
{
    // Summary:
    //     A signed integer.
    Integer = 1,
    // Summary:
    //     A floating point value.
    Real = 2,
    // Summary:
    //     A text string.
    Text = 3,
    // Summary:
    //     A blob of data.
    Blob = 4,
    // Summary:
    //     A date column
    Date = 5
}
