using KeyPass_Kazakov.Classes;
using KeyPass_Kazakov.Models;
using Microsoft.AspNetCore.Mvc;

namespace KeyPass_Kazakov.Controllers
{
    [Route("/user")]
    public class UserController : Controller
    {
        private DatabaseManager dbManager;

        public UserController() =>
            this.dbManager = this.dbManager = new DatabaseManager();

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
                User? AuthUser = dbManager.Users.Where(x => x.Login == login && x.Password == password).FirstOrDefault();

                if (AuthUser == null)
                    return StatusCode(401);
                else
                {
                    string Token = JwtToken.Generate(AuthUser);
                    AuthUser.LastAuth = DateTime.Now;
                    dbManager.SaveChanges();
                    return Ok(new { token = Token });
                }
            }
            catch (Exception exp)
            {
                return StatusCode(501, exp.Message);
            }
        }
    }
}
