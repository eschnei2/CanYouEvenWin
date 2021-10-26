using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CanYouEvenWin.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string FirebaseUserId { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedDated { get; set; }
        public string ImageLocation { get; set; }
        public int RoleId { get; set; }
    }
}
