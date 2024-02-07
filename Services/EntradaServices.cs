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
    public class EntradaServices
    {
        private readonly StockControlContext _context;

        public EntradaServices(StockControlContext context)
        {
            _context = context;
        }

        public async Task<List<Entrada>> GetEntradas()
        {
            return await _context.Entradas.ToListAsync();
        }

        public async Task InsertDataBase(List<string> codigos)
        {
            try
            {
                // Verificar si la lista de códigos es nula o vacía
                if (codigos == null || !codigos.Any())
                {
                    Debug.WriteLine("La lista de códigos es nula o vacía.");
                    return;
                }

                // Iterar sobre la lista de códigos
                foreach (var codigo in codigos)
                {
                    // Imprimir información de depuración
                    Debug.WriteLine($"Insertando codigo: {codigo}");

                    // Crear una nueva entrada con el código actual, la fecha actual y el conteo establecido en 1
                    var entrada = new Entrada
                    {
                        Codigo = codigo,
                        FechaRegistro = DateTime.Now,
                        Conteo = 1 // Siempre establece el conteo en 1
                    };

                    // Agregar la nueva entrada al contexto de base de datos
                    _context.Entradas.Add(entrada);
                }

                // Imprimir información de depuración
                Debug.WriteLine("Guardando cambios en la base de datos...");

                // Guardar los cambios en la base de datos de forma asíncrona
                await _context.SaveChangesAsync();

                // Imprimir información de depuración
                Debug.WriteLine("Cambios guardados exitosamente");
            }
            catch (Exception ex)
            {
                // Capturar y manejar excepciones, imprimir información de depuración en caso de error
                Debug.WriteLine($"Error al insertar datos en la base de datos: {ex.Message}");
            }
        }

    }
}
