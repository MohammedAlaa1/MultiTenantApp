using Microsoft.EntityFrameworkCore.Diagnostics;
using MultiTenantApp.Models;
using MultiTenantApp.Services;

namespace MultiTenantApp.Data
{
    public class TenantSaveChangesInterceptor : SaveChangesInterceptor
    {

        private readonly TenantService _tenantService;

        public TenantSaveChangesInterceptor (TenantService tenantService)
        {
            _tenantService = tenantService;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            var tenantId = _tenantService.GetTenantId();
            var entries = eventData.Context.ChangeTracker.Entries()
                .Where(e => e.Entity is EntityBase);

            foreach (var entry in entries)
            {
                var entity = entry.Entity as EntityBase;

                if (entity.TenantId == null)
                {
                    entity.TenantId = tenantId;
                }
            }
            return result;
        } 

    }
}
