using Microsoft.AspNetCore.Mvc;
namespace Caso1.Controllers;
public class HomeController : Controller
{
    public IActionResult Index() => View();
    public IActionResult Error() => View();
}
