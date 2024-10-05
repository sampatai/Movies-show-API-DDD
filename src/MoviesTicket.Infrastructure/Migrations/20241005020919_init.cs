using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviesTicket.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateSequence(
                name: "eventseq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "movieseq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "reservationseq",
                schema: "dbo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "showtimeseq",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "EventLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EventType_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventType_Id = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    MovieGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Genres_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Genres_Id = table.Column<int>(type: "int", nullable: false),
                    Runtime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Director = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Synopsis = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShowsTime",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ShowsTimeGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShowDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowsTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShowsTime_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShowsTime_Reservation",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShowsTimeGUID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowsTime_Reservation", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK_ShowsTime_Reservation_ShowsTime_ShowsTimeGUID",
                        column: x => x.ShowsTimeGUID,
                        principalTable: "ShowsTime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_MovieGUID",
                table: "Movies",
                column: "MovieGUID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShowsTime_MovieId",
                table: "ShowsTime",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_ShowsTime_ShowsTimeGUID",
                table: "ShowsTime",
                column: "ShowsTimeGUID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShowsTime_Reservation_ShowsTimeGUID",
                table: "ShowsTime_Reservation",
                column: "ShowsTimeGUID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventLogs");

            migrationBuilder.DropTable(
                name: "ShowsTime_Reservation");

            migrationBuilder.DropTable(
                name: "ShowsTime");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropSequence(
                name: "eventseq");

            migrationBuilder.DropSequence(
                name: "movieseq");

            migrationBuilder.DropSequence(
                name: "reservationseq",
                schema: "dbo");

            migrationBuilder.DropSequence(
                name: "showtimeseq");
        }
    }
}
