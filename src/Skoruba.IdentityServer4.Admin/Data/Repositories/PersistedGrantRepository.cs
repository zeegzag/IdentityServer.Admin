﻿using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Skoruba.IdentityServer4.Admin.Data.DbContexts;
using Skoruba.IdentityServer4.Admin.Data.Entities;
using Skoruba.IdentityServer4.Admin.Helpers;
using Skoruba.IdentityServer4.Admin.ViewModels.Common;
using Skoruba.IdentityServer4.Admin.ViewModels.Enums;

namespace Skoruba.IdentityServer4.Admin.Data.Repositories
{
    public class PersistedGrantRepository : IPersistedGrantRepository
    {
        private readonly AdminDbContext _dbContext;

        public bool AutoSaveChanges { get; set; } = true;

        public PersistedGrantRepository(AdminDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<PersistedGrantDataView>> GetPersitedGrantsByUsers(int page = 1, int pageSize = 10)
        {
            var pagedList = new PagedList<PersistedGrantDataView>();

            var persistedGrantByUsers = (from pe in _dbContext.PersistedGrants
                                         join us in _dbContext.Users on Convert.ToInt32(pe.SubjectId) equals us.Id into per
                                         from us in per.DefaultIfEmpty()
                                         select new PersistedGrantDataView { SubjectId = pe.SubjectId, SubjectName = us == null ? string.Empty : us.UserName })
                                        .GroupBy(x => new { x.SubjectId })
                                        .Select(grp => grp.First());

            var persistedGrantsData = await persistedGrantByUsers.PageBy(x => x.SubjectId, page, pageSize).ToListAsync();
            var persistedGrantsDataCount = await persistedGrantByUsers.CountAsync();

            pagedList.Data.AddRange(persistedGrantsData);
            pagedList.TotalCount = persistedGrantsDataCount;
            pagedList.PageSize = pageSize;

            return pagedList;
        }

        public async Task<PagedList<PersistedGrant>> GetPersitedGrantsByUser(string subjectId, int page = 1, int pageSize = 10)
        {
            var pagedList = new PagedList<PersistedGrant>();

            var persistedGrantsData = await _dbContext.PersistedGrants.Where(x => x.SubjectId == subjectId).Select(x => new PersistedGrant()
            {
                SubjectId = x.SubjectId,
                Type = x.Type,
                Key = x.Key,
                ClientId = x.ClientId,
                Data = x.Data,
                Expiration = x.Expiration,
                CreationTime = x.CreationTime
            }).PageBy(x=> x.SubjectId, page, pageSize).ToListAsync();

            var persistedGrantsCount = await _dbContext.PersistedGrants.Where(x => x.SubjectId == subjectId).CountAsync();

            pagedList.Data.AddRange(persistedGrantsData);
            pagedList.TotalCount = persistedGrantsCount;
            pagedList.PageSize = pageSize;

            return pagedList;
        }

        public async Task<PersistedGrant> GetPersitedGrantAsync(string key)
        {
            var persistedGrant = await _dbContext.PersistedGrants.SingleOrDefaultAsync(x => x.Key == key);

            return persistedGrant;
        }

        public async Task<int> DeletePersistedGrantAsync(string key)
        {
            var persistedGrant = await _dbContext.PersistedGrants.Where(x => x.Key == key).SingleOrDefaultAsync();

            _dbContext.PersistedGrants.Remove(persistedGrant);

            return await AutoSaveChangesAsync();
        }

        public async Task<bool> ExistsPersistedGrantsAsync(string subjectId)
        {
            var exists = await _dbContext.PersistedGrants.AnyAsync(x => x.SubjectId == subjectId);

            return exists;
        }

        public async Task<int> DeletePersistedGrantsAsync(int userId)
        {
            var grants = await _dbContext.PersistedGrants.Where(x => x.SubjectId == userId.ToString()).ToListAsync();

            _dbContext.RemoveRange(grants);

            return await AutoSaveChangesAsync();
        }

        private async Task<int> AutoSaveChangesAsync()
        {
            return AutoSaveChanges ? await _dbContext.SaveChangesAsync() : (int)SavedStatus.WillBeSavedExplicitly;
        }

        public async Task<int> SaveAllChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}