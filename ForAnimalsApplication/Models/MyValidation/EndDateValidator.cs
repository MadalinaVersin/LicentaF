using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ForAnimalsApplication.Models.MyValidation
{
    public class EndDateValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Competition competition = (Competition)validationContext.ObjectInstance;
            DateTime startDate = competition.StartDate;
            DateTime endDate = competition.EndDate;
            string dateS = startDate.ToString("yyyy-MM-dd");
            string dateE = endDate.ToString("yyyy-MM-dd");
            if (startDate >= endDate)
            {
                return new ValidationResult("Data este incorecata! Data de inceput trebuie sa fie mai mica ca data de sfarsit!");
            } /*else if(dateE == dateS)
            {
                return new ValidationResult("O competitie trebuie sa dureze cel putin o zi");
            }*/
            return ValidationResult.Success;
        }
    }
}