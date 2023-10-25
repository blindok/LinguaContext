using LinguaContext.Areas.User.Controlles;
using LinguaContext.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LinguaContext.Areas.Admin.Controllers;

[Area("Admin")]
public class SentenceManageController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public SentenceManageController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
