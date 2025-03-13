using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactManager.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePhoneEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "CategoryId", "Name" },
                values: new object[] { 2, 2, "Jane Smith" });

            migrationBuilder.InsertData(
                table: "Emails",
                columns: new[] { "Id", "ContactId", "EmailAddress", "IsPrimary" },
                values: new object[] { 2, 2, "jane.smith@example.com", true });

            migrationBuilder.InsertData(
                table: "Phones",
                columns: new[] { "Id", "ContactId", "IsPrimary", "PhoneNumber" },
                values: new object[] { 2, 2, true, "987-654-3210" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Emails",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Phones",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
