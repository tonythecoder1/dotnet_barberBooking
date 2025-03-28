using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using dotidentity.Models;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace dotidentity.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(UserManager<IdentityUser> userManager, ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login (LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password)){
                    var Identity = new ClaimsIdentity("cookies");
                    Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    Identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                    await HttpContext.SignInAsync("cookies", new ClaimsPrincipal(Identity));
                    return RedirectToAction("About");

                }

                ModelState.AddModelError("", "Utilizador ou senha errado");
            }
            

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            Console.WriteLine("🟢 Método Register chamado");

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"❌ Erro de validação: {error.ErrorMessage}");
                }
                return View(model);
            }

            var user = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.Email,
                NormalizedUserName = model.UserName.ToUpper(),
                NormalizedEmail = model.Email.ToUpper()
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                Console.WriteLine("✅ Usuário registrado com sucesso!");
                TempData["SuccessMessage"] = "Usuário registrado com sucesso!";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                Console.WriteLine($"❌ Erro na criação: {error.Description}");
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult About(){
            return View();
        }

        [HttpGet]
        public IActionResult Success(){
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
