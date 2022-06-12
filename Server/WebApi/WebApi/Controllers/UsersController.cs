using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

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


    
    [HttpPost("register")]
    public async Task<ActionResult> Add([FromBody] UserModel value)
    {
        try
        {
            await _userService.AddAsync(value);
        }
        catch(Exception)
        {
            return BadRequest(StatusCode(400));
        }
        return CreatedAtAction(nameof(Add), new { id = value.Id }, value);
    }


}