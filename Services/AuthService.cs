namespace QuickPay.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using QuickPay.Data;
    using QuickPay.Models.Domain;
    using QuickPay.Models.DTO;
    using QuickPay.Services.Interface;
    using System.ComponentModel.DataAnnotations;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly QuickPayDbContext dbcontext;

        public AuthService(IConfiguration configuration, QuickPayDbContext dbcontext)
        {
            _configuration = configuration;
            this.dbcontext = dbcontext;
        }

        public string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }


        public string GenerateJwtToken(Users user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<RegisterResponseDto> RegisterAysnc(RegisterDto registerDto)
        {
            var Transaction = await dbcontext.Database.BeginTransactionAsync();

            try
            {
                var user = new Users
                {
                    Name = registerDto.Name,
                    Email = registerDto.Email,
                    Password = HashPassword(registerDto.Password),
                    CreatedAt = DateTime.UtcNow
                };

                var existingUser = await dbcontext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == registerDto.Email.ToLower());
                if (existingUser != null)
                {
                    throw new ValidationException("Email Already Registered");
                }
                else
                {
                    dbcontext.Users.Add(user);

                    await dbcontext.SaveChangesAsync();
                    var wallet = new Wallet
                    {
                        UserId = user.Id,
                        Balance = 0,
                        LastUpdatedAt = DateTime.UtcNow
                    };

                    dbcontext.Wallets.Add(wallet);
                    await dbcontext.SaveChangesAsync();
                    await Transaction.CommitAsync();
                    return new RegisterResponseDto
                    {
                        UserId = user.Id,
                        Email = user.Email,
                        Name = user.Name
                    };
                }

            }
            catch (Exception)
            {
                await Transaction.RollbackAsync();
                throw;
            }

        }


        public async Task<LoginResponeDto> LoginAsync(LoginDto dto)
        {
            var user = await dbcontext
                .Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower());
            if (user == null)
                throw new Exception("Invalid email or password");
            var hashedPassword = HashPassword(dto.Password);
            if (user.Password != hashedPassword)
                throw new Exception("Invalid email or password");
            var token = GenerateJwtToken(user);
            return new LoginResponeDto
            {
                Token = token,
                Email = user.Email,
                UserId = user.Id
            };


        }

    }
}
