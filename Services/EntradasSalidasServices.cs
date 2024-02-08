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
            var entradas = await _context.Entradas.ToListAsync();
            var salidas = await _context.Salidas.ToListAsync();

            var historialInventario = new List<Historialinventario>();

            // Agrupar las entradas por número de parte (código)
            var entradasAgrupadas = entradas.GroupBy(e => e.Codigo)
                                            .Select(g => new
                                            {
                                                Codigo = g.Key,
                                                Entradas = g.OrderBy(e => e.FechaRegistro).ToList()
                                            })
                                            .ToList();

            // Agrupar las salidas por número de parte (código)
            var salidasAgrupadas = salidas.GroupBy(s => s.Codigo)
                                            .Select(g => new
                                            {
                                                Codigo = g.Key,
                                                Salidas = g.OrderBy(s => s.FechaRegistro).ToList()
                                            })
                                            .ToList();

            // Iterar sobre cada grupo de entradas
            foreach (var entradaGroup in entradasAgrupadas)
            {
                var codigo = entradaGroup.Codigo;
                var entradasGrupo = entradaGroup.Entradas;

                // Obtener el grupo de salidas correspondiente al código
                var salidasGrupo = salidasAgrupadas.FirstOrDefault(s => s.Codigo == codigo)?.Salidas ?? new List<Salida>();

                // Inicializar la cantidad final para el código actual
                var cantidadFinal = 0;

                // Iterar sobre cada entrada del grupo
                foreach (var entrada in entradasGrupo)
                {
                    // Sumar la cantidad de la entrada actual
                    cantidadFinal += entrada.Conteo;

                    // Restar la cantidad de las salidas que ocurrieron antes de esta entrada
                    cantidadFinal -= salidasGrupo.Where(s => s.FechaRegistro.Date <= entrada.FechaRegistro.Date)
                                                 .Sum(s => s.Conteo);

                    // Crear una instancia de HistorialInventario y agregarla a la lista
                    var historial = new Historialinventario
                    {
                        Codigo = codigo,
                        CantidadEntrada = entrada.Conteo,
                        FechaEntrada = entrada.FechaRegistro.Date,
                        CantidadSalida = 0, // Inicialmente no hay salidas para esta entrada
                        FechaSalida = null, // Inicialmente no hay fecha de salida
                        CantidadFinal = cantidadFinal
                    };
                    historialInventario.Add(historial);
                }
            }

            // Guardar el historial de inventario en la base de datos
            await _context.Historialinventarios.AddRangeAsync(historialInventario);
            await _context.SaveChangesAsync();

            // Retornar el historial de inventario
            return historialInventario;
        }

    }
}
