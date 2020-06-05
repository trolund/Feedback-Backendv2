using System;
using System.ComponentModel.DataAnnotations;
using Infrastructure.ViewModels;

public class MeetingDateAttribute : ValidationAttribute {

    protected override ValidationResult IsValid (object value,
        ValidationContext validationContext) {
        var meeting = (MeetingDTO) validationContext.ObjectInstance;

        if (meeting.StartTime > meeting.EndTime) {
            return new ValidationResult ("Meeting start date must be before end date.");
        } else if (meeting.EndTime.Equals (meeting.StartTime)) {
            return new ValidationResult ("Start and end time must be different.");
        } else if (!meeting.StartTime.Month.Equals (meeting.EndTime.Month) && meeting.StartTime.Day.Equals (meeting.EndTime.Day) && meeting.StartTime.Year.Equals (meeting.EndTime.Year)) {
            return new ValidationResult ("Start and end time must be on the same day.");
        }

        return ValidationResult.Success;
    }
}