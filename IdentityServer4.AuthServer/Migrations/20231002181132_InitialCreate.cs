using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityServer4.AuthServer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomUsers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CustomUsers",
                columns: new[] { "Id", "City", "Email", "Name", "Password", "Surname", "Username" },
                values: new object[] { "25c946d0-eb7a-4279-a24b-7fb0a094d2cd", "Amasya", "mehmet.hasan@gmail.com", "Mehmet", "test.123", "Hasan", "mehmet.hasan" });

            migrationBuilder.InsertData(
                table: "CustomUsers",
                columns: new[] { "Id", "City", "Email", "Name", "Password", "Surname", "Username" },
                values: new object[] { "3a3aa9ee-d20f-4317-bbf1-b798c3fb0fa8", "İzmir", "murat.resulogullari1@gmail.com", "Murat", "test.123", "Resuloğulları", "murat.resulogullari" });

            migrationBuilder.InsertData(
                table: "CustomUsers",
                columns: new[] { "Id", "City", "Email", "Name", "Password", "Surname", "Username" },
                values: new object[] { "e68b1917-cb76-49a8-b4c2-621763f77f1d", "Konya", "ali.veli@gmail.com", "Ali", "test.123", "Veli", "ali.veli" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomUsers");
        }
    }
}
