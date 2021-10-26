using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CanYouEvenWin.Models
{
    public class Contest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SiteURL { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ContestMaker { get; set; }
        public UserProfile UserProfile { get; set; }
        public int UserProfileId {get;set;}
        public Prize Prize { get; set; }

    }
}
