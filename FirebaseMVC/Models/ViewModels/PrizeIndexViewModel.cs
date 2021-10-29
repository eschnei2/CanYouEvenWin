using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CanYouEvenWin.Models.ViewModels
{
    public class PrizeIndexViewModel
    {
        public int ContestId { get; set; }
        public Prize Prize { get; set; }
        public List<Prize> PrizeList { get; set; }
        public Contest Contest { get; set; }
    }
}
