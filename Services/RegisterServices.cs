using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.VisualBasic;
using StockControl.Models;
using System.Diagnostics;

namespace StockControl.Services
{
    public class RegisterServices
    {
        private readonly StockControlContext _context;

        public RegisterServices(StockControlContext context)
        {
            _context = context;
        }

        public async Task<List<Rol>>GetRol()
        {
            return await _context.Rols.ToListAsync();
        }

        public async Task<int> GetTotalPagesAsync(int PageSize)
        {
            var totalItems = await _context.Users.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            return totalPages;
        }

        public async Task<List<User>> GetPagedUsers(int PageNumber, int PageSize)
        {
            try
            {
                var skip = PageNumber == 1 ? 0 : (PageNumber - 1) * PageSize;
                return await _context.Users
                    .Include(r => r.Role)
                    .OrderBy(s => s.Id)
                    .Skip(skip)
                    .Take(PageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return new List<User>();
        }

        public async Task<User> UPDATE(int registerID,User register)
        {
            var registerToUpdate = await _context.Users
                                    .Include(r => r.Role)
                                    .FirstOrDefaultAsync(r => r.Id == registerID);

            if (registerToUpdate != null)
            {
                // Actualiza los campos de la entidad Register
                registerToUpdate.Email = register.Email;
                registerToUpdate.Password = register.Password;

                // Asocia la entidad Rol
                registerToUpdate.Role = register.Role;

                await _context.SaveChangesAsync();
            }

            return registerToUpdate;
        }

        public async Task<User> CREATE(User register)
        {
            if (register != null)
            {
               await _context.Users.AddAsync(register);
               await _context.SaveChangesAsync();
               return register;
            }
            else
            {
                return new User();
            }
        }

        public async Task<(bool, string)> Authentication(User register)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == register.Email);

                if (user != null && user.Password == register.Password)
                {
                    // Verifica si el usuario tiene un rol asociado
                    if (user.Role != null)
                    {
                        return (true, user.Role.Name);
                    }
                    else
                    {
                        return (false, null);
                    }
                }
                else
                {
                    return (false, null);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error durante la autenticación: {ex}");
                return (false, null);
            }
        }

    }
}