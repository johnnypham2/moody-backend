using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Models;
using API.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace API.Services
{
    public class UserService : ControllerBase
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        //help function for user exist
        public bool DoesUserExist(string username)
        {
            return _context.Users.SingleOrDefault(u => u.Username == username) != null;
        }

        //ADDING USER LOGIC
        public bool AddUser(CreateAccountDTO userToAdd)
        {
            bool result = false;
            if (!DoesUserExist(userToAdd.Username))
            {
                User newUser = new User();


                var newHashedPassword = HashPassword(userToAdd.Password);

                newUser.Id = userToAdd.Id;
                newUser.Username = userToAdd.Username;
                newUser.Salt = newHashedPassword.Salt;
                newUser.Hash = newHashedPassword.Hash;

                _context.Add(newUser);

                result = _context.SaveChanges() != 0;

            }
            return result;
        }

        public PasswordDTO HashPassword(string password)
        {
            PasswordDTO newHashedPassword = new PasswordDTO();

            byte[] SaltBytes = new byte[64];
            var provider = new RNGCryptoServiceProvider();
            provider.GetNonZeroBytes(SaltBytes);
            var Salt = Convert.ToBase64String(SaltBytes);
            var Rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, SaltBytes, 10000);
            var Hash = Convert.ToBase64String(Rfc2898DeriveBytes.GetBytes(256));

            newHashedPassword.Salt = Salt;
            newHashedPassword.Hash = Hash;

            return newHashedPassword;

        }

        public bool VerifyUserPassword(string? Password, string?StoredHash, string? StoredSalt)
        {
            var SaltBytes = Convert.FromBase64String(StoredSalt);
            var Rfc2898DeriveBytes = new Rfc2898DeriveBytes(Password, SaltBytes, 10000);
            var newHash = Convert.ToBase64String(Rfc2898DeriveBytes.GetBytes(256));
            return newHash == StoredHash;
        }

        public IActionResult Login(LoginDTO user)
        {
            IActionResult Result = Unauthorized();
            if(DoesUserExist(user.UserName))
            {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("reallylongsupersuperkeysuperSecretKey@345"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:5295",
                audience: "https://localhost:5295",
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            Result = Ok(new  { Token = tokenString }); 
            }
            return Result;
        }

        private User GetUser(int id)
        {
            return _context.Users.SingleOrDefault(user => user.Id == id);
        }

        // public bool UpdateUser(int id, string username)
        // {
        //     User foundUser = GetUser(id);
        //     bool result = false;
        //     if(foundUser != null)
        //     {
        //         foundUser.Username = username;
        //         _context.Update<User>(foundUser);
        //         result = _context.SaveChanges() != 0;
        //     }
        //     return result;
        // }

        public bool DeleteUser(int userToDelete)
        {
            User foundUser = GetUser(userToDelete);
            bool result = false;

            if (foundUser != null)
            {
                _context.Remove<User>(foundUser);
                result = _context.SaveChanges() != 0;
            }

            return result;
        }
    }
}