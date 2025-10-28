
namespace HotelManager.Models
{
    public class Hospede
    {
        public int HospedeID { get; set; }
        public string NomeCompleto { get; set; }
        public string Documento { get; set; }
        public string Email { get; set; }
        public string? Telefone { get; set; } // Pode ser nulo
    }
}