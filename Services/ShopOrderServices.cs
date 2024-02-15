using Microsoft.EntityFrameworkCore;
using StockControl.Models;
using StockControl.Data;
using System.Diagnostics;

namespace StockControl.Services
{
    public class ShopOrderServices
    {
        private readonly StockControlContext _context;

        public ShopOrderServices(StockControlContext context)
        {
            _context = context;
        }

        public List<Salida> GetSalidaList()
        {
            return _context.Salidas.ToList();
        }

        public List<Planner> GetPlans()
        {
            return _context.Planners.ToList();
        }

        public async Task<List<Planner>> GetShopOrder(int shopOrder)
        {
            var result = await _context.Planners.Where(p => p.ShopOrder == shopOrder).ToListAsync();
            return result;
        }

        public List<Salida> GetPartNumberByDate(DateTime date)
        {
            var result = _context.Salidas.Where(p => p.FechaRegistro.Date == date).ToList();
            return result;
        }

        public void Save(List<Registrohistorico> registrohistoricos)
        {
            try
            {
                foreach(var item in  registrohistoricos)
                {
                    _context.Registrohistoricos.Add(item);
                }
                _context.SaveChanges();
            }
            catch(Exception ex) 
            {
                Debug.WriteLine($"Error al guardar{ex}");
            }
        }
    }
}