using demoApi.DTO;

namespace demoApi
{
    //fake DB
    public static class Store
    {
        public static List<UserDTO> userList = new List<UserDTO> {
             new UserDTO { Id = 1, Name="andy", Password="123"},
             new UserDTO { Id = 2, Name="bob", Password="456"}
        };
    }
}
