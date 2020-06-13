using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackBatchs_QuestionSets_QuestionSetId1",
                table: "FeedbackBatchs");

            migrationBuilder.DropIndex(
                name: "IX_FeedbackBatchs_QuestionSetId",
                table: "FeedbackBatchs");

            migrationBuilder.DropIndex(
                name: "IX_FeedbackBatchs_QuestionSetId1",
                table: "FeedbackBatchs");

            migrationBuilder.DropColumn(
                name: "QuestionSetId1",
                table: "FeedbackBatchs");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackBatchs_QuestionSetId",
                table: "FeedbackBatchs",
                column: "QuestionSetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FeedbackBatchs_QuestionSetId",
                table: "FeedbackBatchs");

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionSetId1",
                table: "FeedbackBatchs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackBatchs_QuestionSetId",
                table: "FeedbackBatchs",
                column: "QuestionSetId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackBatchs_QuestionSetId1",
                table: "FeedbackBatchs",
                column: "QuestionSetId1");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackBatchs_QuestionSets_QuestionSetId1",
                table: "FeedbackBatchs",
                column: "QuestionSetId1",
                principalTable: "QuestionSets",
                principalColumn: "QuestionSetId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
