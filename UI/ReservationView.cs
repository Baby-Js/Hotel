using HotelManager.Data;
using HotelManager.Models;
using HotelManager.Services;

namespace HotelManager.UI
{
    public static class ReservationView
    {
        public static void ShowMenu()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("--- Gerenciamento de Reservas ---");
                Console.WriteLine("1. Criar Nova Reserva");
                Console.WriteLine("2. Cancelar Reserva");
                Console.WriteLine("0. Voltar ao Menu Principal");
                Console.Write("Escolha uma opção: ");

                switch (Console.ReadLine())
                {
                    case "1": CriarReserva(); break;
                    case "2": CancelarReserva(); break;
                    case "0": running = false; break;
                    default: Console.WriteLine("Opção inválida."); Console.ReadKey(); break;
                }
            }
        }

        private static void CriarReserva()
        {
            Console.Clear();
            Console.WriteLine("--- Nova Reserva ---");

            try
            {
                // 1. Selecionar Hóspede
                GuestView.ShowMenu(); // Usa a tela de hóspedes para cadastrar ou ver IDs
                Console.Clear();
                Console.WriteLine("--- Nova Reserva (Hóspede) ---");
                int hospedeId = ConsoleHelpers.GetInt("Digite o ID do Hóspede: ");

                // 2. Selecionar Quarto
                RoomView.ShowMenu(); // Usa a tela de quartos para ver IDs
                Console.Clear();
                Console.WriteLine("--- Nova Reserva (Quarto) ---");
                int quartoId = ConsoleHelpers.GetInt("Digite o ID do Quarto: ");

                // 3. Informar Datas
                Console.WriteLine("--- Nova Reserva (Datas) ---");
                DateTime dataEntrada = ConsoleHelpers.GetDate("Data de Entrada");
                DateTime dataSaida = ConsoleHelpers.GetDate("Data de Saída");

                // 4. Chamar o Serviço
                Console.WriteLine("\nProcessando reserva...");
                var reserva = ReservaService.CriarReserva(hospedeId, quartoId, dataEntrada, dataSaida);

                Console.WriteLine("\n--- RESERVA CRIADA (PENDENTE DE PAGAMENTO) ---");
                Console.WriteLine($"ID da Reserva: {reserva.ReservaID}");
                Console.WriteLine($"Hóspede (ID): {reserva.HospedeID}");
                Console.WriteLine($"Quarto (ID): {reserva.QuartoID}");
                Console.WriteLine($"Período: {reserva.DataEntrada:dd/MM/yy} a {reserva.DataSaida:dd/MM/yy}");
                Console.WriteLine($"Valor Total: {reserva.ValorTotalReserva:C}");
                Console.WriteLine("\nAcesse o menu 'Pagamentos' para confirmar a reserva.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nFalha ao criar reserva: {ex.Message}");
            }
            ConsoleHelpers.Pause();
        }

        private static void CancelarReserva()
        {
            Console.Clear();
            Console.WriteLine("--- Cancelar Reserva ---");
            try
            {
                int reservaId = ConsoleHelpers.GetInt("Digite o ID da Reserva a cancelar: ");
                var reserva = Database.GetReservaById(reservaId);

                if (reserva == null)
                {
                    Console.WriteLine("Reserva não encontrada.");
                }
                else if (reserva.StatusReserva == ReservaStatus.Cancelada || reserva.StatusReserva == ReservaStatus.Finalizada)
                {
                    Console.WriteLine($"Esta reserva já está no status '{reserva.StatusReserva}' e não pode ser cancelada.");
                }
                else
                {
                    Console.WriteLine($"Reserva {reserva.ReservaID} (Status: {reserva.StatusReserva}) será CANCELADA.");
                    Console.Write("Confirmar? (s/n): ");
                    if (Console.ReadLine()?.ToLower() == "s")
                    {
                        Database.UpdateReservaStatus(reservaId, ReservaStatus.Cancelada);
                        Console.WriteLine("Reserva cancelada com sucesso.");
                    }
                    else
                    {
                        Console.WriteLine("Operação abortada.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro ao cancelar: {ex.Message}");
            }
            ConsoleHelpers.Pause();
        }
    }
}