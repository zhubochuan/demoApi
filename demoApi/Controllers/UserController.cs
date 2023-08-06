using AutoMapper;
using demoApi.DTO;
using demoApi.Model;
using demoApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demoApi.Controllers
{
    [Route("api/demoApi")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly IUserRepository _db;
        private readonly IMapper _mapper;
        public UserController(IUserRepository db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        //get
        [HttpGet("getUsers")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            IEnumerable<User> userList = await _db.GetAll();
            return Ok(_mapper.Map<List<UserDTO>>(userList));
        }

        //post
        [HttpPost("addUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public  async Task<ActionResult<IEnumerable<UserDTO>>> AddUser([FromBody] UserDTO userDTO)
        {
            if (userDTO == null) {
                return BadRequest(userDTO);
            }
            if (userDTO.Id > 0) { 
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            User user = _mapper.Map<User>(userDTO);
            //User user = new()
            //{
            //  Name = userDTO.Name,
            //  Password = userDTO.Password,
            //};
            await _db.Create(user);
            return Ok(user);
        }
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model) {
            var loginRes = await _db.Login(model);
            if (loginRes.User == null || string.IsNullOrEmpty(loginRes.Token)) {
                return BadRequest(new { message = "user credient not found" });   
            }
            return Ok(loginRes);
        }
    }
}
