using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using templater.Models;

namespace templater.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Test([FromServices] Mojo.Framework.Templating.IEngine Engine, [FromServices]IHostingEnvironment env)
        {
            string p = Path.Combine(env.WebRootPath, "greeting.hbs");
            var template = Engine.Compile(p);
            return Ok(template.Render(new { name = "Matt", today = DateTime.Now }));
        }
    }
}
