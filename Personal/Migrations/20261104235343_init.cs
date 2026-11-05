using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Personal.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SkillAccomplishments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillAccomplishments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SkillSynonyms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    SkillId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillSynonyms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillSynonyms_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillToSkillAccomplishments",
                columns: table => new
                {
                    SkillId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SkillAccomplishmentId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillToSkillAccomplishments", x => new { x.SkillId, x.SkillAccomplishmentId });
                    table.ForeignKey(
                        name: "FK_SkillToSkillAccomplishments_SkillAccomplishments_SkillAccomplishmentId",
                        column: x => x.SkillAccomplishmentId,
                        principalTable: "SkillAccomplishments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SkillToSkillAccomplishments_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Name",
                table: "Skills",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Type",
                table: "Skills",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_SkillSynonyms_Name",
                table: "SkillSynonyms",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SkillSynonyms_SkillId",
                table: "SkillSynonyms",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillToSkillAccomplishments_SkillAccomplishmentId",
                table: "SkillToSkillAccomplishments",
                column: "SkillAccomplishmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkillSynonyms");

            migrationBuilder.DropTable(
                name: "SkillToSkillAccomplishments");

            migrationBuilder.DropTable(
                name: "SkillAccomplishments");

            migrationBuilder.DropTable(
                name: "Skills");
        }
    }
}
