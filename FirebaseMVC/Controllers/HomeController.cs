using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CanYouEvenWin.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CanYouEvenWin.Repositories;
using CanYouEvenWin.Models.ViewModels;

namespace CanYouEvenWin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ICalculationRepository _calcRepo;

        public HomeController(IUserProfileRepository userProfileRepository, ICalculationRepository calculationRepository)
        {
            _userProfileRepository = userProfileRepository;
            _calcRepo = calculationRepository;
        }

        public IActionResult Index()
        {
            HomeViewModel vm = new HomeViewModel();
            var userProfileId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            vm.UserProfile = _userProfileRepository.GetById(userProfileId);
            vm.Calculation = _calcRepo.GetAllCalculation();
            vm.SCalculation = _calcRepo.GetAllCalculationbyId(userProfileId);
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
