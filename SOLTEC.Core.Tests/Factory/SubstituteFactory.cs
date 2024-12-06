using SOLTEC.MQApiBusiness.Message.Infrastructure.Repositories;
using NSubstitute;

namespace SOLTEC.Core.Tests.Factory;

public static class SubstituteFactory 
{
    public static MessagesRepositorySql MessagesRepositorySql() 
    {
        return Substitute.For<MessagesRepositorySql>(new object?[] { null });
    }
}
