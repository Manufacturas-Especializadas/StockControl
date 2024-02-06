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



    }
}
