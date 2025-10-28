using HotelManager.Data;
using HotelManager.Models;

namespace HotelManager.UI
{
    public static class GuestView
    {
        public static void ShowMenu()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("--- Gerenciamento de Hóspedes ---");
                Console.WriteLine("1. Cadastrar Novo Hóspede");
                Console.WriteLine("2. Listar Todos os Hóspedes");
                Console.WriteLine("0. Voltar ao Menu Principal");
                Console.Write("Escolha uma opção: ");

                switch (Console.ReadLine())
                {
                    case "1": CadastrarHospede(); break;
                    case "2": ListarHospedes(); break;
                    case "0": running = false; break;
                    default: Console.WriteLine("Opção inválida."); Console.ReadKey(); break;
                }
            }
        }

        private static void CadastrarHospede()
        {
            Console.Clear();
            Console.WriteLine("--- Novo Hóspede ---");
            try
            {
                var hospede = new Hospede
                {
                    NomeCompleto = ConsoleHelpers.GetString("Nome Completo: "),
                    Documento = ConsoleHelpers.GetString("Documento (CPF/Passaporte): "),
                    Email = ConsoleHelpers.GetString("Email: "),
                    Telefone = ConsoleHelpers.GetString("Telefone (Opcional): ")
                };

                int id = Database.AddHospede(hospede);
                Console.WriteLine($"\nHóspede cadastrado com sucesso! (ID: {id})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro ao cadastrar: {ex.Message}");
            }
            ConsoleHelpers.Pause();
        }

        private static void ListarHospedes()
        {
            Console.Clear();
            Console.WriteLine("--- Lista de Hóspedes ---");
            try
            {
                var hospedes = Database.GetAllHospedes();
                if (hospedes.Count == 0)
                {
                    Console.WriteLine("Nenhum hóspede cadastrado.");
                }
                else
                {
                    foreach (var h in hospedes)
                    {
                        Console.WriteLine($"ID: {h.HospedeID} | Nome: {h.NomeCompleto} | Doc: {h.Documento} | Email: {h.Email}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro ao listar: {ex.Message}");
            }
            ConsoleHelpers.Pause();
        }
    }
}