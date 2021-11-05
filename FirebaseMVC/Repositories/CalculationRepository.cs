using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CanYouEvenWin.Models;
using CanYouEvenWin.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CanYouEvenWin.Repositories
{
    public class CalculationRepository : BaseRepository, ICalculationRepository
    {
        public CalculationRepository(IConfiguration config) : base(config) { }
        public Calculation GetAllCalculation()
        {
            int total = 0;
            int winC = 0;
            double winRate = 0;
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT COUNT(*)  AS totalCount FROM Attempt;
                                        ";

                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        total = DbUtils.GetInt(reader, "totalCount");
                    }
                }
            }
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT COUNT(*)  AS winCount FROM Attempt a
                                        INNER JOIN Prize p ON p.Id = a.PrizeId
                                        WHERE  p.Name != 'Nothing';
                                        ";

                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        winC = DbUtils.GetInt(reader, "winCount");
                    }
                }
            }
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT
                                        cast((select count(a.Id) From Attempt a 
                                        INNER JOIN Prize p on p.Id = a.PrizeId
                                        WHERE p.Name != 'Nothing') AS float) /
                                        cast((select count(a.Id) FROM Attempt a) AS float) AS winRate;
                                        ";

                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        winRate = DbUtils.GetDoubler(reader, "winRate");
                    }
                }
            }
            Calculation calculation = new Calculation()
            {
                CTotal = total,
                UCTotal = winC,
                Percentage = Math.Round(winRate, 2) * 100
            };

            return calculation;
        }

        public Calculation GetAllCalculationbyId(int id)
        {
            int total = 0;
            int winC = 0;
            double winRate = 0;
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT COUNT(*)  AS totalCount FROM Attempt
                                        WHERE UserProfileId = @id;
                                        ";

                    DbUtils.AddParameter(cmd, "@Id", id);
                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        total = DbUtils.GetInt(reader, "totalCount");
                    }
                }
            }
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT COUNT(*)  AS winCount FROM Attempt a
                                        INNER JOIN Prize p ON p.Id = a.PrizeId
                                        WHERE  p.Name != 'Nothing' AND UserProfileId = @Id;
                                        ";
                    DbUtils.AddParameter(cmd, "@Id", id);
                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        winC = DbUtils.GetInt(reader, "winCount");
                    }
                }
            }
            if (total != 0)
            {
                using (var conn = Connection)
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                                        SELECT
                                        cast((select count(a.Id) From Attempt a 
                                        INNER JOIN Prize p on p.Id = a.PrizeId
                                        WHERE p.Name != 'Nothing' AND UserProfileId = @Id) AS float) /
                                        cast((select count(a.Id) FROM Attempt a WHERE UserProfileId = @Id) AS float) AS winRate;
                                        ";

                        DbUtils.AddParameter(cmd, "@Id", id);
                        using var reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            winRate = DbUtils.GetDoubler(reader, "winRate");
                        }
                    }
                }
            }
            Calculation calculation = new Calculation()
            {
                CTotal = total,
                UCTotal = winC,
                Percentage = Math.Round(winRate, 2)*100
            };

            return calculation;
        }
        public Calculation GetAllContestCalculation(int cId)
        {
            int total = 0;
            int winC = 0;
            double winRate = 0;
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT COUNT(*)  AS totalCount FROM Attempt WHERE ContestId = @cId;
                                        ";

                    DbUtils.AddParameter(cmd, "@cId", cId);
                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        total = DbUtils.GetInt(reader, "totalCount");
                    }
                }
            }
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT COUNT(*)  AS winCount FROM Attempt a
                                        INNER JOIN Prize p ON p.Id = a.PrizeId
                                        WHERE  p.Name != 'Nothing' AND a.ContestId = @cId;
                                        ";

                    DbUtils.AddParameter(cmd, "@cId", cId);
                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        winC = DbUtils.GetInt(reader, "winCount");
                    }
                }
            }
            if (total != 0)
            {
                using (var conn = Connection)
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                                        SELECT
                                        cast((select count(a.Id) From Attempt a 
                                        INNER JOIN Prize p on p.Id = a.PrizeId
                                        WHERE p.Name != 'Nothing' AND a.ContestId = @cId) AS float) /
                                        cast((select count(a.Id) FROM Attempt a WHERE a.ContestId = @cId) AS float) AS winRate;
                                        ";

                        DbUtils.AddParameter(cmd, "@cId", cId);
                        using var reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            winRate = DbUtils.GetDoubler(reader, "winRate");
                        }
                    }
                }
            }
            Calculation calculation = new Calculation()
            {
                CTotal = total,
                UCTotal = winC,
                Percentage = Math.Round(winRate, 2) * 100
            };

            return calculation;
        }

        public Calculation GetAllContestCalculationbyId(int id, int cId)
        {
            int total = 0;
            int winC = 0;
            double winRate = 0;
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT COUNT(*)  AS totalCount FROM Attempt
                                        WHERE UserProfileId = @id AND ContestId = @cId;
                                        ";

                    DbUtils.AddParameter(cmd, "@Id", id);
                    DbUtils.AddParameter(cmd, "@cId", cId);
                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        total = DbUtils.GetInt(reader, "totalCount");
                    }
                }
            }
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT COUNT(*)  AS winCount FROM Attempt a
                                        INNER JOIN Prize p ON p.Id = a.PrizeId
                                        WHERE  p.Name != 'Nothing' AND a.UserProfileId = @Id AND a.ContestId = @cId;
                                        ";
                    DbUtils.AddParameter(cmd, "@Id", id);
                    DbUtils.AddParameter(cmd, "@cId", cId);
                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        winC = DbUtils.GetInt(reader, "winCount");
                    }
                }
            }
            if (total != 0)
            {
                using (var conn = Connection)
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                                        SELECT
                                        cast((select count(a.Id) From Attempt a 
                                        INNER JOIN Prize p on p.Id = a.PrizeId
                                        WHERE p.Name != 'Nothing' AND UserProfileId = @Id AND a.ContestId = @cId) AS float) /
                                        cast((select count(a.Id) FROM Attempt a WHERE UserProfileId = @Id AND a.ContestId = @cId) AS float) AS winRate;
                                        ";

                        DbUtils.AddParameter(cmd, "@Id", id);
                        DbUtils.AddParameter(cmd, "@cId", cId);
                        using var reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            winRate = DbUtils.GetDoubler(reader, "winRate");
                        }
                    }
                }
            }
            Calculation calculation = new Calculation()
            {
                CTotal = total,
                UCTotal = winC,
                Percentage = Math.Round(winRate, 2) * 100
            };

            return calculation;
        }
    }

}
