using System.ComponentModel.DataAnnotations;
using System;
using offyesproj.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;


namespace offyesproj.Models

{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        // MySQL VARCHAR and TEXT types can be represeted by a string
        [Required]
        [MinLength(2, ErrorMessage = "Nick name has to be atleast 2 characters!")]
        public string NickName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(2, ErrorMessage = "Password has to be atleast 2 characters!")]
        public string Password { get; set; }
        // The MySQL DATETIME type can be represented by a DateTime
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; } = DateTime.Now;

        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPw { get; set; }
        public List<UserRoom> ListOfRooms { get; set; }

    }
    public class LoginUser
    {
        // No other fields!
        [Required(ErrorMessage = "Please enter correct Nick Name")]
        [EmailAddress]
        public string LoginNickName { get; set; }

        [Required(ErrorMessage = "Please enter your password!")]
        [DataType(DataType.Password)]
        public string LoginPassword { get; set; }
    }

    public class Room
    {
        [Key]
        public int RoomID { get; set; }
        public string RoomCode {get; set;}

        public List<UserRoom> ListOfUsers { get; set; }
        public List<Question> ListOfQuestions { get; set; }
    }

    public class UserRoom
    {
        [Key]
        public int UserRoomID { get; set; }
        public User User { get; set; }
        public Room Room { get; set; }
        public int Score {get; set;}
        public string AnswerSheet {get; set;}
    }

    public class Question
    {
        [Key]
        public int QuestionID {get; set;}

        public int RoomID { get; set; }
        public string QuestionText {get; set;}
        public int Point {get; set;} = 1;
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; } = DateTime.Now;
        public List<Answer> ListOfAnswers { get; set; }
    }

    public class Answer
    {
        [Key]
        public int AnswerID {get; set;}

        public int QuestionID {get; set;}
        public string AnswerText {get; set;}
        public bool CorrectAnswer {get; set;} = false;
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; } = DateTime.Now;
    }


}