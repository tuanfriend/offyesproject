﻿using System;
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

        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
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
            return RedirectToAction("AddAnswer", new { Queid = newQue.QuestionID });
        }

        [HttpGet("addanswers/{Queid}")]
        public IActionResult AddAnswer(int Queid)
        {
            Question newque = dbContext.Questions.SingleOrDefault(u => u.QuestionID == Queid);
            ViewBag.QueName = newque.QuestionText;
            ViewBag.QueID = newque.QuestionID;
            HttpContext.Session.SetInt32("QuestionID", newque.QuestionID);
            return View();
        }

        [HttpPost("btAddAnswer")]
        public IActionResult Bt_Add_Answers()
        {

            Answer answer1 = new Answer();
            answer1.QuestionID = (int)HttpContext.Session.GetInt32("QuestionID");
            answer1.AnswerText = Request.Form["answer1"];
            if (Request.Form["ckanswer1"] == "true")
            {
                answer1.CorrectAnswer = true;
            }
            else
            {
                answer1.CorrectAnswer = false;
            }

            dbContext.Add(answer1);
            dbContext.SaveChanges();

            Answer answer2 = new Answer();
            answer2.QuestionID = (int)HttpContext.Session.GetInt32("QuestionID");
            answer2.AnswerText = Request.Form["answer2"];
            if (Request.Form["ckanswer2"] == "true")
            {
                answer2.CorrectAnswer = true;
                Console.WriteLine(answer2.CorrectAnswer);
            }
            else
            {
                answer2.CorrectAnswer = false;
            }
            dbContext.Add(answer2);
            dbContext.SaveChanges();

            Answer answer3 = new Answer();
            answer3.QuestionID = (int)HttpContext.Session.GetInt32("QuestionID");
            answer3.AnswerText = Request.Form["answer3"];
            if (Request.Form["ckanswer3"] == "true")
            {
                answer3.CorrectAnswer = true;
            }
            else
            {
                answer3.CorrectAnswer = false;
            }

            dbContext.Add(answer3);
            dbContext.SaveChanges();

            Answer answer4 = new Answer();
            answer4.QuestionID = (int)HttpContext.Session.GetInt32("QuestionID");
            answer4.AnswerText = Request.Form["answer4"];
            if (Request.Form["ckanswer4"] == "true")
            {
                answer4.CorrectAnswer = true;
            }
            else
            {
                answer4.CorrectAnswer = false;
            }
            dbContext.Add(answer4);
            dbContext.SaveChanges();

            return RedirectToAction("ReviewQuestion");

        }

        [HttpGet("reviewquestion")]
        public IActionResult ReviewQuestion()
        {
            ViewBag.RoomID = HttpContext.Session.GetInt32("RoomID");

            List<Question> listquestion = dbContext.Questions
            .Where(u => u.RoomID == HttpContext.Session.GetInt32("RoomID"))
            .Include(an => an.ListOfAnswers)
            .ToList();

            ViewBag.listquestion = listquestion;
            return View("ReviewQuestion");
        }

        [HttpGet("createroom")]
        public IActionResult CreateRoom()
        {
            return View("CreateRoom");
        }

        [HttpGet("ready/{roomid}")]
        public IActionResult Ready(int roomid)
        {
            ViewBag.listOfQuestions = dbContext.Questions.Where(qu => qu.RoomID == roomid).ToList();
            
            
            Room roomworking = dbContext.Rooms.SingleOrDefault(u => u.RoomID == roomid);
            ViewBag.RoomCode = roomworking.RoomCode;
            ViewBag.RoomID = roomid;

            ViewBag.num = ViewBag.listOfQuestions[0].QuestionID;
            //return View("LoadQuestion", new{ roomid = roomid, questionid = ViewBag.num});
            // int num = 0;
            // num++;
            // int questid = ViewBag.listOfQuestions[num].QuestionID;
            // if(questid == null){
            //     return RedirectToAction("index");
            // }else{
            //     return View("Ready", new { roomid = roomid, qid = questid });
            // }
            return View();
        }

        [HttpGet("loadquestion/{roomid}/{questionid}")]
        public IActionResult LoadQuestion(int roomid, int questionid)
        {
            ViewBag.question = dbContext.Questions.FirstOrDefault(a => a.QuestionID == questionid);

            //ViewBag.listOfQuestions = dbContext.Questions.Where(qu => qu.RoomID == roomid).ToList();
            //int questid = questionid; //13
           // questid ++;
            //int? nextquestid = ViewBag.listOfQuestions[questid].QuestionID;
           // if(nextquestid == null){
            //    return RedirectToAction("ResultPage");
            //}else{
           //     return RedirectToAction("LoadQuestion", {new = })
           // }
          //  ViewBag.questionnext = dbContext.Questions.FirstOrDefault(a => a.QuestionID == questid);
            
            // Room currentroom = dbContext.Rooms
            // .Include(q => q.ListOfUsers)
            // .ThenInclude(u => u.User)
            // .FirstOrDefault(r => r.RoomID == roomid);

            // List<Question> listquestionInRoom = dbContext.Questions
            // .Where(u => u.RoomID == roomid)
            // .Include(an => an.ListOfAnswers)
            // .ToList();
            // ViewBag.listquestionInRoom = listquestionInRoom;

            ViewBag.RoomID = roomid;

            return View("LoadQuestion");
        }

        [HttpPost("loadnextquestion")]
        public IActionResult LoadNextQuestion()
        {
            int currentquestionID = Int32.Parse(Request.Form["currentquestionID"]);
            currentquestionID++;
            int currentroomID = Int32.Parse(Request.Form["roomID"]);

            ViewBag.listOfQuestions = dbContext.Questions.Where(qu => qu.RoomID == currentroomID).ToList();
            
            Question checkquestion = dbContext.Questions
            .Where(qu => qu.RoomID == currentroomID)
            .FirstOrDefault(ts => ts.QuestionID == currentquestionID);
            if(checkquestion == null){
                return RedirectToAction("ResultPage");
            }else{
                return RedirectToAction("LoadQuestion", new {roomid = currentroomID, questionid = currentquestionID});
            }
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
                HttpContext.Session.SetInt32("RoomID", newRoom.RoomID);
                return RedirectToAction("AddQuestion", new { id = newRoom.RoomID });
            }
            else
            {
                return View("CreateRoom");
            }
        }

        //Enter passcode for enter room for user
        [HttpGet("enterroom")]
        public IActionResult EnterRoom()
        {
            return View();
        }

        [HttpPost("btEnterRoom")]
        public IActionResult btEnterRoom()
        {
            //If check correct passcode or no here
            Room roomworking = dbContext.Rooms.SingleOrDefault(u => u.RoomCode == Request.Form["RoomCode"]);
            if (roomworking == null)
            {
                return View("EnterRoom");
            }
            else
            {
                UserRoom userandroom = new UserRoom();
                userandroom.UserID = (int)HttpContext.Session.GetInt32("UserID");
                userandroom.RoomID = roomworking.RoomID;
                userandroom.Score = 0;
                userandroom.AnswerSheet = "okay";
                dbContext.Add(userandroom);
                dbContext.SaveChanges();
                return RedirectToAction("UserReady", new{userid = userandroom.UserID });
            }

        }

        [HttpGet("userready/{userid}")]
        public IActionResult UserReady(int userid)
        {
            return View();
        }
        
        [HttpGet("resultpage")]
        public IActionResult ResultPage()
        {
            return View("ResultPage");
        }
    }
}
