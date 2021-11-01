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
    public class AttemptController : Controller
    {
        private readonly IAttemptRepository _attemptRepo;
        private readonly IPrizeRepository _prizeRepo;

        public AttemptController(IAttemptRepository attemptRepository, IPrizeRepository prizeRepository)
        {
            _attemptRepo = attemptRepository;
            _prizeRepo = prizeRepository;
        }
        // GET: AttemptController
        public ActionResult Index(int id)
        {
            AttemptsIndexViewModel vm = new AttemptsIndexViewModel();
            vm.ContestId = id;
            vm.UserProfileId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
             var UserProfileId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            vm.AttemptsList = _attemptRepo.GetAttemptsByUserIdAndContestId(UserProfileId, id);

            return View(vm);
        }

        // GET: AttemptController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AttemptController/Create
        public ActionResult Create(int id)
        {
            List<Prize> prizes = _prizeRepo.GetPrizeByContestId(id);

            AttemptFormViewModel vm = new AttemptFormViewModel()
            {
                Attempt = new Attempt(),
                Prizes = prizes
            };
            return View(vm);
        }

        // POST: AttemptController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, AttemptFormViewModel attemptCreate)
        {
            try
            {
                attemptCreate.Attempt.UserProfileId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                attemptCreate.Attempt.ContestId = id;
                _attemptRepo.AddAttempt(attemptCreate.Attempt);
                return RedirectToAction("Index", new { id });
            }
            catch
            {              
                return View(attemptCreate);
            }
        }

        // GET: AttemptController/Edit/5
        public ActionResult Edit(int id)
        {
            Attempt attempter = _attemptRepo.GetAttemptById(id);
            List<Prize> prizes = _prizeRepo.GetPrizeByContestId(attempter.ContestId);
            AttemptFormViewModel vm = new AttemptFormViewModel()
            {
                Attempt = _attemptRepo.GetAttemptById(id),
                Prizes = prizes
            };

            if (vm == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        // POST: AttemptController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, AttemptFormViewModel attemptEdit)
        {
            Attempt attempter = _attemptRepo.GetAttemptById(id);
            try
            {
                _attemptRepo.UpdateAttempt(attemptEdit.Attempt);

                return RedirectToAction("Index", new { id = attempter.ContestId });
            }
            catch
            {
                return View();
            }
        }

        // GET: AttemptController/Delete/5
        public ActionResult Delete(int id)
        {
            Attempt attempt = _attemptRepo.GetAttemptById(id);
            return View(attempt);
        }

        // POST: AttemptController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Attempt attempt)
        {
            Attempt attempter = _attemptRepo.GetAttemptById(id);
            try
            {
                _attemptRepo.DeleteAttempt(id);
                return RedirectToAction("Index", new { id = attempter.ContestId });
            }
            catch
            {
                return View(attempt);
            }
        }
    }
}
