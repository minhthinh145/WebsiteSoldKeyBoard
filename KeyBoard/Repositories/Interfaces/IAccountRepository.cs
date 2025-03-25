using KeyBoard.DTOs.AuthenDTOs;
using Microsoft.AspNetCore.Identity;

namespace KeyBoard.Repositories.Interfaces
{
    public interface IAccountRepository
    {
       public Task<IdentityResult> SignUpAsync(SignUpDTO signup); 
       public Task<string> SignInAsync(SignInDTO signin);
    }
}
