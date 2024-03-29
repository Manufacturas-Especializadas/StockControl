﻿using Microsoft.EntityFrameworkCore;
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
using System.Linq.Dynamic.Core;

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

        public async Task<int> GetTotalPagesAsync(int PageSize)
        {
            var totalItems = await _context.Entradas.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            return totalPages;
        }

        public async Task<List<Entrada>> GetPagedEntradas(int PageNumber, int PageSize)
        {
            try
            {
                var skip = PageNumber == 1 ? 0 : (PageNumber - 1) * PageSize;
                return _context.Entradas.OrderBy(s => s.Id).Skip(skip).Take(PageSize).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return new List<Entrada>();
        }


        public async Task InsertDataBase(List<string> codigos)
        {
            try
            {
                // Verifica si la lista de códigos es nula o vacía
                if (codigos == null || !codigos.Any())
                {
                    Debug.WriteLine("La lista de códigos es nula o vacía.");
                    return;
                }

                // Itera sobre la lista de códigos
                foreach (var codigo in codigos)
                {
                    Debug.WriteLine($"Insertando codigo: {codigo}");

                    // Crea una nueva entrada con el código actual, la fecha actual y el conteo establecido en 1
                    var entrada = new Entrada
                    {
                        Codigo = codigo,
                        FechaRegistro = DateTime.Now,
                        Conteo = 1 // Siempre establece el conteo en 1
                    };

                    // Agrega la nueva entrada al contexto de base de datos
                    _context.Entradas.Add(entrada);
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
