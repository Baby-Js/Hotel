using HotelManager.UI;

namespace HotelManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando Sistema de Gestão Hoteleira...");

            RunMainMenu();

            Console.WriteLine("Sistema finalizado.");
        }

        private static void RunMainMenu()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("--- Sistema de Gestão Hoteleira ---");
                Console.WriteLine("1. Gerenciar Hóspedes ");
                Console.WriteLine("2. Gerenciar Quartos ");
                Console.WriteLine("3. Gerenciar Reservas");
                Console.WriteLine("4. Registrar Pagamento ");
                Console.WriteLine("5. Relatórios");
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine("0. Sair");
                Console.Write("\nEscolha um módulo: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        GuestView.ShowMenu();
                        break;
                    case "2":
                        RoomView.ShowMenu();
                        break;
                    case "3":
                        ReservationView.ShowMenu();
                        break;
                    case "4":
                        PaymentView.ShowMenu();
                        break;
                    case "5":
                        ReportView.ShowMenu();
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Pressione [Enter] para tentar novamente.");
                        Console.ReadLine();
                        break;
                }
            }
        }
    }
}