using SOLTEC.Core.Enums;
using SOLTEC.Core.Exceptions;

namespace SOLTEC.Core.DataSources.Exceptions;

public class SQLException : ResultException
{
    public SQLException(SQLErrorEnum reason)
    {
        Key = "SQLException";
        Reason = reason.ToString();
    }
}
