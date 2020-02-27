using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Configuration.Core;
using Blog.Entities;
using Blog.Foundation.Helper;
using Blog.Services;
using Blog.Web.Base;
using Blog.Web.Filters;
using Blog.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Web.Controllers
{
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class AnonymusController : BlogBaseController
    {
        public IUserService _userService { get; }
        public IMapper _mapper { get; }
        public IHostEnvironment _hostingEnvironment { get; }
        private readonly AppSettings _appSettings;


        public AnonymusController(IUserService userService, IMapper mapper, IHostEnvironment hostingEnvironment, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
            _appSettings = appSettings.Value;
        }
        [HttpGet]

        public IActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            model.CreatedBy = "Admin";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!await _userService.IsEmailAlreadyRegistered(model.Email))
                {

                    string uniqueFileName = null;

                    if (model.PicPath != null)
                    {
                        string uploadFolder = Path.Combine(_hostingEnvironment.ContentRootPath, @"wwwroot\\user-images");
                        uniqueFileName = Guid.NewGuid().ToString() + '_' + model.PicPath.FileName;
                        string absFilePath = Path.Combine(uploadFolder, uniqueFileName);
                        model.PicPath.CopyTo(new FileStream(absFilePath, FileMode.Create));
                    }
                    var entity = _mapper.Map<UserEntity>(model);
                    entity.PicPath = uniqueFileName;
                    await _userService.Create(entity);
                    return RedirectToAction("Login");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Email Already Regsitered.";
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.Authenticate(model.Email, Crypt.GetMD5Hash(model.Password));
                if (user != null && !String.IsNullOrWhiteSpace(user.Id))
                {
                    // Set Cookie Automatically for Client Auth - Encypted Key is Store.
                    var claims = new List<Claim>{
                                new Claim("Email", user.Email),
                                new Claim("FullName", user.FirstName + ' ' + user.LastName),
                                new Claim("Roles", user.Roles),
                                new Claim("Id", user.Id),
                                new Claim("PicPath",user.PicPath)
                            };


                    var claimsIdentity = new ClaimsIdentity(claims, "Custom");

                    var authProperties = new AuthenticationProperties
                    {
                        //AllowRefresh = <bool>,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                        IsPersistent = true,
                        IssuedUtc = DateTime.Now,
                        RedirectUri = "/home/index"
                    };

                    // authentication successful so generate jwt token
                    string token = GenerateToken(claimsIdentity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    SetAuthCookiesForClient(user, token);
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid username and password.";
                }
                return View(model);
            }
            return View(model);
        }

        private void SetAuthCookiesForClient(UserEntity user, string token)
        {
            SetCookie(nameof(user.Id), user.Id, 10);
            SetCookie(nameof(user.Token), token, 10);
        }

        private string GenerateToken(ClaimsIdentity claimsIdentity)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                IssuedAt = DateTime.Now,
                Issuer = "ask",
                Audience = "ask"
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public virtual ActionResult Error(int? statusCode = null)
        {
            ErrorViewModel model = new ErrorViewModel();
            string message = "An error occurred while processing your request";
            if (statusCode.HasValue)
            {
                switch (statusCode)
                {
                    case 401:
                        message = "You are Unauthorized to access this page. Please contact the administrator";
                        break;
                    case 404:
                        message = "We were unable to find the page you requested. Where did you find te URL from ?";
                        break;
                    case 500:
                        message = "A server error occurred while processing your request"; ;
                        break;
                    default:
                        message = message;
                        break;

                }
            }
            else
            {
                statusCode = 0;
            }
            model.StatusCode = statusCode.ToString();
            model.Message = message;
            return View(model);
        }

        [HttpGet]
        public IActionResult Creator(string id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            if (tokenHandler.CanReadToken(id))
            {
                var claimsPrip = ValidateToken(id);
                ViewBag.Name = claimsPrip.Claims.Where(c => c.Type == "Email").FirstOrDefault().Value;

            }
            return View();

        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = "ask".ToLower();
            validationParameters.ValidIssuer = "ask".ToLower();

            validationParameters.IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);


            return principal;
        }

    }
}
