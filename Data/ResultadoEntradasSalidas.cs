namespace StockControl.Data
{

    [Serializable]
    public class ResultadoEntradasSalidas
    {
        public string Codigo { get; set; }

        public int CantidadEntrada { get; set; }

        public int CantidadSalida  { get; set; }

        public int CantidadFinal { get; set; }

        public DateTime FechaEntrada { get; set; }

        public DateTime? FechaSalida { get; set; }

    }
}
