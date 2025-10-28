using HotelManager.Data;
using HotelManager.Models;

namespace HotelManager.Services
{
    public static class ReservaService
    {
        public static Reserva CriarReserva(int hospedeId, int quartoId, DateTime dataEntrada, DateTime dataSaida)
        {
            // 1. Validar datas
            if (dataEntrada.Date < DateTime.Today || dataSaida.Date <= dataEntrada.Date)
            {
                throw new Exception("Datas da reserva s�o inv�lidas.");
            }

            // 2. Buscar dados do quarto (para pre�o e status)
            var quarto = Database.GetQuartoById(quartoId);
            if (quarto == null)
            {
                throw new Exception("Quarto n�o encontrado.");
            }

            // 3. Verificar status manual do quarto
            if (quarto.Status == QuartoStatus.Manuten��o)
            {
                throw new Exception("Este quarto est� em manuten��o e n�o pode ser reservado.");
            }

            // 4. Verificar disponibilidade (l�gica de datas)
            bool disponivel = Database.IsQuartoDisponivel(quartoId, dataEntrada, dataSaida);
            if (!disponivel)
            {
                throw new Exception("O quarto n�o est� dispon�vel para as datas selecionadas.");
            }

            // 5. Calcular valor total
            int totalDias = (dataSaida.Date - dataEntrada.Date).Days;
            if (totalDias == 0) totalDias = 1; // M�nimo de 1 di�ria

            decimal valorTotal = totalDias * quarto.TipoQuarto!.PrecoDiaria;

            // 6. Criar o objeto Reserva
            var novaReserva = new Reserva
            {
                HospedeID = hospedeId,
                QuartoID = quartoId,
                DataEntrada = dataEntrada.Date,
                DataSaida = dataSaida.Date,
                ValorTotalReserva = valorTotal,
                StatusReserva = ReservaStatus.Pendente // Reserva come�a como pendente at� o pagamento
            };

            // 7. Salvar no banco
            int novoId = Database.AddReserva(novaReserva);
            novaReserva.ReservaID = novoId;

            return novaReserva;
        }
    }
}