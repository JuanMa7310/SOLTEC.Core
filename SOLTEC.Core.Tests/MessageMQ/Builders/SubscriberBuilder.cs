using SOLTEC.Core.DataSources.Interfaces;

namespace SOLTEC.Core.Tests.MessageMQ.Builders;

public class SubscriberBuilder {
    private readonly IDataSource _dataSource;
    private Guid _subscriptorId = DefaultValues.SubscriberGuid;
    private readonly string _creation = DefaultValues.CreationWithTime;
    private string _nameSubscriptor = DefaultValues.SubscriberName;
    private bool _active = true;

    public SubscriberBuilder(IDataSource dataSource) 
    {
        _dataSource = dataSource;
    }

    public void Build() 
    {
        _dataSource.ExecuteDapper<object>(@"
            if (select count(0) from SOLTECMQ.dbo.tbSubscriptors where SubscriptorId = @subscriptorId) = 0
            begin
                insert into dbo.tbSubscriptors (SubscriptorId, [Name], Creation, Active) 
                values (@subscriptorId, @name, @creation, @active)
            end",
            new {
                subscriptorId = _subscriptorId,
                name = _nameSubscriptor,
                creation = _creation,
                active = _active
            });
    }

    public SubscriberBuilder WithSubscriptorId(Guid value) 
    {
        _subscriptorId = value;
        
        return this;
    }

    public SubscriberBuilder WithSubscriptorName(string value) 
    {
        _nameSubscriptor = value;
        
        return this;
    }

    public SubscriberBuilder WithActive(bool value) 
    {
        _active = value;
        
        return this;
    }
}
