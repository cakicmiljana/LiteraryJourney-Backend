using backend.model;
using Microsoft.AspNetCore.Mvc;

namespace backend.controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{

    public UserController()
    {

    }

    [HttpPost("CreateUser")]
    public async Task<ActionResult> CreateUser([FromBody] User user)
    {
        return Ok("User created!");
    }

    [HttpGet("GetUser/{id}")]
    public async Task<ActionResult> GetUser(int id)
    {
        return Ok("Hello from UserController!");
    }

    [HttpPut("UpdateUser")]
    public async Task<ActionResult> UpdateUser([FromBody] User user)
    {
        return Ok("User updated!");
    }
}