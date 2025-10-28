namespace HotelManager.Models
{
    public class TipoDeQuarto
    {
        public int TipoQuartoID { get; set; }
        public string NomeTipo { get; set; }
        public string? Descricao { get; set; }
        public int CapacidadeMaxima { get; set; }
        public decimal PrecoDiaria { get; set; }
    }
}
