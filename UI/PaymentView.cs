using HotelManager.Data;
using HotelManager.Services;

namespace HotelManager.UI
{
    public static class PaymentView
    {
        public static void ShowMenu()
        {
            RegistrarPagamento();
        }

        private static void RegistrarPagamento()
        {
            Console.Clear();
            Console.WriteLine("--- Registro de Pagamento ---");
            try
            {
                // 1. Identificar a Reserva
                int reservaId = ConsoleHelpers.GetInt("Digite o ID da Reserva: ");
                var reserva = Database.GetReservaById(reservaId);

                if (reserva == null)
                {
                    Console.WriteLine("Reserva não encontrada.");
                    ConsoleHelpers.Pause();
                    return;
                }

                // 2. Mostrar detalhes da dívida
                var pagamentos = Database.GetPagamentosByReserva(reservaId);
                decimal totalPago = pagamentos.Sum(p => p.ValorPago);
                decimal saldoDevedor = reserva.ValorTotalReserva - totalPago;

                Console.WriteLine($"\nDetalhes da Reserva {reserva.ReservaID} (Status: {reserva.StatusReserva})");
                Console.WriteLine($"Valor Total: {reserva.ValorTotalReserva:C}");
                Console.WriteLine($"Total Pago: {totalPago:C}");
                Console.WriteLine($"Saldo Devedor: {saldoDevedor:C}");

                if (saldoDevedor <= 0)
                {
                    Console.WriteLine("\nEsta reserva já foi totalmente paga.");
                    ConsoleHelpers.Pause();
                    return;
                }

                // 3. Registrar o pagamento
                Console.WriteLine("\n--- Novo Pagamento ---");
                decimal valorAPagar = ConsoleHelpers.GetDecimal("Valor a pagar: R$ ");
                string metodo = ConsoleHelpers.GetString("Método (Dinheiro, Cartão, PIX): ");

                // 4. Chamar o Serviço
                PagamentoService.RegistrarPagamento(reservaId, valorAPagar, metodo);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nFalha ao registrar pagamento: {ex.Message}");
            }
            ConsoleHelpers.Pause();
        }
    }
}