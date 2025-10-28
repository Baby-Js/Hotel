namespace HotelManager.Models
{
    public class Reserva
    {
        public int ReservaID { get; set; }
        public int HospedeID { get; set; }
        public int QuartoID { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime DataSaida { get; set; }
        public decimal ValorTotalReserva { get; set; }
        public ReservaStatus StatusReserva { get; set; }
        public DateTime DataCriacao { get; set; }

        // Propriedades para dados relacionados (JOIN)
        public Hospede? Hospede { get; set; }
        public Quarto? Quarto { get; set; }
    }
}
