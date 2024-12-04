using CommonRSSFeed.DB;
using FastEndpoints;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CommonRSSFeed.Features.UserManagement
{
    public class RegisterEndpoint : Endpoint<RegisterRequest,RegisterResponse>
    {
        private readonly AppDBContext _context;

        public RegisterEndpoint(AppDBContext context)
        {
            _context = context;
        }


        public override void Configure()
        {
            Post("auth/register");
            AllowAnonymous();
        }
        public override async Task<RegisterResponse> ExecuteAsync(RegisterRequest req, CancellationToken ct)
        {
            var exisitingUser = _context.AppUsers.Any(a => a.Email == req.Email);
            if (exisitingUser)
                ThrowError("Email already in use.", StatusCodes.Status409Conflict);
            var newUser = new AppUser {Name = req.Name, Email = req.Email, Password = req.Password, Role = req.Role };
            await _context.AppUsers.AddAsync(newUser);

            await _context.SaveChangesAsync();

            var res = new RegisterResponse() { Id = newUser.Id, Email = newUser.Email, Name = newUser.Name};
            return res;
        }
    }

    public class RegisterRequest
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }

    }

    public class RegisterResponse
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public required string Name { get; set; }

    }
}
