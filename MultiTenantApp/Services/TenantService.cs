    using Microsoft.AspNetCore.Http;
    using System.Linq;

namespace MultiTenantApp.Services
{
    public class TenantService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetTenantId()
        {
            var tenantId = _httpContextAccessor.HttpContext?.Request.Headers["x-tenant-id"];

            if (string.IsNullOrEmpty(tenantId))
            {
                throw new UnauthorizedAccessException("Tenant ID is missing in request headers.");
            }

            return tenantId;
        }
    }
}
