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
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
        }

        public async Task<List<Salida>> GetSalidas()
        {
            return await _context.Salidas.ToListAsync();
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
