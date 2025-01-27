
using AutoMapper;
using Grpc.Core;
using GrpcClientApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace GrpcClientApi.Controllers
{
    /// <summary>
    /// Контроллер пользователя
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserService.UserServiceClient _client;
        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger, UserService.UserServiceClient client, IMapper mapper)
        {
            _logger = logger;
            _client = client;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение всех пользователей.
        /// </summary>
        /// <remarks>
        /// Данный метод позволяет получить список всех пользователей. 
        /// </remarks>
        /// <response code="200">Получение списка пользователей</response>
        [HttpGet]
        [ProducesResponseType<UserModel>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            // получение списка объектов
            var users = await _client.ListUsersAsync(new Google.Protobuf.WellKnownTypes.Empty());

            return StatusCode(StatusCodes.Status200OK, _mapper.Map<ICollection<UserModel>>(users.Users.ToList()));
        }

        /// <summary>
        /// Получение объекта пользователя. Получение списка всех пользователей.
        /// </summary>
        /// <remarks>Данный метод возвращает массив пользователей</remarks>
        /// <param name="id">ID пользователя. Оставить пустым для получения полного списка пользователей</param>
        /// <response code="200">Массив пользователей. При указании ID массив будет состоять из одного элемента</response>
        /// <response code="404">Пользователь не найден по указанному идентификатору</response>
        [HttpGet]
        [ProducesResponseType<UserModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            try
            {
                // получение одного объекта по id 
                UserReply user = await _client.GetUserAsync(new GetUserRequest { Id = id });
                return StatusCode(StatusCodes.Status200OK, _mapper.Map<UserModel>(user));
            }
            catch (RpcException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Status.Detail);
            }
        }

        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <remarks>
        /// Данный метод позволяет создать нового пользователя
        /// </remarks>
        /// <response code="200">Новый пользователь успешно создан</response>
        /// <response code="400">Ошибка при создании нового пользователя</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] UserCreateModel model)
        {
            // добавление одного объекта
            UserReply user = await _client.CreateUserAsync(_mapper.Map<CreateUserRequest>(model));

            if (user != null)
                return StatusCode(StatusCodes.Status200OK);
            else
                return StatusCode(StatusCodes.Status400BadRequest);
        }

        /// <summary>
        /// Обновление пользователя
        /// </summary>
        /// <remarks>
        /// Данный метод позволяет обновлять информацию существующего пользователя. Подробное описание свойств  -  см. схему UserEditViewModel
        /// </remarks>
        /// <response code="200">Возвращает объект пользователя с обновленными данными</response>
        /// <response code="400">Ошибка при обновлении пользователя</response>
        /// <response code="404">Пользователь для обновления не найден</response>
        [HttpPut]
        [ProducesResponseType<UserModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UserModel model)
        {
            try
            {
                //обновление одного объекта - изменим имя у объекта с id = 1 на Tomas
                UserReply user = await _client.UpdateUserAsync(_mapper.Map<UpdateUserRequest>(model));
                if (user != null)
                    return StatusCode(StatusCodes.Status200OK);
                else
                    return StatusCode(StatusCodes.Status400BadRequest);
            }
            catch (RpcException ex)
            {
                Console.WriteLine(ex.Status.Detail);
                return StatusCode(StatusCodes.Status404NotFound, ex.Status.Detail);
            }
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <remarks>Данный метод позволяет удалить пользователя</remarks>
        /// <param name="id">Идентификатор пользователя, которого необходимо удалить</param>
        /// <response code="200">Возвращает объект пользователя, который был удалён</response>
        /// <response code="400">Ошибка при удалении пользователя</response>
        /// <response code="404">Пользователь для удаления не найден</response>
        [HttpDelete]
        [Produces("application/json")]
        [Route("{id}")]
        [ProducesResponseType<UserModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                // удаление объекта с id = 2
                UserReply user = await _client.DeleteUserAsync(new DeleteUserRequest { Id = id });
                if (user != null)
                    return StatusCode(StatusCodes.Status200OK);
                else
                    return StatusCode(StatusCodes.Status400BadRequest);
            }
            catch (RpcException ex)
            {
                Console.WriteLine(ex.Status.Detail);
                return StatusCode(StatusCodes.Status404NotFound, ex.Status.Detail);
            }
        }
    }
}
