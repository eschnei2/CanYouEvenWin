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
    public class PrizeController : Controller
    {
        private readonly IPrizeRepository _prizeRepo;

        public PrizeController(IPrizeRepository prizeRepository)
        {
            _prizeRepo = prizeRepository;
        }
        // GET: PrizeController
        public ActionResult Index(int id)
        {
            PrizeIndexViewModel vm = new PrizeIndexViewModel();
            vm.PrizeList = _prizeRepo.GetPrizeByContestId(id);
            vm.ContestId = id;
            return View(vm);
        }

        // GET: PrizeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PrizeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PrizeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Prize prize, int id)
        {
            try
            {
                prize.ContestId = id;
                _prizeRepo.AddPrize(prize);
                return RedirectToAction("Index", new { id });
            }
            catch
            {
                return View(prize);
            }
        }

        // GET: PrizeController/Edit/5
        public ActionResult Edit(int id)
        {
            Prize prize = _prizeRepo.GetPrizeById(id);

            if (prize == null)
            {
                return NotFound();
            }

            return View(prize);
        }

        // POST: PrizeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Prize prize)
        {

            Prize prizer = _prizeRepo.GetPrizeById(id);
            try
            {
                _prizeRepo.UpdatePrize(prize);
                return RedirectToAction("Index", new { id = prizer.ContestId });
            }
            catch
            {
                return View();
            }
        }

        // GET: PrizeController/Delete/5
        public ActionResult Delete(int id)
        {
            Prize prize = _prizeRepo.GetPrizeById(id);
            return View(prize);
        }

        // POST: PrizeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Prize prize)
        {
            Prize prizer = _prizeRepo.GetPrizeById(id);
            try
            {
                _prizeRepo.DeletePrize(id);
                return RedirectToAction("Index", new { id = prizer.ContestId });
            }
            catch
            {
                return View(prize);
            }
        }
    }
}
