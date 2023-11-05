using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LinguaContext.Areas.User.Controlles;

[Area("User")]
public class TaskController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Roles = "user,admin")]
    [HttpGet]
    public IActionResult GetInfo()
    {
        return Ok("You are stupid!");
    }

    [HttpGet]
    public IActionResult WhatAmI()
    {
        var currentUser = HttpContext.User;
        if (currentUser != null)
        {
            return View(currentUser);
        }
        return Ok("You are stupid!");
    }

}
