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

        [HttpGet]
        public IActionResult Login(string returnUrl) // вызывается, когда человек переходит на страницу входа. 
        {
            var viewModel = new LoginViewModel 
            { 
                ReturnUrl = returnUrl
            };
            return View(); // метод отдает пустую форму "логин/пароль"
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel viewModel) // вызывается, когда человек нажал кнопку "войти" и форма с логином и паролем летит на сервер
        {
            return View(viewModel);
        }

        //если не указать атрибуты на эти методы - при запуске получим ошибку
        // "несколько эндпоинтов соответствуют вызову"
    }
}
