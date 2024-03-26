using Microsoft.EntityFrameworkCore;
using StockControl.Models;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

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

        public async Task<int> GetTotalPagesAsync(int PageSize)
        {
            var totalItems = await _context.Registrohistoricos.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            return totalPages;
        }

        public async Task<List<Registrohistorico>> GetPagedHistorial(int PageNumber, int PageSize)
        {
            try
            {
                var skip = PageNumber == 1 ? 0 : (PageNumber - 1) * PageSize;
                return _context.Registrohistoricos.OrderBy(s => s.Id).Skip(skip).Take(PageSize).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return new List<Registrohistorico>();
        }

        public async Task<List<Registrohistorico>> Filters(int? ShopOrder, string Partnumber)
        {
            var query = _context.Registrohistoricos.AsQueryable();

            if (ShopOrder.HasValue)
            {
                query = query.Where(s => s.ShopOrderId == ShopOrder.Value);
            }

            if (!string.IsNullOrEmpty(Partnumber))
            {
                query = query.Where(p => p.PartNumber.Contains(Partnumber));
            }

            return await query.ToListAsync();
        }
    }
}