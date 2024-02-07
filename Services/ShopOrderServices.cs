using Microsoft.EntityFrameworkCore;
using StockControl.Models;
using StockControl.Data;

namespace StockControl.Services
{
    public class ShopOrderServices
    {
        private readonly StockControlContext _context;

        public ShopOrderServices(StockControlContext context)
        {
            _context = context;
        }

        public async Task<List<Planner>> GetPlanner()
        {
            return await _context.Planners.ToListAsync();
        }

        public async Task<List<Salida>> GetSalidas()
        {
            return await _context.Salidas.ToListAsync();
        }

        public async Task<List<Planner>> GetShopOrder(int shopOrder)
        {
            var result = await _context.Planners.Where(p => p.ShopOrder == shopOrder).ToListAsync();
            return result;
        }

        public async Task<List<Salida>> GetPartNumberByDate(DateTime? date)
        {
            var result = await _context.Salidas.Where(p => p.FechaRegistro == date).ToListAsync();
            return result;
        }
    }
}
