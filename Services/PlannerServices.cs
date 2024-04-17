using Microsoft.EntityFrameworkCore;
using StockControl.Models;
using System.Diagnostics;
using CsvHelper;
using CsvHelper.Configuration;
using System.Data;
using System.Globalization;

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
            var totalItems = await _context.PlannerArchivos.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            return totalPages;
        }

        public async Task<List<PlannerArchivo>> GetPagedPlanners(int PageNumber, int PageSize)
        {
            try
            {
                var skip = PageNumber == 1 ? 0 : (PageNumber - 1) * PageSize;
                return _context.PlannerArchivos.OrderBy(s => s.Id).Skip(skip).Take(PageSize).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return new List<PlannerArchivo>();
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

        public async Task InsertDatabase(DataTable dt)
        {
            try
            {
                if(dt == null || dt.Rows.Count == 0)
                {
                    Debug.WriteLine($"El datatable es nulo o vacio");
                    return;
                }

                foreach (DataRow row in dt.Rows)
                {
                    var plannerFile = new PlannerArchivo
                    {
                        OrdenProduccion = Convert.ToDecimal(row["Orden de Produccion"]),
                        NumeroParte = Convert.ToString(row["Numero de Parte"]),
                        FechaProduccionCarrier = DateTime.ParseExact(row["Fecha de PRODUCCION CARRIER"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture),
                        Cantidad = byte.Parse((string)row["Cantidad Requerida"])
                    };

                    _context.PlannerArchivos.Add(plannerFile);
                }
                
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al insertar datos en la base de datos: {ex.Message}");
            }
        }

        public async Task<List<PlannerArchivo>> Filter(int? ShopOrder, string NumeroParte)
        {
            var query = _context.PlannerArchivos.AsQueryable();

            if(ShopOrder.HasValue)
            {
                query = query.Where(p => p.OrdenProduccion == ShopOrder.Value);
            }

            if (!string.IsNullOrEmpty(NumeroParte))
            {
                query = query.Where(p => p.NumeroParte.Contains(NumeroParte));
            }

            return await query.ToListAsync();
        }
    }
}