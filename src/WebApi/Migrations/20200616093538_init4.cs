using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class init4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_FeedbackBatchs_FeedbackBatchId",
                table: "Feedback");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_FeedbackBatchs_FeedbackBatchId",
                table: "Feedback",
                column: "FeedbackBatchId",
                principalTable: "FeedbackBatchs",
                principalColumn: "FeedbackBatchId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_FeedbackBatchs_FeedbackBatchId",
                table: "Feedback");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_FeedbackBatchs_FeedbackBatchId",
                table: "Feedback",
                column: "FeedbackBatchId",
                principalTable: "FeedbackBatchs",
                principalColumn: "FeedbackBatchId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
