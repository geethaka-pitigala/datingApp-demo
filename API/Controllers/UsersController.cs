﻿using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController(DataContext dataContext): ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> getUsers(){
        var users = await dataContext.Users.ToListAsync();
        return users;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppUser>> getUser(int id){
        var user = await dataContext.Users.FindAsync(id);
        if(user == null) return NotFound();
        return user;
    }
}
