using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    public class Fallback : Controller
    {
        public IActionResult Index()
        {
            //rediriger toutes les autres req comme /members à index pour les gérer via angular router. index.html est appelée auto au début grace à la config dans startup
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/HTML");
            
        }   
    }
}