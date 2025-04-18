﻿using KeyBoard.DTOs.AuthenDTOs;
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
        Task<string> SignInAsync(SignInDTO signin);
    }
}
