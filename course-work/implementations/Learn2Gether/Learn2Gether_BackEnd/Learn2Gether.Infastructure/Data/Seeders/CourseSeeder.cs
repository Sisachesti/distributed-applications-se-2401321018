using System;
using System.Collections.Generic;
using Learn2Gether.Domain.Entities;

namespace Learn2Gether.Infastructure.Data.Seeders
{
    public static class CourseSeeder
    {
        public static List<Course> Seed()
        {
            List<Course> courses = new List<Course>
            {
                new Course
                {
                    Id = Guid.Parse("E0A1B2C3-D4E5-4678-9ABC-DEF012345678"),
                    Title = "Introduction to C#",
                    Rating = 4.7,
                    Duration = 200,
                    ImageUrl = "/photos/csharp.jpg",
                    Description = "A practical course covering core C# concepts and hands-on examples.",
                    InstructorId = Guid.Parse("993D3F82-4B45-4677-8859-C140569263FC")
                },
                new Course
                {
                    Id = Guid.Parse("F1B2C3D4-E5F6-4789-ABCD-EF0123456789"),
                    Title = "Web Development with ASP.NET Core",
                    Rating = 4.8,
                    Duration = 250,
                    ImageUrl = "/photos/aspnetcore.jpg",
                    Description = "Learn to build robust web applications using ASP.NET Core framework.",
                    InstructorId = Guid.Parse("993D3F82-4B45-4677-8859-C140569263FC")
                },
                new Course
                {
                    Id = Guid.Parse("A2B3C4D5-E6F7-489A-BCDE-F0123456789A"),
                    Title = "Entity Framework Core Essentials",
                    Rating = 4.6,
                    Duration = 180,
                    ImageUrl = "/photos/efcore.jpg",
                    Description = "Master data access with Entity Framework Core in .NET applications.",
                    InstructorId = Guid.Parse("993D3F82-4B45-4677-8859-C140569263FC")
                },
            };

            return courses;
        }
    }
}
