﻿using System;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Dtos.Common;
using Skoruba.IdentityServer4.Admin.EntityFramework.Entities;
using Skoruba.IdentityServer4.Admin.EntityFramework.Interfaces;

namespace Skoruba.IdentityServer4.Admin.BusinessLogic.Repositories.Interfaces
{
	public interface IPersistedGrantRepository<TDbContext, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
	    where TDbContext : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>, IAdminPersistedGrantIdentityDbContext
	    where TUser : IdentityUser<TKey>
	    where TRole : IdentityRole<TKey>
	    where TKey : IEquatable<TKey>
	    where TUserClaim : IdentityUserClaim<TKey>
	    where TUserRole : IdentityUserRole<TKey>
	    where TUserLogin : IdentityUserLogin<TKey>
	    where TRoleClaim : IdentityRoleClaim<TKey>
	    where TUserToken : IdentityUserToken<TKey>
    {
		Task<PagedList<PersistedGrantDataView>> GetPersitedGrantsByUsers(string search, int page = 1, int pageSize = 10);
		Task<PagedList<PersistedGrant>> GetPersitedGrantsByUser(string subjectId, int page = 1, int pageSize = 10);
	    Task<PersistedGrant> GetPersitedGrantAsync(string key);
	    Task<int> DeletePersistedGrantAsync(string key);
	    Task<int> DeletePersistedGrantsAsync(int userId);
        Task<bool> ExistsPersistedGrantsAsync(string subjectId);
	    Task<int> SaveAllChangesAsync();
	}
}