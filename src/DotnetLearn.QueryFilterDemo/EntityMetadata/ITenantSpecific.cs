namespace DotnetLearn.QueryFilterDemo.EntityMetadata
{
    /// <summary>
    /// Interface for entities that are associated with a single tenant
    /// </summary>
    public interface ITenantSpecific
    {
        int TenantId { get; }
    }
}