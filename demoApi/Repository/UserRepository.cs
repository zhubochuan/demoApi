using demoApi.DTO;
using demoApi.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace demoApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private string secretKey;
        public UserRepository(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            secretKey = config.GetValue<string>("ApiSettings:Secret");
        }
        public async Task Create(User entity)
        {
         await _context.Users.AddAsync(entity);
            await Save();
        }

        public async Task<List<User>> GetAll(Expression<Func<User,bool>> filter = null)
        {
            IQueryable<User> query = _context.Users;
            if (filter != null)
            {
                query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _context.Users.FirstOrDefault(e => e.Name == loginRequestDTO.Name
            && loginRequestDTO.Password == e.Password
            );
            if (user == null) {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }

            //token
            var generateToken = new JwtSecurityTokenHandler();
            var code = Encoding.ASCII.GetBytes(secretKey);
            var tokenDes = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = new(new SymmetricSecurityKey(code), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = generateToken.CreateToken(tokenDes);//token
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = generateToken.WriteToken(token),
                User = user
            };
            return loginResponseDTO;
        }

        public async Task<User> Register(RegResponseDTO registerDTO)
        {
            User user = new()
            {
                Name = registerDTO.Name,
                Password = registerDTO.Password,
            };

            _context.Users.Add(user);
          await  _context.SaveChangesAsync();
            return user;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
