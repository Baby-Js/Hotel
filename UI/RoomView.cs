using HotelManager.Data;
using HotelManager.Models;

namespace HotelManager.UI
{
    public static class RoomView
    {
        public static void ShowMenu()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("--- Gerenciamento de Quartos ---");
                Console.WriteLine("1. Cadastrar Novo Tipo de Quarto");
                Console.WriteLine("2. Listar Tipos de Quarto");
                Console.WriteLine("3. Cadastrar Novo Quarto");
                Console.WriteLine("4. Listar Todos os Quartos");
                Console.WriteLine("5. Atualizar Status de um Quarto");
                Console.WriteLine("0. Voltar ao Menu Principal");
                Console.Write("Escolha uma op��o: ");

                switch (Console.ReadLine())
                {
                    case "1": CadastrarTipoQuarto(); break;
                    case "2": ListarTiposQuarto(); break;
                    case "3": CadastrarQuarto(); break;
                    case "4": ListarQuartos(); break;
                    case "5": AtualizarStatusQuarto(); break;
                    case "0": running = false; break;
                    default: Console.WriteLine("Op��o inv�lida."); Console.ReadKey(); break;
                }
            }
        }

        private static void CadastrarTipoQuarto()
        {
            Console.Clear();
            Console.WriteLine("--- Novo Tipo de Quarto ---");
            try
            {
                var tipo = new TipoDeQuarto
                {
                    NomeTipo = ConsoleHelpers.GetString("Nome do Tipo (ex: Standard, Deluxe): "),
                    Descricao = ConsoleHelpers.GetString("Descri��o (Opcional): "),
                    CapacidadeMaxima = ConsoleHelpers.GetInt("Capacidade M�xima (pessoas): "),
                    PrecoDiaria = ConsoleHelpers.GetDecimal("Pre�o da Di�ria (R$): ")
                };

                int id = Database.AddTipoQuarto(tipo);
                Console.WriteLine($"\nTipo de quarto cadastrado com sucesso! (ID: {id})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro ao cadastrar: {ex.Message}");
            }
            ConsoleHelpers.Pause();
        }

        private static List<TipoDeQuarto>? ListarTiposQuarto(bool pause = true)
        {
            Console.Clear();
            Console.WriteLine("--- Lista de Tipos de Quarto ---");
            try
            {
                var tipos = Database.GetAllTiposQuarto();
                if (tipos.Count == 0)
                {
                    Console.WriteLine("Nenhum tipo de quarto cadastrado.");
                    if (pause) ConsoleHelpers.Pause();
                    return null;
                }

                foreach (var t in tipos)
                {
                    Console.WriteLine($"ID: {t.TipoQuartoID} | Nome: {t.NomeTipo} | Pre�o: {t.PrecoDiaria:C} | Cap: {t.CapacidadeMaxima}pax");
                }
                if (pause) ConsoleHelpers.Pause();
                return tipos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro ao listar: {ex.Message}");
                if (pause) ConsoleHelpers.Pause();
                return null;
            }
        }

        private static void CadastrarQuarto()
        {
            Console.Clear();
            Console.WriteLine("--- Novo Quarto ---");

            // Mostra os tipos de quarto para o usu�rio escolher
            var tipos = ListarTiposQuarto(pause: false);
            if (tipos == null || tipos.Count == 0)
            {
                Console.WriteLine("\n� preciso cadastrar um 'Tipo de Quarto' primeiro.");
                ConsoleHelpers.Pause();
                return;
            }

            Console.WriteLine("-----------------------------");
            try
            {
                var quarto = new Quarto
                {
                    TipoQuartoID = ConsoleHelpers.GetInt("Digite o ID do Tipo de Quarto: "),
                    NumeroQuarto = ConsoleHelpers.GetString("N�mero/Nome do Quarto (ex: 101, 205B): "),
                    Status = QuartoStatus.Dispon�vel // Novo quarto sempre come�a dispon�vel
                };

                // Valida��o simples
                if (!tipos.Any(t => t.TipoQuartoID == quarto.TipoQuartoID))
                {
                    Console.WriteLine("\nID de Tipo de Quarto inv�lido.");
                    ConsoleHelpers.Pause();
                    return;
                }

                int id = Database.AddQuarto(quarto);
                Console.WriteLine($"\nQuarto cadastrado com sucesso! (ID: {id})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro ao cadastrar: {ex.Message}");
            }
            ConsoleHelpers.Pause();
        }

        private static List<Quarto>? ListarQuartos(bool pause = true)
        {
            Console.Clear();
            Console.WriteLine("--- Lista de Quartos ---");
            try
            {
                var quartos = Database.GetAllQuartos();
                if (quartos.Count == 0)
                {
                    Console.WriteLine("Nenhum quarto cadastrado.");
                    if (pause) ConsoleHelpers.Pause();
                    return null;
                }

                foreach (var q in quartos)
                {
                    Console.WriteLine($"ID: {q.QuartoID} | Num: {q.NumeroQuarto} | Tipo: {q.TipoQuarto?.NomeTipo} | Status: {q.Status} | Di�ria: {q.TipoQuarto?.PrecoDiaria:C}");
                }
                if (pause) ConsoleHelpers.Pause();
                return quartos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro ao listar: {ex.Message}");
                if (pause) ConsoleHelpers.Pause();
                return null;
            }
        }

        private static void AtualizarStatusQuarto()
        {
            Console.Clear();
            Console.WriteLine("--- Atualizar Status do Quarto ---");
            var quartos = ListarQuartos(pause: false);
            if (quartos == null) { ConsoleHelpers.Pause(); return; }

            Console.WriteLine("-----------------------------");
            try
            {
                int quartoId = ConsoleHelpers.GetInt("Digite o ID do Quarto a atualizar: ");
                if (!quartos.Any(q => q.QuartoID == quartoId))
                {
                    Console.WriteLine("ID inv�lido.");
                    ConsoleHelpers.Pause();
                    return;
                }

                Console.WriteLine("Selecione o novo status:");
                Console.WriteLine($"1. {QuartoStatus.Dispon�vel}");
                Console.WriteLine($"2. {QuartoStatus.Ocupado}");
                Console.WriteLine($"3. {QuartoStatus.Manuten��o}");
                string op = ConsoleHelpers.GetString("Op��o: ");

                QuartoStatus novoStatus;
                switch (op)
                {
                    case "1": novoStatus = QuartoStatus.Dispon�vel; break;
                    case "2": novoStatus = QuartoStatus.Ocupado; break;
                    case "3": novoStatus = QuartoStatus.Manuten��o; break;
                    default: Console.WriteLine("Op��o inv�lida."); ConsoleHelpers.Pause(); return;
                }

                Database.UpdateQuartoStatus(quartoId, novoStatus);
                Console.WriteLine("\nStatus do quarto atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro ao atualizar: {ex.Message}");
            }
            ConsoleHelpers.Pause();
        }
    }
}