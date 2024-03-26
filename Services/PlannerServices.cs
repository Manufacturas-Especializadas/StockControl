using Microsoft.EntityFrameworkCore;
using StockControl.Models;
using System.Diagnostics;

namespace StockControl.Services
{
    public class PlannerServices
    {
        private readonly StockControlContext _context;

        public PlannerServices(StockControlContext context) 
        {
            _context = context;
        }

        public async Task<List<Models.Planner>> GetPlanners()
        {
            return await _context.Planners.ToListAsync();
        }

        public async Task<Planner> GetPlannerAsync(int plannerId)
        {
            var planner = await _context.Planners.FirstOrDefaultAsync(p => p.Id == plannerId);

            return planner;
        }

        public async Task<Planner> GetPlannerByID(int id)
        {
            var planner = await _context.Planners.FirstOrDefaultAsync(p => p.Id == id);

            return planner;
        }

        public async Task<int> GetTotalPagesAsync(int PageSize)
        {
            var totalItems = await _context.Planners.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            return totalPages;
        }

        public async Task<List<Planner>> GetPagedPlanners(int PageNumber, int PageSize)
        {
            try
            {
                var skip = PageNumber == 1 ? 0 : (PageNumber - 1) * PageSize;
                return _context.Planners.OrderBy(s => s.Id).Skip(skip).Take(PageSize).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return new List<Planner>();
        }


        public async Task<Planner> CREATE(Planner planner)
        {
            if(planner != null)
            {
                await _context.Planners.AddAsync(planner);
                await _context.SaveChangesAsync();
                return planner;
            }
            else
            {
                return new Planner();
            }
        }

        public async Task<Planner> UPDATE(int plannerId, Planner planner)
        {
            var plannerDB = await _context.Planners.FindAsync(plannerId);

            if (plannerDB != null)
            {
                // Actualiza solo las propiedades que deseas cambiar
                _context.Entry(plannerDB).CurrentValues.SetValues(planner);

                await _context.SaveChangesAsync();
            }

            return plannerDB;
        }


        public async Task DELETE(int id)
        {
            var plannerDB = await _context.Planners.FindAsync(id);
            if (plannerDB != null)
            {
                _context.Remove(plannerDB);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"No se encontró ninguna entidad Planner con el ID {id}.");
            }
        }
    }
}
