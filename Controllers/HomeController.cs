using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using offyesproj.Models;
using Microsoft.EntityFrameworkCore;


namespace offyesproj.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet("enterroom")]
        public IActionResult EnterRoom()
        {
            return View();
        }

        [HttpGet("answers")]
        public IActionResult DisplayAnswer()
        {
            return View();
        }
        
    }
}
