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
    public class PrizeRepository : BaseRepository, IPrizeRepository
    {
        public PrizeRepository(IConfiguration config) : base(config) { }

        public List<Prize> GetPrizeByContestId(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT Id, Name, Quantity, ContestId
                                        FROM Prize
                                        WHERE ContestId = @Id";

                    DbUtils.AddParameter(cmd, "@Id", id);
                    var prizes = new List<Prize>();

                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var prize = new Prize()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            Name = DbUtils.GetString(reader, "Name"),
                            Quantity = DbUtils.GetInt(reader, "Quantity"),
                            ContestId = DbUtils.GetInt(reader, "ContestId")
                        };

                        prizes.Add(prize);
                    }
                    return prizes;
                }
            }
        }
        public Prize GetPrizeById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT Id, Name, Quantity, ContestId
                                        FROM Prize
                                        WHERE Id = @Id";

                    DbUtils.AddParameter(cmd, "@Id", id);
                    Prize prize = null;

                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                         prize = new Prize()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            Name = DbUtils.GetString(reader, "Name"),
                            Quantity = DbUtils.GetInt(reader, "Quantity"),
                            ContestId = DbUtils.GetInt(reader, "ContestId")
                        };
                    }
                    return prize;
                }
            }
        }
        public void AddPrize(Prize prize)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO Prize (Name, Quantity, ContestId)
                                        OUTPUT INSERTED.ID
                                        VALUES (@name, @quantity, @contestId)";

                    DbUtils.AddParameter(cmd, "@name", prize.Name);
                    DbUtils.AddParameter(cmd, "@Quantity", prize.Quantity);
                    DbUtils.AddParameter(cmd, "@ContestId", prize.ContestId);

                    prize.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
        public void DeletePrize(int Id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        DELETE FROM Attempt
                                        WHERE PrizeId = @id";

                    cmd.Parameters.AddWithValue("@id", Id);
                    cmd.ExecuteNonQuery();
                }
            }
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        DELETE FROM Prize
                                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdatePrize(Prize prize)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        UPDATE Prize
                                        SET
                                        Name = @name,
                                        Quantity = @quantity
                                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", prize.Name);
                    cmd.Parameters.AddWithValue("@quantity", prize.Quantity);
                    cmd.Parameters.AddWithValue("@id", prize.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }  
}
