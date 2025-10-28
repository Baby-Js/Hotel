namespace HotelManager.UI
{
    public static class ConsoleHelpers
    {
        public static string GetString(string prompt)
        {
            string? input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(input));
            return input;
        }

        public static int GetInt(string prompt)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out value))
                {
                    return value;
                }
                Console.WriteLine("Valor inválido. Tente novamente.");
            }
        }

        public static decimal GetDecimal(string prompt)
        {
            decimal value;
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out value) && value > 0)
                {
                    return value;
                }
                Console.WriteLine("Valor inválido. Deve ser um número positivo. Tente novamente.");
            }
        }

        public static DateTime GetDate(string prompt)
        {
            DateTime value;
            while (true)
            {
                Console.Write(prompt + " (dd/mm/aaaa): ");
                if (DateTime.TryParse(Console.ReadLine(), out value))
                {
                    return value.Date;
                }
                Console.WriteLine("Data inválida. Use o formato dd/mm/aaaa. Tente novamente.");
            }
        }

        public static void Pause()
        {
            Console.WriteLine("\nPressione [Enter] para continuar...");
            Console.ReadLine();
        }
    }
}