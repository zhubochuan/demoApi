using AutoMapper;
using demoApi.DTO;
using demoApi.Model;
namespace demoApi
{
    public class MappingConfig:Profile
    {
        public MappingConfig() {
           CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
