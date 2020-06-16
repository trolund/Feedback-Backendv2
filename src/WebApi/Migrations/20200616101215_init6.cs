using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class init6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_FeedbackBatchs_FeedbackBatchId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackBatchs_Meetings_MeetingId",
                table: "FeedbackBatchs");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_FeedbackBatchs_FeedbackBatchId",
                table: "Feedback",
                column: "FeedbackBatchId",
                principalTable: "FeedbackBatchs",
                principalColumn: "FeedbackBatchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackBatchs_Meetings_MeetingId",
                table: "FeedbackBatchs",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "MeetingId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_FeedbackBatchs_FeedbackBatchId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackBatchs_Meetings_MeetingId",
                table: "FeedbackBatchs");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_FeedbackBatchs_FeedbackBatchId",
                table: "Feedback",
                column: "FeedbackBatchId",
                principalTable: "FeedbackBatchs",
                principalColumn: "FeedbackBatchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackBatchs_Meetings_MeetingId",
                table: "FeedbackBatchs",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "MeetingId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
