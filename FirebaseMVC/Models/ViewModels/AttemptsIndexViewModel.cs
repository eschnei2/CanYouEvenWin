using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CanYouEvenWin.Models.ViewModels
{
    public class AttemptsIndexViewModel
    {
        public int UserProfileId { get; set; }
        public Prize Prize { get; set; }
        public List<Prize> Prizes {get ;set; }
        public List <Attempt> AttemptsList { get; set; }
        public Attempt Attempt { get; set; }
        public int ContestId { get; set; }
    }
}
