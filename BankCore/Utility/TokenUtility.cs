﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace BankCore.Utility
{
    public class TokenUtility
    {
        public static bool Validate(string userEmail, IHeaderDictionary httpHeaders)
        {
            var token = ParseFromHeaders(httpHeaders);
            if (token == null)
            {
                return false;
            }

            var securityToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            var decodedEmail = securityToken.Payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata"]as string;

            var isUserAuthorized = decodedEmail == userEmail;
            var tokenExpired = securityToken.ValidTo < DateTime.UtcNow;

            return isUserAuthorized && !tokenExpired;
        }

        private static string ParseFromHeaders(IHeaderDictionary httpHeaders)
        {
            var success = httpHeaders.TryGetValue("Authorization", out var authorizationString);
            if (!success)
            {
                return null;
            }

            var token = authorizationString.ToString().Split(' ')[1];
            return token;
        }
    }
}
