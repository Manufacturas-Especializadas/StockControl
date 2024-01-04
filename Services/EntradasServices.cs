using StockControl.Models;


namespace StockControl.Services
{
    public class EntradasServices
    {
        public IQueryable<Entrada> GetEntradas { get; set; }
    }
}
