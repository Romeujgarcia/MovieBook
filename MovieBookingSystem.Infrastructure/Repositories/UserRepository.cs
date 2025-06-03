using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieBookingSystem.Domain.Entities;
using MovieBookingSystem.Domain.Interfaces;
using MovieBookingSystem.Infrastructure.Data;


public class UserRepository : IUserRepository
{
    private readonly MovieBookingDbContext _context;

    public UserRepository(MovieBookingDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
    }

    public async Task<IList<User>> GetAllAsync() // Corrigido para IList<User>
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        return user;
    }

    public async Task<User> UpdateAsync(User user) // Corrigido para retornar User
    {
        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync(); // Salvar as alterações
        return user; // Retornar o usuário atualizado
    }

    public async Task DeleteAsync(User user) // Corrigido para receber User
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(); // Salvar as alterações
    }
}
