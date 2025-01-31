﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CommonRSSFeed
{
    public static class Extensions
    {
        public static TokenUser ToTokenUser(this ClaimsPrincipal user)
        {
            var userId = Guid.Parse(user.FindFirstValue(JwtRegisteredClaimNames.Sub));
            var name = user.FindFirstValue(JwtRegisteredClaimNames.Name);

            return new TokenUser() { Id = userId, Name = name };

        }
    }

    public class TokenUser
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
