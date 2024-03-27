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
using StockControl.Data;
using System.Diagnostics;
using StockControl.Pages;


namespace StockControl.Services
{
    public class EntradasSalidasServices
    {
        private readonly StockControlContext _context;

        public EntradasSalidasServices(StockControlContext context)
        {
            _context = context;
        }

        public class CodigoFecha
        {
            public string Codigo { get; set; }

            public DateTime FechaRegistro { get; set; }

        }

        public List<Entrada> GetEntradas()
        {
            return _context.Entradas.ToList();
        }

        public List<Salida> GetSalidas()
        {
            return _context.Salidas.ToList();
        }

        public async Task<List<Historialinventario>> GetEntradasSalidas()
        {
            try
            {
                // Obtene todas las entradas y salidas de la base de datos
                var entradas = await _context.Entradas.ToListAsync();
                var salidas = await _context.Salidas.ToListAsync();

                // Crea un diccionario para almacenar las salidas agrupadas por código y fecha
                var salidasAgrupadas = salidas
                    .GroupBy(s => new { s.Codigo, FechaRegistro = s.FechaRegistro.Date })
                    .ToDictionary(
                        s => new KeyValuePair<string, DateTime>(s.Key.Codigo, s.Key.FechaRegistro),
                        s => s.Sum(x => x.Conteo)
                    );

                // Crea una lista para almacenar el historial de inventario
                var historialInventario = new List<Historialinventario>();

                // Agrupa las entradas por código y fecha, sumando la cantidad (conteo)
                var entradasAgrupadas = entradas
                    .GroupBy(e => new { e.Codigo, FechaRegistro = e.FechaRegistro.Date })
                    .Select(e => new Historialinventario
                    {
                        Codigo = e.Key.Codigo,
                        CantidadEntrada = e.Sum(x => x.Conteo),
                        FechaEntrada = e.Key.FechaRegistro
                    })
                    .OrderBy(e => e.FechaEntrada)
                    .ToList();

                // Itera sobre las entradas agrupadas para calcular la cantidad de salidas y final
                foreach (var entrada in entradasAgrupadas)
                {
                    // Intenta obtener la cantidad de salidas correspondiente a la entrada actual
                    if (salidasAgrupadas.TryGetValue(new KeyValuePair<string, DateTime>(entrada.Codigo, entrada.FechaEntrada),
                        out var cantidadSalida))
                    {
                        // Asigna la cantidad de salidas y calcular la cantidad final
                        entrada.CantidadSalida = cantidadSalida;
                        entrada.CantidadFinal = entrada.CantidadEntrada - cantidadSalida;

                        // Obtiene la fecha de salida
                        var fechaSalida = salidas
                            .Where(s => s.Codigo == entrada.Codigo && s.FechaRegistro.Date == entrada.FechaEntrada.Date)
                            .Select(s => s.FechaRegistro)
                            .FirstOrDefault();

                        // Asigna la fecha de salida si existe
                        entrada.FechaSalida = fechaSalida;
                    }
                    else
                    {
                        // No hay salidas para esta entrada, establecer cantidad de salida y final como cero
                        entrada.CantidadSalida = 0;
                        entrada.CantidadFinal = entrada.CantidadEntrada;
                        entrada.FechaSalida = null;
                    }

                    // Guarda el historial de inventario en la lista
                    historialInventario.Add(entrada);
                }

                // Guarda el historial de inventario en la base de datos
                await _context.Historialinventarios.AddRangeAsync(historialInventario);
                await _context.SaveChangesAsync();

                // Retorna el historial de inventario
                return historialInventario;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al recuperar los datos{ex.Message}");

                return new List<Historialinventario>();
            }
            
        }

    }
}
