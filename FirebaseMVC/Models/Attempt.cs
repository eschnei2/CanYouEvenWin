using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CanYouEvenWin.Models
{
    public class Attempt
    {
        public int Id { get; set; }
        public int UserProfileId { get; set; }
        public int PrizeId { get; set; }
        public int ContestId { get; set; }
        public Prize Prize { get; set; }
        public Contest Contest { get; set; }
    }
}
