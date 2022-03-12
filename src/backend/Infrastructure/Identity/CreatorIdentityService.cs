using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Ardalis.GuardClauses;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class CreatorIdentityService : ICreatorIdentityService
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IDateTime _dateTime;
        private readonly IAuthTokenService _authTokenService;

        public CreatorIdentityService(IApplicationDbContext dbContext, IDateTime dateTime, IAuthTokenService authTokenService)
        {
            _dbContext = dbContext;
            _dateTime = dateTime;
            _authTokenService = authTokenService;
        }

        public async Task<Result<AuthTokenHandler>> GetAuthTokenAsync(int id, DateTime? expires = null)
        {
            var creator = await GetAsync(id);

            if (creator == null)
                return await Result<AuthTokenHandler>.FailAsync();

            var nameIdentifer = $"{creator.Id}";

            var accountClaims = new Dictionary<string, string> { { ClaimTypes.NameIdentifier, nameIdentifer } };
            if (expires.HasValue) { expires = DateTime.SpecifyKind(expires.Value, DateTimeKind.Utc); }
            var token = _authTokenService.GenerateToken(accountClaims, expires);
            return await Result<AuthTokenHandler>.SuccessAsync(token);
        }

        public async Task<Creator> GetAsync(int id)
        {
            Guard.Against.NegativeOrZero(id, nameof(id));

            var creator = await _dbContext.Creators.AsQueryable().FirstOrDefaultAsync(u => u.Id == id && u.IsAccountValid);

            return creator;
        }

        public async Task<Creator> GetAsync(string username)
        {
            Guard.Against.NullOrEmpty(username, nameof(username));

            var creator = await _dbContext.Creators.AsQueryable().FirstOrDefaultAsync(u => u.UsernameNormalize == username.ToNormalize() && u.IsAccountValid);

            return creator;
        }

        public async Task<Result<int>> CreateAsync(Creator creator)
        {
            var currentCreator = await GetAsync(creator.Username);

            if (currentCreator != null)
                return await Result<int>.FailAsync("Username already used.");

            creator.Username = creator.Username.ToLower();
            creator.UsernameNormalize = creator.Username.ToNormalize();
            creator.DateAccountAcquired = _dateTime.UtcNow;
            creator.DateRegistered = _dateTime.UtcNow;
            creator.Salt = RandomExtensions.GenerateSalt(32);
            creator.AccountSecret = AESExtensions.Encrypt(creator.AccountSecret, creator.Salt);

            _dbContext.Creators.Add(creator);
            _dbContext.SaveChangesAsync().Wait();

            return await Result<int>.SuccessAsync(creator.Id);
        }

        public async Task<IResult> UpdateAsync(int id, Creator creator)
        {
            var currentCreator = await GetAsync(id);

            if (currentCreator == null)
                return await Result.FailAsync("Account not found.");

            currentCreator.Name = creator.Name;
            currentCreator.Bio = creator.Bio;

            _dbContext.Creators.Update(currentCreator);
            _dbContext.SaveChangesAsync().Wait();

            return await Result.SuccessAsync();
        }

        public async Task<IResult> UpdateWalletAsync(int id, Creator creator)
        {
            var currentCreator = await GetAsync(id);

            if (currentCreator == null)
                return await Result.FailAsync("Account not found.");

            currentCreator.IsAccountValid = creator.IsAccountValid;
            currentCreator.AccountXAddress = creator.AccountXAddress;
            currentCreator.AccountSecret = AESExtensions.Encrypt(creator.AccountSecret, creator.Salt);
            currentCreator.AccountClassicAddress = creator.AccountClassicAddress;
            currentCreator.AccountAddress = creator.AccountAddress;
            currentCreator.DateAccountAcquired = creator.DateAccountAcquired;

            _dbContext.Creators.Update(currentCreator);
            _dbContext.SaveChangesAsync().Wait();

            return await Result.SuccessAsync();
        }

        public async Task<IResult> UpdateProfilePicture(int id, string filename)
        {
            var currentCreator = await GetAsync(id);

            if (currentCreator == null)
                return await Result.FailAsync("Account not found.");

            currentCreator.ProfilePictureFilename = filename;

            _dbContext.Creators.Update(currentCreator);
            _dbContext.SaveChangesAsync().Wait();

            return await Result.SuccessAsync();
        }

        public async Task<Result<AuthTokenHandler>> LoginAsync(string username, string password)
        {
            var creator = await _dbContext.Creators.AsQueryable().FirstOrDefaultAsync(u => u.UsernameNormalize == username.ToNormalize());

            if (creator != null)
            {
                var userPassword = await _dbContext.CreatorPasswords.AsQueryable().FirstOrDefaultAsync(x => !x.IsDeleted && x.CreatorId == creator.Id);
                if (userPassword != null)
                {
                    var passwordHash = HasherExtensions.Hash(password, userPassword.Salt, SHA512.Create());

                    if (userPassword.Digest == passwordHash.Digest)
                    {
                        if (!creator.IsAccountValid)
                            return await Result<AuthTokenHandler>.FailAsync("Due to XRP NFTDEV limitation, your account wallet has been expired. To continue, you may create a new account.");

                        var authTokenResult = await GetAuthTokenAsync(creator.Id);
                        if (authTokenResult.Succeeded)
                        {
                            return authTokenResult;
                        }
                    }
                }
            }

            return await Result<AuthTokenHandler>.FailAsync("You have entered an invalid username or password.");
        }

        public async Task SetPasswordAsync(int id, string password)
        {
            var passwordHash = HasherExtensions.Hash(password, 64, SHA512.Create());

            var currentPassword = await _dbContext.CreatorPasswords.AsQueryable().FirstOrDefaultAsync(x => x.CreatorId == id && !x.IsDeleted);

            if (currentPassword != null)
            {
                currentPassword.IsDeleted = true;
                _dbContext.CreatorPasswords.Update(currentPassword);
                _dbContext.SaveChangesAsync().Wait();
            }

            var newPassword = new CreatorPassword
            {
                CreatorId = id,
                Salt = passwordHash.Salt,
                Digest = passwordHash.Digest
            };

            _dbContext.CreatorPasswords.Add(newPassword);
            _dbContext.SaveChangesAsync().Wait();
        }
    }

}
