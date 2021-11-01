using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CanYouEvenWin.Models.ViewModels
{
    public class AttemptFormViewModel
    {
        public List<Prize> Prizes { get; set; }
        public Attempt Attempt { get; set; }
    }
}
