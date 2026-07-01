using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Audit.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OldData = table.Column<string>(type: "TEXT", nullable: true),
                    NewData = table.Column<string>(type: "TEXT", nullable: true),
                    MicroserviceName = table.Column<string>(type: "TEXT", nullable: false),
                    EntityName = table.Column<string>(type: "TEXT", nullable: false),
                    Action = table.Column<string>(type: "TEXT", nullable: false),
                    EventType = table.Column<string>(type: "TEXT", nullable: false),
                    ReferenceId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Records_Action",
                table: "Records",
                column: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_Records_CreatedAt",
                table: "Records",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Records_EntityName",
                table: "Records",
                column: "EntityName");

            migrationBuilder.CreateIndex(
                name: "IX_Records_EventType",
                table: "Records",
                column: "EventType");

            migrationBuilder.CreateIndex(
                name: "IX_Records_MicroserviceName",
                table: "Records",
                column: "MicroserviceName");

            migrationBuilder.CreateIndex(
                name: "IX_Records_ReferenceId",
                table: "Records",
                column: "ReferenceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Records");
        }
    }
}
