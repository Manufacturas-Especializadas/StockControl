using StockControl.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Components.Forms;
using OfficeOpenXml;
using Microsoft.JSInterop;
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;
using System.Globalization;
using System.Threading.Tasks;

namespace StockControl.Services
{
    public class EntradaServices
    {
        public partial class Scan
        {
            public string scan { get; set; }
        }

        private readonly StockControlContext _context;

        public EntradaServices(StockControlContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        private IDisposable CsvReader(StreamReader streamReader, CsvConfiguration csvConfiguration)
        {
            throw new NotImplementedException();
        }
        //public async Task<List<string>>ScanBarcode(IJSObjectReference archivo)
        //{
        //    var codigos = await readData(archivo);

        //    foreach(var codigo in codigos)
        //    {
        //        _context.Entradas.Add(new Entrada { Codigo = codigo });
        //    }

        //    await _context.SaveChangesAsync();

        //    return codigos;
        //}

        //public async Task<List<string>> readData(Stream filePath)
        //{
        //    var result = new List<string>();

        //    try
        //    {
        //        using(var reader = new StreamReader(filePath))
        //        {
        //            while (!reader.EndOfStream)
        //            {
        //                var line = await reader.ReadLineAsync();
        //                result.Add(line.Trim());    
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine($"Error al leer el archivo: {ex.Message}");
        //    }



        //    return result;
        //}


        //public async Task<List<string>> ScanBarcodeReadFile()
        //{
        //    var pathFile = "C:\\Users\\Innovacion\\Documents\\SRG100_data";

        //    return await readData(pathFile);
        //}


        //public async Task<List<string>> ProcessAndSave(List<string> barcode)
        //{
        //    foreach(var code in barcode)
        //    {
        //         _context.Entradas.Add(new Entrada { Codigo = code });
        //    }

        //    await _context.SaveChangesAsync();

        //    return barcode;
        //}

        //public async Task<string> ScanBarcode(string FilePath)
        //{
        //    string scannBarcode = GetBarcodeNumer(FilePath);  

        //}

    }
}
