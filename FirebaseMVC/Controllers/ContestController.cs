using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CanYouEvenWin.Models;
using CanYouEvenWin.Repositories;
using CanYouEvenWin.Models.ViewModels;
using System.Security.Claims;

namespace CanYouEvenWin.Controllers
{
    public class ContestController : Controller
    {
        private readonly IContestRepository _contestRepo;
        private readonly ICalculationRepository _calcRepo;
        public ContestController(IContestRepository contestRepository, ICalculationRepository calculationRepository)
        {
            _contestRepo = contestRepository;
            _calcRepo = calculationRepository;
        }
        // GET: ContestController
        public ActionResult Index()
        {
            List<Contest> contests = _contestRepo.GetAllContests();

            return View(contests);
        }

        // GET: ContestController/Details/5
        public ActionResult Details(int id)
        {
            var uId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Contest contest = _contestRepo.GetContestById(id);
            List<Prize> prizes = _contestRepo.GetPrizeByContestId(id);
            Calculation calculation = _calcRepo.GetAllContestCalculation(id);
            Calculation ucalculation = _calcRepo.GetAllContestCalculationbyId(uId, id);


            ContestDetailViewModel vm = new ContestDetailViewModel
            {
                Prizes = prizes,
                Contest = contest,
                Calculation = calculation,
                UCalculation = ucalculation
            };

            return View(vm);
        }

        // GET: ContestController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contest contest)
        {
            try
            {
                contest.UserProfileId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                _contestRepo.AddContest(contest);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(contest);
            }
        }

        // GET: ContestController/Edit/5
        public ActionResult Edit(int id)
        {
            Contest contest = _contestRepo.GetContestById(id);
            if (contest == null)
            {
                return NotFound();
            }
            return View(contest);
        }

        // POST: ContestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contest contest)
        {
            try
            {
                _contestRepo.UpdateContest(contest);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ContestController/Delete/5
        public ActionResult Delete(int id)
        {
            Contest contest = _contestRepo.GetContestById(id);

            return View(contest);
        }

        // POST: ContestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Contest contest)
        {
            try
            {
                _contestRepo.DeleteContest(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(contest);
            }
        }
    }
}

//var userProfileId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
