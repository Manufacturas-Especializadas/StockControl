using Microsoft.EntityFrameworkCore;
using StockControl.Models;

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

    }
}
