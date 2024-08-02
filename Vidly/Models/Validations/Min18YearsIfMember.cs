using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vidly2.DTOs;

namespace Vidly2.Models.Validations
{
    public class Min18YearsIfMember : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // to handle dto validation that uses the same attribute
            var customer = new Customer();
            if (validationContext.ObjectType == typeof(Customer))
                customer = validationContext.ObjectInstance as Customer;
            else
                customer = Mapper.Map(validationContext.ObjectInstance as CustomerDto, customer);

            //if (customer.MembershipTypeId == 0 || customer.MembershipTypeId == 1)
            if (customer.MembershipTypeId == MembershipType.Unknown || customer.MembershipTypeId == MembershipType.PayAsYouGo)
                return ValidationResult.Success;

            if (customer.Birthdate == null)
                return new ValidationResult("Birthdate is required.");

            var age = DateTime.Today.Year - customer.Birthdate.Value.Year;
            return age >= 18 ? ValidationResult.Success : new ValidationResult("Customer should be at least 18 years old to be on a membership.");
        }
    }
}