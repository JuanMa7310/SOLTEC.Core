using SOLTEC.Core.Enums;
using SOLTEC.Core.Exceptions;

namespace SOLTEC.Core.DataSources.Exceptions;

public class SqlException : ResultException
{
    public SqlException(SqlErrorEnum reason)
    {
        Key = "SQLException";
        Reason = reason.ToString();
    }
}
