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
                                        SELECT c.Id, c.Name AS CName, c.SiteURL, c.UserProfileId, c.StartDate, c.EndDate, c.ContestMaker, p.Name AS PName,
                                        FROM Contest C
                                        ORDER BY EndDate";

                    DbUtils.AddParameter(cmd, "@Id", id);
                    using var reader = cmd.ExecuteReader();

                    Contest contest = null;

                    while (reader.Read())
                    {
                        contest = new Contest()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            Name = DbUtils.GetString(reader, "Name"),
                            SiteURL = DbUtils.GetString(reader, "SiteUrl"),
                            UserProfileId = DbUtils.GetInt(reader, "UserProfileId"),
                            StartDate = DbUtils.GetDateTime(reader, "StartDate"),
                            EndDate = DbUtils.GetDateTime(reader, "EndDate"),
                            ContestMaker = DbUtils.GetString(reader, "ContestMaker")
                        };

                        //contests.Add(contest);
                    }
                    return contest;
                }
            }

        }

    }
}