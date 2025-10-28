using MySql.Data.MySqlClient;
using HotelManager.Models;
using System.Data;

namespace HotelManager.Data
{
    public static class Database
    {
        // !!! IMPORTANTE: Altere esta string para o seu banco MySQL !!!
        private const string connectionString = "Server=127.0.0.1;Database=hotelmanager_db;Uid=root;Pwd=;";

        private static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        // --- CRUD Hóspedes ---

        public static int AddHospede(Hospede hospede)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var query = "INSERT INTO Hospedes (NomeCompleto, Documento, Email, Telefone) VALUES (@Nome, @Doc, @Email, @Tel); SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nome", hospede.NomeCompleto);
                    cmd.Parameters.AddWithValue("@Doc", hospede.Documento);
                    cmd.Parameters.AddWithValue("@Email", hospede.Email);
                    cmd.Parameters.AddWithValue("@Tel", hospede.Telefone);
                    // Retorna o ID do novo hóspede
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public static List<Hospede> GetAllHospedes()
        {
            var list = new List<Hospede>();
            using (var conn = GetConnection())
            {
                conn.Open();
                var query = "SELECT * FROM Hospedes ORDER BY NomeCompleto";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Hospede
                        {
                            HospedeID = reader.GetInt32("HospedeID"),
                            NomeCompleto = reader.GetString("NomeCompleto"),
                            Documento = reader.GetString("Documento"),
                            Email = reader.GetString("Email"),
                            Telefone = reader.IsDBNull("Telefone") ? null : reader.GetString("Telefone")
                        });
                    }
                }
            }
            return list;
        }

        // (Aqui iriam os métodos UpdateHospede e DeleteHospede)

        // --- CRUD Quartos e Tipos ---

        public static int AddTipoQuarto(TipoDeQuarto tipo)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var query = "INSERT INTO TipoDeQuarto (NomeTipo, Descricao, CapacidadeMaxima, PrecoDiaria) VALUES (@Nome, @Desc, @Cap, @Preco); SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nome", tipo.NomeTipo);
                    cmd.Parameters.AddWithValue("@Desc", tipo.Descricao);
                    cmd.Parameters.AddWithValue("@Cap", tipo.CapacidadeMaxima);
                    cmd.Parameters.AddWithValue("@Preco", tipo.PrecoDiaria);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public static List<TipoDeQuarto> GetAllTiposQuarto()
        {
            var list = new List<TipoDeQuarto>();
            using (var conn = GetConnection())
            {
                conn.Open();
                var query = "SELECT * FROM TipoDeQuarto ORDER BY NomeTipo";
                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new TipoDeQuarto
                        {
                            TipoQuartoID = reader.GetInt32("TipoQuartoID"),
                            NomeTipo = reader.GetString("NomeTipo"),
                            Descricao = reader.IsDBNull("Descricao") ? null : reader.GetString("Descricao"),
                            CapacidadeMaxima = reader.GetInt32("CapacidadeMaxima"),
                            PrecoDiaria = reader.GetDecimal("PrecoDiaria")
                        });
                    }
                }
            }
            return list;
        }

        public static int AddQuarto(Quarto quarto)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var query = "INSERT INTO Quartos (TipoQuartoID, NumeroQuarto, Status) VALUES (@TipoID, @Num, @Status); SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TipoID", quarto.TipoQuartoID);
                    cmd.Parameters.AddWithValue("@Num", quarto.NumeroQuarto);
                    cmd.Parameters.AddWithValue("@Status", quarto.Status.ToString());
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public static List<Quarto> GetAllQuartos()
        {
            var list = new List<Quarto>();
            using (var conn = GetConnection())
            {
                conn.Open();
                // JOIN para pegar informações do Tipo de Quarto
                var query = @"
                    SELECT q.*, t.NomeTipo, t.PrecoDiaria 
                    FROM Quartos q
                    JOIN TipoDeQuarto t ON q.TipoQuartoID = t.TipoQuartoID
                    ORDER BY q.NumeroQuarto";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var quarto = new Quarto
                        {
                            QuartoID = reader.GetInt32("QuartoID"),
                            TipoQuartoID = reader.GetInt32("TipoQuartoID"),
                            NumeroQuarto = reader.GetString("NumeroQuarto"),
                            Status = Enum.Parse<QuartoStatus>(reader.GetString("Status")),
                            TipoQuarto = new TipoDeQuarto // Preenche o objeto aninhado
                            {
                                NomeTipo = reader.GetString("NomeTipo"),
                                PrecoDiaria = reader.GetDecimal("PrecoDiaria")
                            }
                        };
                        list.Add(quarto);
                    }
                }
            }
            return list;
        }

        public static Quarto? GetQuartoById(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var query = @"
                    SELECT q.*, t.NomeTipo, t.PrecoDiaria, t.CapacidadeMaxima, t.Descricao, t.TipoQuartoID as TipoID
                    FROM Quartos q
                    JOIN TipoDeQuarto t ON q.TipoQuartoID = t.TipoQuartoID
                    WHERE q.QuartoID = @ID";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Quarto
                            {
                                QuartoID = reader.GetInt32("QuartoID"),
                                TipoQuartoID = reader.GetInt32("TipoQuartoID"),
                                NumeroQuarto = reader.GetString("NumeroQuarto"),
                                Status = Enum.Parse<QuartoStatus>(reader.GetString("Status")),
                                TipoQuarto = new TipoDeQuarto
                                {
                                    TipoQuartoID = reader.GetInt32("TipoID"),
                                    NomeTipo = reader.GetString("NomeTipo"),
                                    PrecoDiaria = reader.GetDecimal("PrecoDiaria"),
                                    CapacidadeMaxima = reader.GetInt32("CapacidadeMaxima"),
                                    Descricao = reader.IsDBNull("Descricao") ? null : reader.GetString("Descricao")
                                }
                            };
                        }
                    }
                }
            }
            return null;
        }

        public static void UpdateQuartoStatus(int quartoId, QuartoStatus status)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var query = "UPDATE Quartos SET Status = @Status WHERE QuartoID = @ID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", status.ToString());
                    cmd.Parameters.AddWithValue("@ID", quartoId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // --- Lógica de Reserva ---

        public static bool IsQuartoDisponivel(int quartoId, DateTime entrada, DateTime saida)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var query = @"
                    SELECT COUNT(*) 
                    FROM Reservas 
                    WHERE QuartoID = @QuartoID
                    AND StatusReserva IN ('Confirmada', 'Pendente')
                    -- Verifica sobreposição de datas: (StartA < EndB) and (EndA > StartB)
                    AND (DataEntrada < @DataSaida AND DataSaida > @DataEntrada)";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@QuartoID", quartoId);
                    cmd.Parameters.AddWithValue("@DataEntrada", entrada);
                    cmd.Parameters.AddWithValue("@DataSaida", saida);

                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count == 0; // Disponível se não houver nenhuma reserva conflitante
                }
            }
        }

        public static int AddReserva(Reserva reserva)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var query = @"
                    INSERT INTO Reservas (HospedeID, QuartoID, DataEntrada, DataSaida, ValorTotalReserva, StatusReserva) 
                    VALUES (@HospedeID, @QuartoID, @DataEntrada, @DataSaida, @ValorTotal, @Status);
                    SELECT LAST_INSERT_ID();";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@HospedeID", reserva.HospedeID);
                    cmd.Parameters.AddWithValue("@QuartoID", reserva.QuartoID);
                    cmd.Parameters.AddWithValue("@DataEntrada", reserva.DataEntrada);
                    cmd.Parameters.AddWithValue("@DataSaida", reserva.DataSaida);
                    cmd.Parameters.AddWithValue("@ValorTotal", reserva.ValorTotalReserva);
                    cmd.Parameters.AddWithValue("@Status", reserva.StatusReserva.ToString());

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public static Reserva? GetReservaById(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var query = "SELECT * FROM Reservas WHERE ReservaID = @ID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Reserva
                            {
                                ReservaID = reader.GetInt32("ReservaID"),
                                HospedeID = reader.GetInt32("HospedeID"),
                                QuartoID = reader.GetInt32("QuartoID"),
                                DataEntrada = reader.GetDateTime("DataEntrada"),
                                DataSaida = reader.GetDateTime("DataSaida"),
                                ValorTotalReserva = reader.GetDecimal("ValorTotalReserva"),
                                StatusReserva = Enum.Parse<ReservaStatus>(reader.GetString("StatusReserva")),
                                DataCriacao = reader.GetDateTime("DataCriacao")
                            };
                        }
                    }
                }
            }
            return null;
        }

        public static void UpdateReservaStatus(int reservaId, ReservaStatus status)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var query = "UPDATE Reservas SET StatusReserva = @Status WHERE ReservaID = @ID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", status.ToString());
                    cmd.Parameters.AddWithValue("@ID", reservaId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // --- Pagamentos ---

        public static int AddPagamento(Pagamento pagamento)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                var query = @"
                    INSERT INTO Pagamentos (ReservaID, ValorPago, DataPagamento, MetodoPagamento) 
                    VALUES (@ReservaID, @Valor, NOW(), @Metodo);
                    SELECT LAST_INSERT_ID();";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ReservaID", pagamento.ReservaID);
                    cmd.Parameters.AddWithValue("@Valor", pagamento.ValorPago);
                    cmd.Parameters.AddWithValue("@Metodo", pagamento.MetodoPagamento);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public static List<Pagamento> GetPagamentosByReserva(int reservaId)
        {
            var list = new List<Pagamento>();
            using (var conn = GetConnection())
            {
                conn.Open();
                var query = "SELECT * FROM Pagamentos WHERE ReservaID = @ID ORDER BY DataPagamento";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", reservaId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Pagamento
                            {
                                PagamentoID = reader.GetInt32("PagamentoID"),
                                ReservaID = reader.GetInt32("ReservaID"),
                                ValorPago = reader.GetDecimal("ValorPago"),
                                DataPagamento = reader.GetDateTime("DataPagamento"),
                                MetodoPagamento = reader.GetString("MetodoPagamento")
                            });
                        }
                    }
                }
            }
            return list;
        }

        // --- Relatórios (simplificados para console) ---

        public static List<Reserva> GetRelatorioOcupacao(DateTime data)
        {
            var list = new List<Reserva>();
            using (var conn = GetConnection())
            {
                conn.Open();
                // Query para buscar reservas ativas (Confirmada/Pendente) na data específica
                var query = @"
                    SELECT r.*, h.NomeCompleto, q.NumeroQuarto
                    FROM Reservas r
                    JOIN Hospedes h ON r.HospedeID = h.HospedeID
                    JOIN Quartos q ON r.QuartoID = q.QuartoID
                    WHERE r.StatusReserva IN ('Confirmada', 'Pendente')
                    AND @DataBusca BETWEEN r.DataEntrada AND DATE_SUB(r.DataSaida, INTERVAL 1 DAY)
                    ORDER BY q.NumeroQuarto";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DataBusca", data.Date);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Reserva
                            {
                                ReservaID = reader.GetInt32("ReservaID"),
                                DataEntrada = reader.GetDateTime("DataEntrada"),
                                DataSaida = reader.GetDateTime("DataSaida"),
                                StatusReserva = Enum.Parse<ReservaStatus>(reader.GetString("StatusReserva")),
                                Hospede = new Hospede { NomeCompleto = reader.GetString("NomeCompleto") },
                                Quarto = new Quarto { NumeroQuarto = reader.GetString("NumeroQuarto") }
                            });
                        }
                    }
                }
            }
            return list;
        }
    }
}