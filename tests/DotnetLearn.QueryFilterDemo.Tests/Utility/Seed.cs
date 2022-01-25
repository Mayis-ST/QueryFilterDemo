using System.Threading.Tasks;

namespace DotnetLearn.QueryFilterDemo.Tests.Utility;

using System;
using Entities;

internal static class Seed
{
    private static readonly Tenant[] tenants =
    {
        new("Interdimensional Plumbing"),
        new("HVAC Arachnid")
    };

    private static readonly Customer[] customers =
    {
        new("Rick", "Sanchez") { Tenant = tenants[0] },
        new("Morty", "Smith") { Tenant = tenants[0] },
        new("Quentin", "Beck") { Tenant = tenants[1] },
        new("Eddie", "Brock") { Tenant = tenants[1] }
    };

    private static readonly Order[] orders =
    {
        new(1000M)
        {
            Customer = customers[0],
            Tenant = customers[0].Tenant
        },
        new(2000M)
        {
            Customer = customers[0],
            Tenant = customers[0].Tenant
        },
        new(3000M)
        {
            Customer = customers[1],
            Tenant = customers[1].Tenant
        },
        new(4000M)
        {
            Customer = customers[1],
            Tenant = customers[1].Tenant
        },
        new(5000M)
        {
            Customer = customers[2],
            Tenant = customers[2].Tenant
        },
        new(6000M)
        {
            Customer = customers[2],
            Tenant = customers[2].Tenant
        },
        new(7000M)
        {
            Customer = customers[3],
            Tenant = customers[3].Tenant
        },
        new(8000M)
        {
            Customer = customers[3],
            Tenant = customers[3].Tenant
        }
    };

    internal static Task WithSampleTenants(QueryFilterContext context) => SeedWithEntities(context, tenants);

    internal static Task WithSampleCustomers(QueryFilterContext context) => SeedWithEntities(context, tenants, customers);

    internal static Task WithSampleOrders(QueryFilterContext context) => SeedWithEntities(context, tenants, customers, orders);

    private static async Task SeedWithEntities(QueryFilterContext context, params object[][] entities)
    {
        foreach(var entityInstances in entities)
            context.AddRange(entityInstances);

        await context.SaveChangesAsync();
    }
}