﻿using AutoMapper;
using AwesomeNetwork.DAL.Extentions;
using AwesomeNetwork.DAL.Models.Users;
using AwesomeNetwork.Web.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeNetwork.Controllers.Account
{
    public class AccountManagerController : Controller
    {
        private IMapper _mapper;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountManagerController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }


        [Route("Login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View("Home/Login");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // string? ReturnUrl - иначе не пррооходит валидацию.
            if (ModelState.IsValid)
            {
                /* 
                Из контекста можно понять, что у пользователя есть возможность войти в систему как с использованием логина, так и с использованием email. 
                Однако, метод PasswordSignInAsync не принимает email в качестве первого аргумента, а принимает только username. 
                Если вы хотите использовать email вместо username, то вам необходимо использовать метод SignInAsync, 
                после проверки с помощью метода CheckPasswordAsync. 

                Однако, этот код не поддерживает проверку на блокировку при неудачных попытках входа, 
                так как метод SignInAsync не поддерживает этот аргумент. 
                Если вы хотите проверять блокировку при неудачных попытках входа, то вам нужно использовать другой метод, 
                как описано в ответе на StackOverflow:

                var user = await userManager.FindByEmailAsync(model.Email);
                var result = await signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);
                */

                //var user = _mapper.Map<User>(model);
                //Т.к. LoginViewModel принимает email, а _signInManager.PasswordSignInAsync() UserName
                //Поэтому:
                var user = await _userManager.FindByEmailAsync(model.Email);

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("MyPage", "AccountManager");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            //return View("Views/Home/Index.cshtml");
            return RedirectToAction("Index", "Home");
        }

        [Route("Logout")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("MyPage")]
        [Authorize]
        [HttpGet]
        public IActionResult MyPage()
        {
            var user = User;

            var result = _userManager.GetUserAsync(user);

            return View("User", new UserViewModel(result.Result));
        }

        [Route("User/Edit")]
        [Authorize]
        [HttpGet]
        public IActionResult Edit()
        {
            var user = User;

            var result = _userManager.GetUserAsync(user);

            if (result != null)
            {
                UserEditViewModel model = new UserEditViewModel
                {
                    UserId = result.Result.Id,
                    Email = result.Result.Email,
                    FirstName = result.Result.FirstName,
                    LastName = result.Result.LastName,
                    MiddleName = result.Result.MiddleName,
                    BirthDate = result.Result.BirthDate,
                    Image = result.Result.Image,
                    Status = result.Result.Status,
                    About = result.Result.About
                };
                return View("Edit", model);
            }
            return RedirectToAction("Index", "Home");
            //return View("Edit", new UserEditViewModel(result.Result));
        }

        [Authorize]
        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);

                user.Convert(model);

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("MyPage", "AccountManager");
                }
                else
                {
                    return RedirectToAction("Edit", "AccountManager");
                }
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return View("Edit", model);
            }
        }
        //search
        [Route("UserList")]
        [HttpPost]
        public IActionResult UserList(string search)
        {
            var model = new SearchViewModel
            {
                UserList = _userManager.Users.AsEnumerable().Where(x => x.GetFullName().ToLower().Contains(search.ToLower())).ToList()
            };
            return View("UserList", model);
        }
    }
}
