using HotelManager.Data;
using HotelManager.Models;

namespace HotelManager.UI
{
    public static class ReportView
    {

        public static void ShowMenu()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("--- Relat�rios ---");
                Console.WriteLine("1. Relat�rio de Ocupa��o por Data");
                Console.WriteLine("0. Voltar ao Menu Principal");
                Console.Write("Escolha uma op��o: ");

                switch (Console.ReadLine())
                {
                    case "1": RelatorioOcupacao(); break;
                    case "0": running = false; break;
                    default: Console.WriteLine("Op��o inv�lida."); Console.ReadKey(); break;
                }
            }
        }

        private static void RelatorioOcupacao()
        {
            Console.Clear();
            Console.WriteLine("--- Relat�rio de Ocupa��o ---");
            try
            {
                DateTime dataBusca = ConsoleHelpers.GetDate("Digite a data para verificar a ocupa��o");

                var reservas = Database.GetRelatorioOcupacao(dataBusca);

                Console.WriteLine($"\n--- Quartos Ocupados em {dataBusca:dd/MM/yyyy} ---");

                if (reservas.Count == 0)
                {
                    Console.WriteLine("Nenhum quarto ocupado (ou com reserva ativa) nesta data.");
                }
                else
                {
                    Console.WriteLine($"Total: {reservas.Count} quartos");
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine($"{"Quarto",-10} | {"H�spede",-30} | {"Per�odo",-25} | {"Status",-12}");
                    Console.WriteLine("--------------------------------------------------");
                    foreach (var r in reservas)
                    {
                        string periodo = $"{r.DataEntrada:dd/MM/yy} a {r.DataSaida:dd/MM/yy}";
                        Console.WriteLine($"{r.Quarto?.NumeroQuarto,-10} | {r.Hospede?.NomeCompleto,-30} | {periodo,-25} | {r.StatusReserva,-12}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro ao gerar relat�rio: {ex.Message}");
            }
            ConsoleHelpers.Pause();
        }
    }
}