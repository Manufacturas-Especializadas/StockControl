using Microsoft.EntityFrameworkCore;
using StockControl.Models;

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

        public async Task<Planner> UPDATE(int planner, Planner planner1)
        {
            var plannerDB = await _context.Planners.FindAsync(planner);

            plannerDB.ShopOrder = planner1.ShopOrder;
            plannerDB.Codigo = planner1.Codigo;
            plannerDB.Codigo2 = planner1.Codigo2;
            plannerDB.Codigo3 = planner1.Codigo3;
            plannerDB.Codigo4 = planner1.Codigo4;
            plannerDB.Codigo5 = planner1.Codigo5;
            plannerDB.Codigo6 = planner1.Codigo6;
            plannerDB.Cantidad2 = planner1.Cantidad2;
            plannerDB.Cantidad3 = planner1.Cantidad3;
            plannerDB.Cantidad4 = planner1.Cantidad4;
            plannerDB.Cantidad5 = planner1.Cantidad5;
            plannerDB.Cantidad6 = planner1.Cantidad6;
            plannerDB.Cantidad7 = planner1.Cantidad7;
            plannerDB.Cantidad = planner1.Cantidad;
            plannerDB.Fecha = planner1.Fecha;

            await _context.SaveChangesAsync();
            return plannerDB;
        }


        public async Task DELETE(int id)
        {
            var plannerDB = await _context.Planners.FindAsync(id);
            _context.Remove(plannerDB);
            await _context.SaveChangesAsync();
        }
    }
}
