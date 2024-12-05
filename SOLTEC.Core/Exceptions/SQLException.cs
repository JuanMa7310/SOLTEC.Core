using SOLTEC.Core.Enums;

namespace SOLTEC.Core.Exceptions;

public class SQLException : ResultException
{
    public SQLException(SQLErrorEnum reason)
    {
        Key = "SQLException";
        Reason = reason.ToString();
    }
}
