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
    }
}
