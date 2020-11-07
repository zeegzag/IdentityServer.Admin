﻿using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Skoruba.IdentityServer4.Admin.EntityFramework.Extensions.Common;
using Skoruba.IdentityServer4.Admin.EntityFramework.Extensions.Enums;
using Skoruba.IdentityServer4.Admin.EntityFramework.Extensions.Extensions;
using Skoruba.IdentityServer4.Admin.EntityFramework.Interfaces;
using Skoruba.IdentityServer4.Admin.EntityFramework.Repositories.Interfaces;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Repositories
{
    public class ApiScopeRepository<TDbContext> : IApiScopeRepository
        where TDbContext : DbContext, IAdminConfigurationDbContext
    {
        protected readonly TDbContext DbContext;

        public bool AutoSaveChanges { get; set; } = true;

        public ApiScopeRepository(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual async Task<bool> CanInsertApiScopeAsync(ApiScope apiScope)
        {
            if (apiScope.Id == 0)
            {
                var existsWithSameName = await DbContext.ApiScopes.Where(x => x.Name == apiScope.Name).SingleOrDefaultAsync();
                return existsWithSameName == null;
            }
            else
            {
                var existsWithSameName = await DbContext.ApiScopes.Where(x => x.Name == apiScope.Name && x.Id != apiScope.Id).SingleOrDefaultAsync();
                return existsWithSameName == null;
            }
        }

        public virtual async Task<PagedList<ApiScope>> GetApiScopesAsync(int page = 1, int pageSize = 10)
        {
            var pagedList = new PagedList<ApiScope>();

            var apiScopes = await DbContext.ApiScopes
                .PageBy(x => x.Name, page, pageSize).ToListAsync();

            pagedList.Data.AddRange(apiScopes);
            pagedList.TotalCount = await DbContext.ApiScopes.CountAsync();
            pagedList.PageSize = pageSize;

            return pagedList;
        }

        public virtual Task<ApiScope> GetApiScopeAsync(int apiScopeId)
        {
            return DbContext.ApiScopes
                .Include(x => x.UserClaims)
                .Where(x => x.Id == apiScopeId)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        /// <summary>
        /// Add new api scope
        /// </summary>
        /// <param name="apiResourceId"></param>
        /// <param name="apiScope"></param>
        /// <returns>This method return new api scope id</returns>
        public virtual async Task<int> AddApiScopeAsync(ApiScope apiScope)
        {
            DbContext.ApiScopes.Add(apiScope);

            await AutoSaveChangesAsync();

            return apiScope.Id;
        }

        private async Task RemoveApiScopeClaimsAsync(ApiScope apiScope)
        {
            //Remove old api scope claims
            var apiScopeClaims = await DbContext.ApiScopeClaims.Where(x => x.Scope.Id == apiScope.Id).ToListAsync();
            DbContext.ApiScopeClaims.RemoveRange(apiScopeClaims);
        }

        public virtual async Task<int> UpdateApiScopeAsync(ApiScope apiScope)
        {
            //Remove old relations
            await RemoveApiScopeClaimsAsync(apiScope);

            //Update with new data
            DbContext.ApiScopes.Update(apiScope);

            return await AutoSaveChangesAsync();
        }

        public virtual async Task<int> DeleteApiScopeAsync(ApiScope apiScope)
        {
            var apiScopeToDelete = await DbContext.ApiScopes.Where(x => x.Id == apiScope.Id).SingleOrDefaultAsync();
            DbContext.ApiScopes.Remove(apiScopeToDelete);

            return await AutoSaveChangesAsync();
        }

        protected virtual async Task<int> AutoSaveChangesAsync()
        {
            return AutoSaveChanges ? await DbContext.SaveChangesAsync() : (int)SavedStatus.WillBeSavedExplicitly;
        }

        public virtual async Task<int> SaveAllChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }
    }
}