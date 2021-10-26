using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CanYouEvenWin.Models;
using CanYouEvenWin.Repositories;

namespace CanYouEvenWin.Controllers
{
    public class ContestController : Controller
    {
        private readonly IContestRepository _contestRepo;
        public ContestController(IContestRepository contestRepository)
        {
            _contestRepo = contestRepository;
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
            return View();
        }

        // GET: ContestController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ContestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ContestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
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
            return View();
        }

        // POST: ContestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
