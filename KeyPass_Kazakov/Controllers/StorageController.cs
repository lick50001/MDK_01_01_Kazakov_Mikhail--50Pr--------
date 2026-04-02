using KeyPass_Kazakov.Classes;
using KeyPass_Kazakov.Models;
using Microsoft.AspNetCore.Mvc;

namespace KeyPass_Kazakov.Controllers
{
    [Route("/storage")]
    public class StorageController : Controller
    {
        private DatabaseManager dbManager;

        public StorageController() =>
            this.dbManager = new DatabaseManager();

        /// <summary>
        /// Получение записей хранилища для пользователя
        /// </summary>
        /// <param name="token">JWT токен из заголовка запроса</param>
        /// <returs>Список записей хранилища в формате DTO(без информации о пользователе)</returs>
        [Route("get")]
        [HttpGet]
        public ActionResult Get([FromHeader] string token)
        {
            try
            {
                int? IdUser = JwtToken.GetUserIdFromToken(token);

                if (IdUser == null)
                    return StatusCode(401);
                
                List<StorageDto> Storages = dbManager.Storages.Where(x => x.UserId == IdUser).Select(s => new StorageDto { Id = s.Id, Name = s.Name, Url = s.Url, Login = s.Login, Password = s.Password, }).ToList();
                return Ok(Storages);
            }
            catch (Exception exp)
            {
                return StatusCode(501, exp.Message);
            }
        }
        ///<summary>
        ///Добвление новой записи в хранилище
        ///</summary>
        ///<param name="token">Jwt токен из заголовка</param>
        ///<param name="storage">Данные новой записи (JSON в теле запроса)</param>
        ///<returns>Добавленная запись</returns>
        [Route("add")]
        [HttpPost]
        public ActionResult Add([FromHeader] string token, [FromBody] Storage storage)
        {
            try
            {
                int? userId = JwtToken.GetUserIdFromToken(token);

                if (userId == null)
                    return Unauthorized("Неверный токен");

                var user = dbManager.Users.Find(userId);

                if (user == null)
                    return NotFound("Пользователь не найден");

                storage.UserId = user.Id;
                storage.User = null;

                dbManager.Storages.Add(storage);
                dbManager.SaveChanges();

                return Ok(storage);
            }
            catch (Exception exp)
            {
                return StatusCode(501, exp.Message);
            }
        }
        ///<summary>
        ///Обновление существующей записи
        /// </summary>
        /// <param name="token">Jwt токен из заголовка</param>
        /// <param name="storage">Обновленные данные записи</param>
        /// <returns>Обновленная запись</returns>
        [Route("update")]
        [HttpPut]
        public ActionResult Update([FromHeader] string token, [FromBody] Storage storage )
        {
            try
            {
                int? userId = JwtToken.GetUserIdFromToken(token);
                if (userId == null)
                    return Unauthorized("Неверный токен");

                var existingStorage = dbManager.Storages
                    .FirstOrDefault(x => x.Id == storage.Id && x.UserId == userId);

                if (existingStorage == null)
                    return NotFound("Запись не найдена или у вас нет прав на её изменение");

                existingStorage.Name = storage.Name;
                existingStorage.Url = storage.Url;
                existingStorage.Login = storage.Login;
                existingStorage.Password = storage.Password;

                dbManager.SaveChanges ();
                return StatusCode(200, existingStorage);
            }
            catch (Exception exp)
            {
                return StatusCode(501, exp.Message);
            }
        }
        ///<summary>
        ///Удаление существующей записи
        /// </summary>
        /// <param name="token">Jwt токен из заголовка</param>
        /// <param name="id">ID Удаляемой  записи</param>
        /// <returns>Статус выполнения операции</returns>
        [Route("delete")]
        [HttpDelete]
        public ActionResult Delete([FromHeader] string token, [FromForm] int id)
        {
            try
            {
                int? IdUser = JwtToken.GetUserIdFromToken(token);

                if (IdUser == null)
                    return StatusCode(401);

                Storage? Storage = dbManager.Storages.Where(x => x.Id == id && x.User.Id == IdUser).FirstOrDefault();

                if (Storage == null)
                    return StatusCode(404);

                dbManager.Storages.Remove(Storage);
                dbManager.SaveChanges();

                return StatusCode(200);
            }
            catch (Exception exp)
            {
                return StatusCode(501, exp.Message);
            }
        }
    }
}
