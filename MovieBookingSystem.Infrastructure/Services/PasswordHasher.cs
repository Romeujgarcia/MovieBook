using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MovieBookingSystem.Domain.Interfaces;

namespace MovieBookingSystem.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            // Gera um salt aleatório
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            
            // Deriva uma chave de 256 bits do password usando HMACSHA256
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
                
            // Combina o salt e o hash para armazenamento
            return $"{Convert.ToBase64String(salt)}:{hashed}";
        }
        
        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            // Extrai o salt e o hash armazenados
            var parts = hashedPassword.Split(':');
            if (parts.Length != 2)
            {
                return false;
            }
            
            var salt = Convert.FromBase64String(parts[0]);
            var storedHash = parts[1];
            
            // Computa o hash do password fornecido com o mesmo salt
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: providedPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
                
            // Compara os hashes
            return storedHash == hashed;
        }
    }
}
