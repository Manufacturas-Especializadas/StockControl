using Microsoft.EntityFrameworkCore;
using StockControl.Models;

namespace StockControl.Services
{
    public class HistorialServices
    {
        private readonly StockControlContext _context;

        public HistorialServices(StockControlContext context)
        {
            _context = context;
        }

        public async Task<List<Registrohistorico>> GetRegistrohistoricos()
        {
            return await _context.Registrohistoricos.ToListAsync();
        }
    }
}
