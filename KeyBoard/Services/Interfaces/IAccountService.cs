using KeyBoard.DTOs.AuthenDTOs;
using KeyBoard.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace KeyBoard.Services.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="signup">User registration details (username, email, password, etc.).</param>
        /// <returns>
        /// Returns an <see cref="IdentityResult"/> indicating success or failure.
        /// </returns>
        Task<IdentityResult> SignUpAsync(SignUpDTO signup);


        /// <summary>
        /// Authenticates a user and returns a JWT token if successful.
        /// </summary>
        /// <param name="signin">User login details (email, password, etc.).</param>
        /// <returns>
        /// Returns a JWT token as a string if successful, otherwise returns an empty string.
        /// </returns>
        Task<TokenResponseDTO> SignInAsync(SignInDTO signin);


        /// <summary>
        /// Find a user and return SignInDTO if successful
        /// </summary>
        /// <param name="userID">User id</param>
        /// <returns>
        /// Return SignInDTO of user
        /// </returns> 
        Task<UserProfileDTO> FindUserById(string userID);

        /// <summary>
        /// Update a user 
        /// </summary>
        /// <param name="userid">User id</param>
        /// <param name="user">Userprofile DTO</param>
        /// <returns>
        /// Return UserProfileDTO
        /// </returns>
        Task<UserProfileDTO> UpdateUserById(string userid,UserProfileDTO user);

        /// <summary>
        /// Check if a password is correct for a specific user.
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="password">Password to verify</param>
        /// <returns>True if correct, false otherwise</returns>
        Task<ServiceResult> CheckPasswordAsync(string userId, string password);

    }
}
