using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using offyesproj.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;


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

        [HttpPost("btRegister")]
        public IActionResult btRegister(User user)
        {
            // Check initial ModelState
            if (ModelState.IsValid)
            {
                // If a User exists with provided email

                if (dbContext.Users.Any(u => u.NickName == user.NickName))
                {
                    // Manually add a ModelState error to the Email field, with provided
                    // error message
                    ModelState.AddModelError("NickName", "NickName already in use!");
                    // You may consider returning to the View at this point
                    return View("Register");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                dbContext.Add(user);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("UserID", user.UserID);
                HttpContext.Session.SetString("Session", "True");
                return RedirectToAction("Dashboard");
            }
            return View("Register");
            // other code
        }

        [HttpPost("btLogin")]
        public IActionResult btLogin(LoginUser userSubmission)
        {
            if (ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var userInDb = dbContext.Users.FirstOrDefault(u => u.NickName == userSubmission.LoginNickName);
                // If no user exists with provided email
                if (userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("NickName", "Invalid Email/Password");
                    return View("Index");
                }

                // Initialize hasher object
                var hasher = new PasswordHasher<LoginUser>();

                // varify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);

                // result can be compared to 0 for failure
                if (result == 0)
                {
                    ModelState.AddModelError("LoginPassword", "Invalid Password");
                    return View("Index");
                    // handle failure (this should be similar to how "existing email" is handled)
                }
                HttpContext.Session.SetInt32("UserID", userInDb.UserID);
                HttpContext.Session.SetString("Session", "True");
                return RedirectToAction("Dashboard");
            }
            return View("Index");
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

        [HttpGet("addquestion/{id}")]
        public IActionResult AddQuestion(int id)
        {
            ViewBag.RoomID = id;
            return View();
        }

        [HttpPost("btAddquestion")]
        public IActionResult Bt_Add_Questions(Question newQue)
        {
                dbContext.Add(newQue);
                dbContext.SaveChanges();
                return RedirectToAction("AddQuestion", new { id = newQue.RoomID });
        }

        [HttpGet("addanswers")]
        public IActionResult AddAnswer()
        {
            return View();
        }

        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        
        [HttpGet("createroom")]
        public IActionResult CreateRoom()
        {
            return View("CreateRoom");
        }

        [HttpGet("loadquestion")]
        public IActionResult LoadQuestion()
        {
            return View("LoadQuestion");
        }

        [HttpGet("ready")]
        public IActionResult Ready()
        {
            return View("Ready");
        }

        [HttpPost("Bt_Create_New_Room")]
        public IActionResult Bt_Create_New_Room(Room newRoom)
        {
            if (ModelState.IsValid)
            {
                if (dbContext.Rooms.Any(u => u.RoomCode == newRoom.RoomCode))
                {
                    // Manually add a ModelState error to the Room field, with provided
                    // error message
                    ModelState.AddModelError("RoomCode", "Room Code already in use!");
                    // You may consider returning to the View at this point
                    return View("CreateRoom");
                }
                dbContext.Add(newRoom);
                dbContext.SaveChanges();
                return RedirectToAction("AddQuestion", new { id = newRoom.RoomID });
            }
            else
            {
                return View("CreateRoom");
            }
        }
    }
}
