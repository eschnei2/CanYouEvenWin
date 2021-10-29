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
    }
}
