using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.Identity.Models;

namespace Notes.Identity.Controllers
{
    /// <summary>
    /// Это обычный MVC контроллер, который обрабатывает вход на сайт
    /// он умеет принимать запросы и возвращать ответы
    /// </summary>
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager; //отвечает за вход/выход и проверку пароля.
        private readonly UserManager<AppUser> _userManager; //помогает искать/создавать пользователей.
        public readonly IIdentityServerInteractionService _interactionService;

        public AuthController(SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            IIdentityServerInteractionService interactionService) =>
            (_signInManager, _userManager, _interactionService) = 
            (signInManager, userManager, interactionService);


        [HttpGet]
        public IActionResult Login(string returnUrl) // вызывается, когда человек переходит на страницу входа. Показывает форму логина
        {
            var viewModel = new LoginViewModel 
            { 
                ReturnUrl = returnUrl
            };
            return View(viewModel); // метод отдает пустую форму "логин/пароль"
        }


        //после того как юзер ввел логин и пароль и нажал "войти" срабаотывает POST
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel) // вызывается, когда человек нажал кнопку "войти" и форма с логином и паролем летит на сервер
        {
            // если данные невалидны - вернем ту же самую вьюшку
            if (!ModelState.IsValid)
            {
                return View(viewModel);//возвращаем ту же страницу
            }

            // ищем пользователя.
            var user = await _userManager.FindByNameAsync(viewModel.Username);
            if (user == null)
            {;
                ModelState.AddModelError(string.Empty, "User not found");
                return View(viewModel);//возвращаем ту же страницу
            }

            // проверяет пароль
            var result = await _signInManager.PasswordSignInAsync(viewModel.Username, viewModel.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Redirect(viewModel.ReturnUrl); //выполняет перенаправление на нужную страницу.
            }
            ModelState.AddModelError(string.Empty, "Login error");
            return View(viewModel); //возвращаем ту же страницу
        }


        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            var viewmodel = new RegisterViewModel 
            {
                ReturnUrl = returnUrl 
            };
            return View(viewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid) //модел стэйт заполняется благодаря атрибутам [Required], [DataType]
            {
                return View(viewModel);//возвращаем ту же страницу
            }

            var user = new AppUser
            {
                UserName = viewModel.Username,
            };

            var result = await _userManager.CreateAsync(user, viewModel.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Redirect(viewModel.ReturnUrl);  //выполняет перенаправление на нужную страницу.
            }
            ModelState.AddModelError(string.Empty, "Error occurred");
            return View(viewModel);//возвращаем ту же страницу
        }

        //если не указать атрибуты на эти методы - при запуске получим ошибку
        // "несколько эндпоинтов соответствуют вызову"
    }
}
