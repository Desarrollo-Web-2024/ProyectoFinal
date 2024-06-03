using System.Security.Cryptography;
using System.Text;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

public class LoginDTO {
    public string Username { get; set; }
    public string Password { get; set; }
}

[ApiController]
[Route("user")]
public class UserController(Context.ProjectContext projectContext) : ControllerBase {
    private DbSet<User> Container { get; } = projectContext.Users;
    
    static byte[] GenerateSalt()
    {
        using (var rng = new RNGCryptoServiceProvider())  {
            byte[] salt = new byte[16]; // Adjust the size based on your security requirements
            rng.GetBytes(salt);
            return salt;
        }
    }
    
    static string HashPassword(string password, byte[] salt) {
        using (var sha256 = new SHA256Managed())  {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];

            // Concatenate password and salt
            Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
            Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

            // Hash the concatenated password and salt
            byte[] hashedBytes = sha256.ComputeHash(saltedPassword);

            // Concatenate the salt and hashed password for storage
            byte[] hashedPasswordWithSalt = new byte[hashedBytes.Length + salt.Length];
            Buffer.BlockCopy(salt, 0, hashedPasswordWithSalt, 0, salt.Length);
            Buffer.BlockCopy(hashedBytes, 0, hashedPasswordWithSalt, salt.Length, hashedBytes.Length);

            return Convert.ToBase64String(hashedPasswordWithSalt);
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<User>> Post(User user) {
        using (var sha256 = new SHA256Managed()) {
            byte[] saltBytes = GenerateSalt();
            string hashedPassword = HashPassword(user.PasswordHash, saltBytes);
            
            user.PasswordHash = $"{hashedPassword}.{Convert.ToBase64String(saltBytes)}";
        }
        
        await Container.AddAsync(user);
        await projectContext.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    [HttpPost("/login")]
    public async Task<ActionResult<User>> Login(LoginDTO loginDto)
    {

        // In a real scenario, you would retrieve these values from your database
         var user = await Container.Where(x => x.Username == loginDto.Username).Select(x => x).FirstOrDefaultAsync();
        if (user == null) {
            return BadRequest();
        }

        var retrievedPassWordWithSalt = user.PasswordHash.Split('.');
        
        string storedHashedPassword = retrievedPassWordWithSalt[0];
        string storedSalt = retrievedPassWordWithSalt[1];
        byte[] storedSaltBytes =  Convert.FromBase64String(storedSalt);

        string enteredPasswordHash = HashPassword(loginDto.Password, storedSaltBytes);

        if (enteredPasswordHash == storedHashedPassword) {
            return user;
        }
        return BadRequest();
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> Get(int id) {
        var user = await Container.FindAsync(id);
        if (user == null) {
            return NotFound();
        }

        return user;
    }
}