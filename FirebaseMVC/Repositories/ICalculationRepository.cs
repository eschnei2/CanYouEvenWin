using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CanYouEvenWin.Models;

namespace CanYouEvenWin.Repositories
{
    public interface ICalculationRepository
    {
        public Calculation GetAllCalculation();
        public Calculation GetAllCalculationbyId(int id);
        public Calculation GetAllContestCalculationbyId(int id, int cId);
        public Calculation GetAllContestCalculation(int cId);
    }
}
