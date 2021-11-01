using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CanYouEvenWin.Models;

namespace CanYouEvenWin.Repositories
{
    public interface IAttemptRepository
    {
        public List<Attempt> GetAttemptsByUserIdAndContestId(int userId, int contestId);

        public List<Prize> GetPrizeByContestId(int id);
        public void AddAttempt(Attempt attempt);
        public Attempt GetAttemptById(int id);
        public void UpdateAttempt(Attempt attempt);
        public void DeleteAttempt(int Id);
    }
}
