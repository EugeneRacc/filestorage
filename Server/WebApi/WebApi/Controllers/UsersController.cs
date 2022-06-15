using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers;

[Route("filestorage/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    public UsersController(IUnitOfWork uow, IMapper mapper)
    {
        _userService = new UserService(uow, mapper);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<UserModel>>> Get()
    {
        IEnumerable<UserModel> customers;
        try
        {
            customers = await _userService.GetAllAsync();
        }
        catch(Exception)
        {
            return BadRequest();
        }
        return Ok(customers);
    }
    

}