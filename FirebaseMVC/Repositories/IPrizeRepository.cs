using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CanYouEvenWin.Models;

namespace CanYouEvenWin.Repositories
{
    public interface IPrizeRepository
    {
        public List<Prize> GetPrizeByContestId(int id);
        public Prize GetPrizeById(int id);
        public void AddPrize(Prize prize);
        public void DeletePrize(int Id);
        public void UpdatePrize(Prize prize);
    }
}
