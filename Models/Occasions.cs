using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace belt.Models {
    public class FutureDateAttribute : ValidationAttribute {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext){
            if (value == null){
                return new ValidationResult("Wedding date is required");
            }
            if ((DateTime)value <= DateTime.Now) {
                return new ValidationResult("Event must take place in future");
            } else {
                return ValidationResult.Success;
            }
        }
    }

    public class Occasion {
        [Key]
        public int OccasionId {get;set;}

        [Required]
        public int CoordinatorId {get;set;}
        
        public User CoordinatorUser {get;set;}

        [Required]
        [Display(Name = "Title")]
        public string Title {get;set;}

        [Display(Name = "Time")]
        [DataType(DataType.Time)]
        public DateTime Time {get;set;}

        [Display(Name = "Date")]
        // [FutureDate]
        [DataType(DataType.Date)]
        public DateTime Date {get;set;}

        [Required]
        [Display(Name = "Duration")]
        [Range(0, int.MaxValue)]
        public int DurationInt {get;set;}

        [Required]
        public string DurationString {get;set;}

        [Required]
        [Display(Name = "Description")]
        public string Description {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public List<Association> Attendants {get;set;}
    }
}