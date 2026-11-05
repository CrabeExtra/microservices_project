using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Personal.Migrations
{
    /// <inheritdoc />
    public partial class skill_synonym_names_non_unique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SkillSynonyms_Name",
                table: "SkillSynonyms");

            migrationBuilder.CreateIndex(
                name: "IX_SkillSynonyms_Name",
                table: "SkillSynonyms",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SkillSynonyms_Name",
                table: "SkillSynonyms");

            migrationBuilder.CreateIndex(
                name: "IX_SkillSynonyms_Name",
                table: "SkillSynonyms",
                column: "Name",
                unique: true);
        }
    }
}
