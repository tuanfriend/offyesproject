﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using offyesproj.Models;

namespace offyesproj.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20190327172557_YourMigrationfrisst")]
    partial class YourMigrationfrisst
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("offyesproj.Models.Answer", b =>
                {
                    b.Property<int>("AnswerID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AnswerText");

                    b.Property<bool>("CorrectAnswer");

                    b.Property<DateTime>("Created_at");

                    b.Property<int>("QuestionID");

                    b.Property<DateTime>("Updated_at");

                    b.HasKey("AnswerID");

                    b.HasIndex("QuestionID");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("offyesproj.Models.Question", b =>
                {
                    b.Property<int>("QuestionID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created_at");

                    b.Property<int>("Point");

                    b.Property<string>("QuestionText");

                    b.Property<int>("RoomID");

                    b.Property<int>("Timer");

                    b.Property<DateTime>("Updated_at");

                    b.HasKey("QuestionID");

                    b.HasIndex("RoomID");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("offyesproj.Models.Room", b =>
                {
                    b.Property<int>("RoomID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("RoomCode")
                        .IsRequired();

                    b.HasKey("RoomID");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("offyesproj.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created_at");

                    b.Property<string>("NickName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<DateTime>("Updated_at");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("offyesproj.Models.UserRoom", b =>
                {
                    b.Property<int>("UserRoomID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AnswerSheet");

                    b.Property<int>("RoomID");

                    b.Property<int>("Score");

                    b.Property<int>("UserID");

                    b.HasKey("UserRoomID");

                    b.HasIndex("RoomID");

                    b.HasIndex("UserID");

                    b.ToTable("UserRooms");
                });

            modelBuilder.Entity("offyesproj.Models.Answer", b =>
                {
                    b.HasOne("offyesproj.Models.Question")
                        .WithMany("ListOfAnswers")
                        .HasForeignKey("QuestionID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("offyesproj.Models.Question", b =>
                {
                    b.HasOne("offyesproj.Models.Room")
                        .WithMany("ListOfQuestions")
                        .HasForeignKey("RoomID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("offyesproj.Models.UserRoom", b =>
                {
                    b.HasOne("offyesproj.Models.Room", "Room")
                        .WithMany("ListOfUsers")
                        .HasForeignKey("RoomID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("offyesproj.Models.User", "User")
                        .WithMany("ListOfRooms")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
