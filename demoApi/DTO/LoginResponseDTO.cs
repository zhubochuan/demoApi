using demoApi.Model;

namespace demoApi.DTO
{
    public class LoginResponseDTO
    {
       
        public string Token { get; set; }
        public User User { get; set; }
    }
}
