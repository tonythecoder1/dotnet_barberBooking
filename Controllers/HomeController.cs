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
        private readonly UserManager<MyUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IUserClaimsPrincipalFactory<MyUser> _userClaimsPrincipalFactory;
        private readonly SignInManager<MyUser> signInManager;

        
        public HomeController(UserManager<MyUser> userManager, ILogger<HomeController> logger, 
        IUserClaimsPrincipalFactory<MyUser> claimsPrincipalFactory, SignInManager<MyUser> signInManager)
        {
            _userManager = userManager;
            _logger = logger;
            _userClaimsPrincipalFactory = claimsPrincipalFactory;
            this.signInManager = signInManager;
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

    [HttpGet]
    public async Task<IActionResult> Reservas()
    {
    return View();
    }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordModel model)
        {
            if (ModelState.IsValid){
                var user = await _userManager.FindByEmailAsync(model.Email);
            

                if(user != null){
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetUrl = Url.Action("ResetPassword","Home",
                        new { token = token, email = model.Email}, Request.Scheme);

                        System.IO.File.WriteAllText("resetLink.txt", resetUrl);
                } else{
                    Console.WriteLine("Erro ao mudar password");
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            return View(new ResetPasswordModel{Token = token, Email = email});
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if(ModelState.IsValid){
                var user = await _userManager.FindByEmailAsync(model.Email);

                if(user != null){
                    var result =  await _userManager.ResetPasswordAsync(user,
                        model.Token, model.Password);

                        if(!result.Succeeded){

                            foreach (var error in result.Errors){
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                        return View("Sucesso");
                }
                ModelState.AddModelError("", "Invalid Request");
            }

            return View();
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login (LoginModel model)
        {
            if (ModelState.IsValid)
            {
               var siginResult = await signInManager.PasswordSignInAsync(model.UserName, 
                        model.Password, false, false);

                if(siginResult.Succeeded){
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

            var user = new MyUser
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();  // Encerra a sessão do utilizador atual
            return RedirectToAction("Login", "Home");  // Redireciona para página de Login
        }

    }
}