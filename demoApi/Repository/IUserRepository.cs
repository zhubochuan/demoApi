using demoApi.DTO;
using demoApi.Model;
using System.Linq.Expressions;

namespace demoApi.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll(Expression<Func<User,bool>> filter = null);
        Task Create(User entity);
        Task Save();
        Task<LoginResponseDTO> Login(LoginRequestDTO login);
        Task<User> Register(RegResponseDTO register);
    }
}
