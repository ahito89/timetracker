using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using timetracker.Models;
using System.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Principal;

namespace timetracker.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JWTSettings _options;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IOptions<JWTSettings> optionsAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _options = optionsAccessor.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if(result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return new JsonResult( new Dictionary<string, object>
                    {
                        { "access_token", GetToken(user) }
                    });
                }
                return Errors(result);
            }
            return Error("Unexpected error");
        }

        private string GetToken(IdentityUser user)
        {
            var handler = new JwtSecurityTokenHandler();
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretKey));

            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(user.UserName, "Token"),
                new[]{
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                }
            );

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                Subject = identity,
                Expires = DateTime.Now.Add(TimeSpan.FromDays(1)),
                NotBefore = DateTime.Now
            });

            return handler.WriteToken(securityToken);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
                if(result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    return new JsonResult( new Dictionary<string, object>
                    {
                        { "access_token", GetToken(user) }
                    });
                }
                return new JsonResult("Unable to sign in") { StatusCode = 401 };
            }
            return Error("Unexpected error");
        }

        private JsonResult Errors(IdentityResult result)
        {
            var items = result.Errors
                .Select(x => x.Description)
                .ToArray();
            return new JsonResult(items) { StatusCode = 400 };
        }

        private JsonResult Error(string message)
        {
            return new JsonResult(message) { StatusCode = 400 };
        }
    }
}
