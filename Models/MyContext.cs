using Microsoft.EntityFrameworkCore;

namespace offyesproj.Models
{
    public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users {get;set;}
        public DbSet<Room> Rooms {get;set;}
        public DbSet<UserRoom> UserRooms {get;set;} //in the middle
        public DbSet<Question> Questions {get;set;}
        public DbSet<Answer> Answers {get;set;}
    }
}