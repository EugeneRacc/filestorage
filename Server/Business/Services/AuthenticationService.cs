﻿using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class AuthenticationService
    {
        private readonly IUserService _userService;
        public AuthenticationService(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<UserModel> GetUserByToken(string authHeader)
        {
            var handler = new JwtSecurityTokenHandler();
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;

            var id = tokenS.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var user = await _userService.GetByIdAsync(int.Parse(id));
            if(user == null)
            {
                throw new ArgumentNullException($"User isn't exist {user}");
            }
            return user;
        }
        public async Task<int> GetUserIdByToken(string authHeader)
        {
            var handler = new JwtSecurityTokenHandler();
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;

            var id = tokenS.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            /*var user = await _userService.GetByIdAsync(int.Parse(id));
            if (user == null)
            {
                throw new ArgumentNullException($"User isn't exist {user}");
            }*/
            int parsed = int.Parse(id);
            return parsed;
        }

    }
}
