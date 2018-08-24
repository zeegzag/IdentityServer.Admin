﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Dtos.Identity;

namespace Skoruba.IdentityServer4.Admin.BusinessLogic.Services
{
    public interface IIdentityService<TIdentityDbContext, TUserDto, TUserDtoKey, TRoleDto, TRoleDtoKey, TClaimDtoKey, TUserKey, TRoleKey, TClaimKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        where TIdentityDbContext : IdentityUserContext<TUser, TKey, TUserClaim, TUserLogin, TUserToken>
        where TUserDto : UserDto<TUserDtoKey>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
        where TUserToken : IdentityUserToken<TKey>
        where TRoleDto : RoleDto<TRoleDtoKey>
    {
        Task<bool> ExistsUserAsync(string userId);

        Task<bool> ExistsRoleAsync(string roleId);

        Task<UsersDto<TUserDto, TUserDtoKey>> GetUsersAsync(string search, int page = 1, int pageSize = 10);

        Task<RolesDto<TRoleDto, TRoleDtoKey>> GetRolesAsync(string search, int page = 1, int pageSize = 10);

        Task<IdentityResult> CreateRoleAsync(TRoleDto role);

        Task<TRoleDto> GetRoleAsync(TRoleDto role);

        Task<List<TRoleDto>> GetRolesAsync();

        Task<IdentityResult> UpdateRoleAsync(TRoleDto role);

        Task<TUserDto> GetUserAsync(string userId);

        Task<IdentityResult> CreateUserAsync(TUserDto user);

        Task<IdentityResult> UpdateUserAsync(TUserDto user);

        Task<IdentityResult> DeleteUserAsync(string userId, TUserDto user);

        Task<IdentityResult> CreateUserRoleAsync(UserRolesDto<TRoleDto, TUserDtoKey, TRoleDtoKey> role);

        Task<UserRolesDto<TRoleDto, TUserDtoKey, TRoleDtoKey>> BuildUserRolesViewModel(TUserDtoKey id, int? page);

        Task<UserRolesDto<TRoleDto, TUserDtoKey, TRoleDtoKey>> GetUserRolesAsync(string userId, int page = 1,
            int pageSize = 10);

        Task<IdentityResult> DeleteUserRoleAsync(UserRolesDto<TRoleDto, TUserDtoKey, TRoleDtoKey> role);

        Task<UserClaimsDto<TUserDtoKey, TClaimDtoKey>> GetUserClaimsAsync(string userId, int page = 1,
            int pageSize = 10);

        Task<UserClaimsDto<TUserDtoKey, TClaimDtoKey>> GetUserClaimAsync(string userId, string claimId);

        Task<IdentityResult> CreateUserClaimsAsync(UserClaimsDto<TUserDtoKey, TClaimDtoKey> claimsDto);

        Task<int> DeleteUserClaimsAsync(UserClaimsDto<TUserDtoKey, TClaimDtoKey> claim);

        Task<UserProvidersDto<TUserDtoKey>> GetUserProvidersAsync(string userId);

        TUserDtoKey ConvertUserDtoKeyFromString(string id);

        Task<IdentityResult> DeleteUserProvidersAsync(UserProviderDto<TUserDtoKey> provider);

        Task<UserProviderDto<TUserDtoKey>> GetUserProviderAsync(string userId, string providerKey);

        Task<IdentityResult> UserChangePasswordAsync(UserChangePasswordDto<TUserDtoKey> userPassword);

        Task<IdentityResult> CreateRoleClaimsAsync(RoleClaimsDto<TRoleDtoKey, TClaimDtoKey> claimsDto);

        Task<RoleClaimsDto<TRoleDtoKey, TClaimDtoKey>> GetRoleClaimsAsync(string roleId, int page = 1, int pageSize = 10);

        Task<RoleClaimsDto<TRoleDtoKey, TClaimDtoKey>> GetRoleClaimAsync(string roleId, string claimId);

        Task<int> DeleteRoleClaimsAsync(RoleClaimsDto<TRoleDtoKey, TClaimDtoKey> role);

        Task<IdentityResult> DeleteRoleAsync(RoleDto<TRoleDtoKey> role);
    }
}