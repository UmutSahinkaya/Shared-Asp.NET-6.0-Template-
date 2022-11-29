using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    public partial class BookCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BookTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WriterName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GivenDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfPrint = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsStock = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
