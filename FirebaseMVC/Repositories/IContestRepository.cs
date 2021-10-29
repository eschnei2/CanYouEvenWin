using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CanYouEvenWin.Models;

namespace CanYouEvenWin.Repositories
{
    public interface IContestRepository
    {
       List<Contest> GetAllContests();
       Contest GetContestById(int id);

       public List<Prize> GetPrizeByContestId(int id);
       public void DeleteContest(int Id);
       public void AddContest(Contest contest);

        public void UpdateContest(Contest contest);
    }
}
