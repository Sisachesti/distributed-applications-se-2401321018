using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Learn2Gether.Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Description", "Duration", "ImageUrl", "InstructorId", "Rating", "Title" },
                values: new object[,]
                {
                    { new Guid("a2b3c4d5-e6f7-489a-bcde-f0123456789a"), "Master data access with Entity Framework Core in .NET applications.", 180, "/photos/efcore.jpg", new Guid("993d3f82-4b45-4677-8859-c140569263fc"), 4.5999999999999996, "Entity Framework Core Essentials" },
                    { new Guid("e0a1b2c3-d4e5-4678-9abc-def012345678"), "A practical course covering core C# concepts and hands-on examples.", 200, "/photos/csharp.jpg", new Guid("993d3f82-4b45-4677-8859-c140569263fc"), 4.7000000000000002, "Introduction to C#" },
                    { new Guid("f1b2c3d4-e5f6-4789-abcd-ef0123456789"), "Learn to build robust web applications using ASP.NET Core framework.", 250, "/photos/aspnetcore.jpg", new Guid("993d3f82-4b45-4677-8859-c140569263fc"), 4.7999999999999998, "Web Development with ASP.NET Core" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("a2b3c4d5-e6f7-489a-bcde-f0123456789a"));

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("e0a1b2c3-d4e5-4678-9abc-def012345678"));

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("f1b2c3d4-e5f6-4789-abcd-ef0123456789"));
        }
    }
}
