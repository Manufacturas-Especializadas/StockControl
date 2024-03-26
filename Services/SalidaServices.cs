using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using System.IO;
using System.Globalization;
using System.Threading.Tasks;
using System.Data;
using StockControl.Models;
using System.Diagnostics;
using StockControl.Pages;



namespace StockControl.Services
{
    public class SalidaServices
    {
        private readonly StockControlContext _context;

        public SalidaServices(StockControlContext context)
        {
            _context = context;
        }

        public async Task<List<Salida>> GetSalidas()
        {
            return await _context.Salidas.ToListAsync();
        }

        public async Task<int> GetTotalPagesAsync(int PageSize)
        {
            var totalItems = await _context.Salidas.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            return totalPages;
        }

        public async Task<List<Salida>> GetPagedSalidas(int PageNumber, int PageSize)
        {
            try
            {
                var skip = PageNumber == 1 ? 0 : (PageNumber - 1) * PageSize;
                return _context.Salidas.OrderBy(s => s.Id).Skip(skip).Take(PageSize).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return new List<Salida>();
        }

        public async Task InserDataBase(List<string> codigos)
        {
            try
            {
                if (codigos == null || !codigos.Any())
                {
                    Debug.WriteLine("La lista de códigos es nula o vacía.");
                    return;
                }


                foreach (var codigo in codigos)
                {
                    Debug.WriteLine($"Insertando codigo: {codigo}");

                    var salida = new Salida
                    {
                        Codigo = codigo,
                        FechaRegistro = DateTime.Now,
                        Conteo = 1 // Inicia el conteo en 1
                    };
                    _context.Salidas.Add(salida);

                }
                Debug.WriteLine("Guardando cambios en la base de datos...");

                await _context.SaveChangesAsync();

                Debug.WriteLine("Cambios guardados exitosamente");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al insertar datos en la base de datos: {ex.Message}");
            }
        }
    }
}
