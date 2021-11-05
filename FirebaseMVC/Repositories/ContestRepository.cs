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
    public class ContestRespository : BaseRepository, IContestRepository
    {
        public ContestRespository(IConfiguration config) : base(config) { }
        public List<Contest> GetAllContests()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT Id, Name, SiteURL, UserProfileId, StartDate, EndDate, ContestMaker
                                        FROM Contest
                                        WHERE EndDate > GETDATE()
                                        ORDER BY EndDate";
                    using var reader = cmd.ExecuteReader();

                    var contests = new List<Contest>();

                    while (reader.Read())
                    {
                        var contest = new Contest()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            Name = DbUtils.GetString(reader, "Name"),
                            SiteURL = DbUtils.GetString(reader, "SiteUrl"),
                            UserProfileId = DbUtils.GetInt(reader, "UserProfileId"),
                            StartDate = DbUtils.GetDateTime(reader, "StartDate"),
                            EndDate = DbUtils.GetDateTime(reader, "EndDate"),
                            ContestMaker = DbUtils.GetString(reader, "ContestMaker")
                        };

                        contests.Add(contest);
                    }
                    return contests;
                }
            }
        }
        public Contest GetContestById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT c.Id AS CId, c.Name AS CName, c.SiteURL, c.UserProfileId, c.StartDate, c.EndDate, c.ContestMaker, p.Name AS PName,
                                        p.Id AS PId, p.Quantity, p.ContestId
                                        FROM Contest C
                                        LEFT JOIN Prize p ON p.ContestId = c.Id
                                        Where c.Id = @id
                                        ORDER BY EndDate";

                    DbUtils.AddParameter(cmd, "@Id", id);

                    Contest contest = null;
                    using var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (contest == null)
                        {
                            contest = new Contest()
                            {
                                Id = DbUtils.GetInt(reader, "CId"),
                                Name = DbUtils.GetString(reader, "CName"),
                                SiteURL = DbUtils.GetString(reader, "SiteUrl"),
                                UserProfileId = DbUtils.GetInt(reader, "UserProfileId"),
                                StartDate = DbUtils.GetDateTime(reader, "StartDate"),
                                EndDate = DbUtils.GetDateTime(reader, "EndDate"),
                                ContestMaker = DbUtils.GetString(reader, "ContestMaker")
                            };
                        }                    
                        //contests.Add(contest);
                    }
                    return contest;
                }
            }

        }
        public List<Prize> GetPrizeByContestId(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using(var cmd = conn.CreateCommand())
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
        public void DeleteContest(int Id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        DELETE FROM Attempt
                                        WHERE ContestId = @id";

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
                                        WHERE ContestId = @id";

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
                                        DELETE FROM Contest
                                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void AddContest(Contest contest)
        {
            var cId = 0;
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO Contest (Name, SiteURL, UserProfileId, StartDate, EndDate, ContestMaker)
                                        OUTPUT INSERTED.ID
                                        VALUES (@name, @siteUrl, @userProfileId, @startDate, @endDate, @contestMaker)";

                    DbUtils.AddParameter(cmd, "@name", contest.Name);
                    DbUtils.AddParameter(cmd, "@siteURL", contest.SiteURL);
                    DbUtils.AddParameter(cmd, "@userProfileId", contest.UserProfileId);
                    DbUtils.AddParameter(cmd, "@startDate", contest.StartDate);
                    DbUtils.AddParameter(cmd, "@endDate", contest.EndDate);
                    DbUtils.AddParameter(cmd, "@ContestMaker", contest.ContestMaker);

                    contest.Id = (int)cmd.ExecuteScalar();
                }
            }
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT Top 1 Id as cId from Contest ORDER BY cId DESC;
                                        ";

                    using var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        cId = DbUtils.GetInt(reader, "cId");
                    }
                }
            }
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO Prize (Name, Quantity, ContestId)
                                        OUTPUT INSERTED.ID
                                        VALUES ('Nothing', 1, @contestId)";

                    Prize prize = new Prize();

                    DbUtils.AddParameter(cmd, "@contestId", cId);

                    prize.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
        public void UpdateContest(Contest contest)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        UPDATE Contest
                                        SET
                                            Name = @name,
                                            SiteUrl = @siteURL,
                                            UserProfileId = @userProfileId,
                                            StartDate = @startDate,
                                            EndDate = @endDate,
                                            ContestMaker = @contestMaker
                                        WHERE Id = @id";


                    DbUtils.AddParameter(cmd, "@id", contest.Id);
                    DbUtils.AddParameter(cmd, "@name", contest.Name);
                    DbUtils.AddParameter(cmd, "@siteURL", contest.SiteURL);
                    DbUtils.AddParameter(cmd, "@userProfileId", contest.UserProfileId);
                    DbUtils.AddParameter(cmd, "@startDate", contest.StartDate);
                    DbUtils.AddParameter(cmd, "@endDate", contest.EndDate);
                    DbUtils.AddParameter(cmd, "@contestMaker", contest.ContestMaker);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}