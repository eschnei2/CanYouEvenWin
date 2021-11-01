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
    public class AttemptRepository : BaseRepository, IAttemptRepository
    {
        public AttemptRepository(IConfiguration config) : base(config) { }

        public List<Attempt> GetAttemptsByUserIdAndContestId(int userId, int contestId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT a.Id as AId, a.ContestId, a.PrizeId, a.UserProfileId, p.Id as PId, p.Name
                                        FROM Attempt a
                                        INNER JOIN Prize p ON p.Id = a.PrizeId
                                        WHERE a.UserProfileId = @uId and a.ContestId = @cId
                                        ORDER BY a.Id";

                    DbUtils.AddParameter(cmd, "@uId", userId);
                    DbUtils.AddParameter(cmd, "@cId", contestId);

                    List<Attempt> attempts = new List<Attempt>();
                    using var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        attempts.Add(new Attempt()
                        {
                            Id = DbUtils.GetInt(reader, "AId"),
                            ContestId = DbUtils.GetInt(reader, "ContestId"),
                            PrizeId = DbUtils.GetInt(reader, "PrizeId"),
                            UserProfileId = DbUtils.GetInt(reader, "UserProfileId"),
                            Prize = new Prize()
                            {
                                Id = DbUtils.GetInt(reader, "PId"),
                                Name = DbUtils.GetString(reader, "Name")
                            }
                        }) ;
                    }
                    reader.Close();

                    return attempts;
                    
                }
            }
        }
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
        public Attempt GetAttemptById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT Id, ContestId, PrizeId, UserProfileId
                                        FROM Attempt
                                        WHERE Id = @Id";

                    DbUtils.AddParameter(cmd, "@Id", id);
                    Attempt attempt = null;

                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        attempt = new Attempt()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            PrizeId = DbUtils.GetInt(reader, "PrizeId"),
                            UserProfileId = DbUtils.GetInt(reader, "UserProfileId"),
                            ContestId = DbUtils.GetInt(reader, "ContestId")
                        };
                    }
                    return attempt;
                }
            }
        }
        public void AddAttempt(Attempt attempt)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO Attempt (ContestId, PrizeId, UserProfileId)
                                        OUTPUT INSERTED.ID
                                        VALUES (@cId, @pId, @uId)";

                    DbUtils.AddParameter(cmd, "@cId", attempt.ContestId);
                    DbUtils.AddParameter(cmd, "@pId", attempt.PrizeId);
                    DbUtils.AddParameter(cmd, "@uId", attempt.UserProfileId);

                    attempt.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
        public void UpdateAttempt(Attempt attempt)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        UPDATE Attempt
                                        SET
                                        PrizeId = @prizeId
                                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@prizeId", attempt.PrizeId);
                    cmd.Parameters.AddWithValue("@id", attempt.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteAttempt(int Id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        DELETE FROM Attempt
                                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
