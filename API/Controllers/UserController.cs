using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Models.DTO;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _data;
        public UserController(UserService dataFromService)
        {
            _data = dataFromService;
        }

        [HttpPost("AddUsers")]
        public bool AddUser(CreateAccountDTO UserToAdd)
        {
            return _data.AddUser(UserToAdd);
        }

        [HttpPost("Login")]

        public IActionResult Login([FromBody] LoginDTO User)
        {
            return _data.Login(User);
        }

        //UPDATE USER
        // [HttpPut("UpdateUser")]

        // public bool UpdateUser(int id, string username)
        // {
        //     return _data.UpdateUser(id, username);
        // }

        [HttpDelete("DeleteUser/{userToDelete}")]

        public bool DeleteUser(int userToDelete)
        {
            return _data.DeleteUser(userToDelete);
        }

        [HttpGet("GetAllUsers")]
        public IEnumerable<User> GetAllUsers()
        {
            return _data.GetAllUsers();
        }
    }
}