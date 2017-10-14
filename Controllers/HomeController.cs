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

        public IActionResult Index([FromServices] Mojo.Framework.Templating.IEngine Engine, [FromServices]IHostingEnvironment env)
        {
            object vm = new {
                name = "Matt",
                today = DateTime.Now,
                words = new[] { "Hello", "World", "Templating", "Engines", "Aren't", "That", "Bad" }
            };
            string p = Path.Combine(env.WebRootPath, "greeting.hbs");
            var template = Engine.Compile(p);
            string html = template.Render(vm);
            p = Path.Combine(env.WebRootPath, "test.dot");
            template = Engine.Compile(p);
            html += "\n" + template.Render(vm);
            return Ok(html);
        }
    }
}
