using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CanYouEvenWin.Models.ViewModels
{
    public class ContestDetailViewModel
    {
        public List<Prize> Prizes { get; set; }
        public Contest Contest { get; set; }
        public List<Attempt> Attempts { get; set; }
        public Attempt Attempt { get; set; }
        public int ContestId { get; set; }
        public int UserProfileId { get; set; }
        public Calculation Calculation { get; set; }
        public Calculation UCalculation { get; set; }
    }
}
