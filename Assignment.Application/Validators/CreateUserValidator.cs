using Assignment.Application.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Application.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Name is required")
                .Matches("^[a-zA-Z ]+$").WithMessage("Name must contain only alphabetic characters.");
           

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email address");
          

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required");

            RuleFor(x => x.ICNumber)
                .NotEmpty().WithMessage("IC Number is required");


        }
    }
}
