using LinguaContext.Areas.User.Controlles;
using LinguaContext.DataAccess.Repository.Interfaces;
using LinguaContext.Models;
using LinguaContext.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinguaContext.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "admin")]
public class ManagementController : Controller
{
    //private readonly IUnitOfWork _unitOfWork;

    public ManagementController()
    {
        //_unitOfWork = unitOfWork;
    }

    [HttpGet]
    public IActionResult Index()
    {
        //List<Sentence> sentences = _unitOfWork.Sentence.GetAll().ToList();
        return View();
    }
}
