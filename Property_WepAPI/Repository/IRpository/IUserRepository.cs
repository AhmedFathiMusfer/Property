﻿using Property_WepAPI.Models;
using Property_WepAPI.Models.Dto;
using System.Runtime.InteropServices;

namespace Property_WepAPI.Repository.IRpository
{
    public interface IUserRepository
    {
         bool IsUniqueUser(string username);
          Task<TokenDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterationRequestDTO regitsterationRequestDTO);
        Task<TokenDTO> RefreshAccessToken(TokenDTO tokenDTO);
        Task RevokeAccessToken(TokenDTO tokenDTO);
        Task<ForgetPassworedDTO> ForgetPasswored(ForgetPassworedDTO forgetPassworedDTO);
        Task<ForgetPassworedConfirmationDTO> ForgetPassworedConfirmation(ForgetPassworedConfirmationDTO forgetPassworedConfirmationDTO);
        Task<bool> ResetPasswored(ResetPassworedDTO resetPassworedDTO);

    }
}
