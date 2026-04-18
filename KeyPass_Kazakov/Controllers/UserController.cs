using KeyPass_Kazakov.Classes;
using KeyPass_Kazakov.Models;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace KeyPass_Kazakov.Controllers
{
    [Route("/user")]
    public class UserController : Controller
    {
        private DatabaseManager dbManager;

        public UserController() =>
            this.dbManager = new DatabaseManager();

        /// <summary>
        /// Метод для аутентификации
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns> JWT токен или код ошибки</returns>
        [Route("login")]
        [HttpPost]
        public ActionResult Login([FromForm] string login, [FromForm] string password)
        {
            try
            {
                User? user = dbManager.Users.FirstOrDefault(x => x.Login == login);
                bool isPassValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

                if (user == null)
                    return Unauthorized("Неверный логин или пароль");

                if (!isPassValid)
                    return Unauthorized("Неверный логин или пароль");
                else
                {
                    string Token = JwtToken.Generate(user);
                    user.LastAuth = DateTime.Now;
                    dbManager.SaveChanges();
                    return Ok(new { token = Token });
                }
            }
            catch (Exception exp)
            {
                return StatusCode(501, exp.Message);
            }
        }

        /// <summary>
        /// Метод для регистрации
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        [Route("register")]
        [HttpPost]
        public ActionResult Register([FromForm] string login, [FromForm] string password)
        {
            try
            {
                bool AuthUser = dbManager.Users.Any(x => x.Login == login);

                if (AuthUser)
                    return BadRequest("Пользователь уже существует");
                
                string hPass = BCrypt.Net.BCrypt.HashPassword(password);

                User newUs = new User
                {
                    Login = login,
                    Password = hPass,
                    LastAuth = DateTime.Now
                };
                dbManager.Users.Add(newUs);
                dbManager.SaveChanges();

                return Ok( new {message = "Успешно зареган", userId = newUs.Id});
            }
            catch (Exception exp)
            {
                return StatusCode(501, exp.Message);
            }
        }
    }
}
