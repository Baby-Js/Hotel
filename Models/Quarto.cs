
namespace HotelManager.Models
{
    public class Quarto
    {
        public int QuartoID { get; set; }
        public int TipoQuartoID { get; set; }
        public string NumeroQuarto { get; set; }
        public QuartoStatus Status { get; set; }

        // Propriedade para dados relacionados (JOIN)
        public TipoDeQuarto? TipoQuarto { get; set; }
    }
}