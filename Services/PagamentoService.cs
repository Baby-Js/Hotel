using HotelManager.Data;
using HotelManager.Models;

namespace HotelManager.Services
{
    public static class PagamentoService
    {
        public static void RegistrarPagamento(int reservaId, decimal valorPago, string metodo)
        {
            // 1. Validar valor
            if (valorPago <= 0)
            {
                throw new Exception("O valor do pagamento deve ser positivo.");
            }

            // 2. Buscar reserva
            var reserva = Database.GetReservaById(reservaId);
            if (reserva == null)
            {
                throw new Exception("Reserva não encontrada.");
            }
            if (reserva.StatusReserva == ReservaStatus.Cancelada || reserva.StatusReserva == ReservaStatus.Finalizada)
            {
                throw new Exception("Não é possível adicionar pagamento a uma reserva cancelada ou finalizada.");
            }

            // 3. Buscar pagamentos existentes
            var pagamentosAnteriores = Database.GetPagamentosByReserva(reservaId);
            decimal totalJaPago = pagamentosAnteriores.Sum(p => p.ValorPago);

            // 4. Verificar se o pagamento excede o valor total
            if ((totalJaPago + valorPago) > reserva.ValorTotalReserva)
            {
                throw new Exception($"Valor excede o total da reserva (Total: {reserva.ValorTotalReserva:C}, Pago: {totalJaPago:C}, Tentativa: {valorPago:C})");
            }

            // 5. Salvar pagamento
            var novoPagamento = new Pagamento
            {
                ReservaID = reservaId,
                ValorPago = valorPago,
                MetodoPagamento = metodo
            };
            Database.AddPagamento(novoPagamento);

            // 6. Atualizar status da reserva se o valor foi totalmente pago
            if ((totalJaPago + valorPago) == reserva.ValorTotalReserva)
            {
                Database.UpdateReservaStatus(reservaId, ReservaStatus.Confirmada);
                Console.WriteLine("Pagamento total recebido. Reserva CONFIRMADA.");
            }
            else
            {
                Console.WriteLine($"Pagamento parcial registrado. Saldo devedor: {(reserva.ValorTotalReserva - (totalJaPago + valorPago)):C}");
            }
        }
    }
}   