namespace Atlas.Shared.Infrastructure.Module;

public sealed class ServiceProviderNotSetException : Exception
{
    public ServiceProviderNotSetException() : base("Service provider not set.") { }
}
