using Application.Common.Models;
using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ICreatorIdentityService
    {
        Task<Result<AuthTokenHandler>> GetAuthTokenAsync(int id, DateTime? expires = null);
        Task<Creator> GetAsync(int id);
        Task<Creator> GetAsync(string username);
        Task<Result<int>> CreateAsync(Creator creator);
        Task<IResult> UpdateAsync(int id, Creator creator);
        Task<IResult> UpdateWalletAsync(int id, Creator creator);
        Task<IResult> UpdateProfilePicture(int id, string filename);
        Task<Result<AuthTokenHandler>> LoginAsync(string username, string password);
        Task SetPasswordAsync(int id, string password);
    }
}
