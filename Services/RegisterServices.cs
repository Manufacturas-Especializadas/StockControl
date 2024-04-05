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
            var totalItems = await _context.Registers.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            return totalPages;
        }

        public async Task<List<Register>> GetPagedUsers(int PageNumber, int PageSize)
        {
            try
            {
                var skip = PageNumber == 1 ? 0 : (PageNumber - 1) * PageSize;
                return await _context.Registers
                    .Include(r => r.FkRolNavigation)
                    .OrderBy(s => s.Id)
                    .Skip(skip)
                    .Take(PageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return new List<Register>();
        }

        public async Task<Register> UPDATE(int registerID,Register register)
        {
            var registerToUpdate = await _context.Registers
                                    .Include(r => r.FkRolNavigation)
                                    .FirstOrDefaultAsync(r => r.Id == registerID);

            if (registerToUpdate != null)
            {
                // Actualiza los campos de la entidad Register
                registerToUpdate.Email = register.Email;
                registerToUpdate.Password = register.Password;

                // Asocia la entidad Rol
                registerToUpdate.FkRolNavigation = register.FkRolNavigation;

                await _context.SaveChangesAsync();
            }

            return registerToUpdate;
        }

        public async Task<Register> CREATE(Register register)
        {
            if (register != null)
            {
               await _context.Registers.AddAsync(register);
               await _context.SaveChangesAsync();
               return register;
            }
            else
            {
                return new Register();
            }
        }

        public async Task<bool> Authentication(Register register)
        {
            try
            {
                var user = await _context.Registers.FirstOrDefaultAsync(u => u.Email == register.Email);

                if(user != null && user.Password == user.Password)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error durante la autenticación{ex}");
                return false;
            }
        }

    }
}