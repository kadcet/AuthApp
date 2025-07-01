using AuthApp.Dal.Contexts;
using AuthApp.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthApp.Bll.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == username))
                return false;

            var hashedPassword = HashPassword(password);

            var user = new User
            {
                UserName = username,
                PasswordHash = hashedPassword
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }


        private byte[] HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            return sha256.ComputeHash(bytes);
        }

        private bool VerifyPassword(string enteredPassword, byte[] storedHash)
        {
            var enteredHash = HashPassword(enteredPassword);

            if (enteredHash.Length != storedHash.Length)
                return false;

            for (int i = 0; i < enteredHash.Length; i++)
            {
                if (enteredHash[i] != storedHash[i])
                    return false;
            }
            return true;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
                return false;

            if (!VerifyPassword(password, user.PasswordHash))
                return false;

            return true;
        }
    }

}

