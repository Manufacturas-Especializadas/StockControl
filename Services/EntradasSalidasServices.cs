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

        public async Task<List<ResultadoEntradasSalidas>> GetEntradasSalidas()
        {
            // Obtener todas las entradas y salidas de la base de datos
            
            var entradas = await _context.Entradas.ToListAsync();
            var salidas = await _context.Salidas.ToListAsync();

            // Agrupar las entradas por código y fecha, sumando la cantidad (conteo)
            var entradasAgrupadas = entradas.GroupBy(e => new { e.Codigo, FechaRegistro = e.FechaRegistro.Date })
                                            .Select(e => new ResultadoEntradasSalidas
                                            {
                                                Codigo = e.Key.Codigo,
                                                CantidadEntrada = e.Sum(x => x.Conteo),
                                                CantidadSalida = 0,
                                                CantidadFinal = 0,
                                                FechaEntrada = e.Key.FechaRegistro.Date
                                            })
                                            .OrderBy(e => e.FechaEntrada)
                                            .ToList();

            // Agrupar las salidas por código y fecha, sumando la cantidad (conteo)
            // Agrupar las salidas por código y fecha, sumando la cantidad (conteo)
            var salidasAgrupadas = salidas.GroupBy(s => new { s.Codigo, FechaRegistro = s.FechaRegistro.Date })
                .Select(s => new
                {
                    Key = new KeyValuePair<string, DateTime>(s.Key.Codigo, s.Key.FechaRegistro.Date),
                    Sum = s.Sum(x => x.Conteo)
                })
                .ToDictionary(s => s.Key, s => s.Sum);

            // Iterar sobre las entradas agrupadas para calcular la cantidad de salidas y final
            foreach (var entrada in entradasAgrupadas)
            {
                var codigoFecha = new CodigoFecha { Codigo = entrada.Codigo, FechaRegistro = entrada.FechaEntrada };
                // Intentar obtener la cantidad de salidas correspondiente a la entrada actual
                if (salidasAgrupadas.TryGetValue(new KeyValuePair<string, DateTime>(codigoFecha.Codigo, codigoFecha.FechaRegistro),
                    out var cantidadSalida))
                {
                    // Asignar la cantidad de salidas y calcular la cantidad final
                    entrada.CantidadSalida = cantidadSalida;
                    entrada.CantidadFinal = entrada.CantidadEntrada - cantidadSalida;
                    entrada.FechaSalida = cantidadSalida > 0 ? (DateTime?)codigoFecha.FechaRegistro : null;
                }
                else
                {
                    // No hay salidas para esta entrada, establecer FechaSalida como null
                    entrada.FechaSalida = null;
                }
            }

            // Retornar las entradas agrupadas y calculadas
            return entradasAgrupadas;
            

        }

    }
}
