using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CanYouEvenWin.Models
{
    public class Prize
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public Contest Contest { get; set; }
        public int ContestId { get; set; }
    }
}
