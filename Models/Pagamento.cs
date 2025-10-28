namespace HotelManager.Models
{
    public class Pagamento
    {
        public int PagamentoID { get; set; }
        public int ReservaID { get; set; }
        public decimal ValorPago { get; set; }
        public DateTime DataPagamento { get; set; }
        public string MetodoPagamento { get; set; }
    }
}
