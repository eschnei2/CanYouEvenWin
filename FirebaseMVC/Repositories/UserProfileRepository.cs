using Microsoft.Data.SqlClient;
using CanYouEvenWin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using CanYouEvenWin.Utils;

namespace CanYouEvenWin.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {

        private readonly IConfiguration _config;

        public UserProfileRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public UserProfile GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT Id, Email, FirebaseUserId, RoleId, FirstName, LastName, CreatedDate, ImageLocation, FirebaseUserId
                                    FROM UserProfile
                                    WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@id", id);

                    UserProfile userProfile = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        userProfile = new UserProfile
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirebaseUserId = reader.GetString(reader.GetOrdinal("FirebaseUserId")),
                            RoleId = reader.GetInt32(reader.GetOrdinal("RoleId")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            CreatedDated = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                            ImageLocation = reader.GetString(reader.GetOrdinal("ImageLocation"))

                        };
                    }
                    reader.Close();

                    return userProfile;
                }
            }
        }

        public UserProfile GetByFirebaseUserId(string firebaseUserId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT Id, Email, RoleId, FirstName, LastName, CreatedDate, ImageLocation, FirebaseUserId
                                    FROM UserProfile
                                    WHERE FirebaseUserId = @FirebaseuserId";

                    cmd.Parameters.AddWithValue("@FirebaseUserId", firebaseUserId);

                    UserProfile userProfile = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        userProfile = new UserProfile
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirebaseUserId = reader.GetString(reader.GetOrdinal("FirebaseUserId")),
                            RoleId = reader.GetInt32(reader.GetOrdinal("RoleId")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            CreatedDated = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                            ImageLocation = reader.GetString(reader.GetOrdinal("ImageLocation"))
                        };
                    }
                    reader.Close();

                    return userProfile;
                }
            }
        }

        public void Add(UserProfile userProfile)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO
                                        UserProfile (Email, RoleId, FirstName, LastName, CreatedDate, ImageLocation, FirebaseUserId) 
                                        OUTPUT INSERTED.ID
                                        VALUES(@email, 2, @firstName, @lastName, GETDATE(), @imageLocation, @firebaseUserId)";

                    cmd.Parameters.AddWithValue("@email", userProfile.Email);
                    cmd.Parameters.AddWithValue("@firebaseUserId", userProfile.FirebaseUserId);
                    DbUtils.AddParameter(cmd, "@firstName", userProfile.FirstName);
                    DbUtils.AddParameter(cmd, "@lastName", userProfile.LastName);
                    DbUtils.AddParameter(cmd, "@imageLocation", userProfile.ImageLocation);


                    userProfile.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}
