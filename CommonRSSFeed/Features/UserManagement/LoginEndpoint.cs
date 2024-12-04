using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CommonRSSFeed.DB;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;

namespace CommonRSSFeed.Features.UserManagement
{
    public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
    {
        private readonly AppDBContext _context;

        public LoginEndpoint(AppDBContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Post("auth/login");
            AllowAnonymous();
        }
        public override async Task<LoginResponse> ExecuteAsync(LoginRequest req, CancellationToken ct)
        {
            var user = await _context.AppUsers.FirstOrDefaultAsync(a => a.Email == req.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.Password))
            {
                ThrowError("Incorrect username or password.", StatusCodes.Status404NotFound);
            }

            var jwt = JwtBearer.CreateToken(options =>
            {
                options.SigningKey = Config["JWTSecret"];
                options.User.Claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
                options.User.Claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.Name));
                options.User.Roles.Add(user.Role);
                options.ExpireAt = DateTime.UtcNow.AddDays(1);
            });

            return new LoginResponse(jwt, user.Email);
        }
    }

    public record LoginRequest(string Email, string Password);

    public record LoginResponse(string JWT, string Email);
}
